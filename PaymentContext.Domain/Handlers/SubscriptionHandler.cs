using System;
using System.Diagnostics.Contracts;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler :
    Notifiable,
    IHandler<CreateBoletoSubscriptionCommand>,
    IHandler<CreatePaypalSubscriptionCommand>

    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;
        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }
        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {   
            //Fail fast Validations
            command.Validate();
            if(command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possivel realizar seu cadastro");
            }

            //Verificar se documento esta cadastrado
            if(_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");
            
            //verificar se email ja esta cadastrado
            if(_repository.EmailExists(command.Email))
                AddNotification("Email", "Este CPF já está em uso");
            
            //gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            //Gerar as entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(null);
            var payment = new BoletoPayment(
                command.BarCode, 
                command.BoletoNumber, 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid, 
                command.Payer, 
                new Document(command.PayerDocument, command.PayerDocumentType), 
                address, 
                email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar Aplicar as validacoes
            AddNotifications(name, document, email, address, student, subscription, payment);
            
            //Checar as notificações
            if(Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            //Salvar as informações
            _repository.CreateSubscription(student);
            
            //Enviar E-mail de boas-vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem-vindo ao balt.io", "Sua assinatura foi criada");
           
            //Retornar Informações
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePaypalSubscriptionCommand command)
        {

            //Fail fast Validations
            command.Validate();
            if(command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possivel realizar seu cadastro");
            }
            
            //Verificar se documento esta cadastrado
            if(_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");
            
            //verificar se email ja esta cadastrado
            if(_repository.EmailExists(command.Email))
                AddNotification("Email", "Este CPF já está em uso");
            
            //gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            //Gerar as entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(null);
            var payment = new PaypalPayment(
                command.TransactionCode, 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid, 
                command.Payer, 
                new Document(command.PayerDocument, command.PayerDocumentType), 
                address, 
                email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar Aplicar as validacoes
            AddNotifications(name, document, email, address, student, subscription, payment);
            
            //Salvar as informações
            _repository.CreateSubscription(student);
            
            //Enviar E-mail de boas-vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem-vindo ao balt.io", "Sua assinatura foi criada");
           
            //Retornar Informações
            return new CommandResult(true, "Assinatura realizada com sucesso");

        }
    }
}
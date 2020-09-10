
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        // Red, Green, Refactor
        [TestMethod]
        public void ShoulReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Igor";
            command.LastName = "Dutra";
            command.Document = "01849887217";
            command.Email = "igordutra@gmail.com";     

            command.BarCode = "123456789";
            command.BoletoNumber = "123456789";
        
            command.PaymentNumber = "1231231";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "Wayne corp";
            command.PayerDocument = "1234567899";
            command.PayerDocumentType = EDocumentType.CPF;

            command.Street = "Rua das camélias";
            command.MyProperty = "asd";
            command.Number = "asd";
            command.PayerEmail = "Batman@dc.com";
            command.City = "as";
            command.Neighborhood = "as";
            command.State = "1232";
            command.Country = "123";
            command.ZipCode = "123";

            handler.Handle(command);
            Assert.AreEqual(false, handler.Valid);
        }
    }
}
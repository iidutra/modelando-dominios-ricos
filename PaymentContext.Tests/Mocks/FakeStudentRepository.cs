using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Repositories;

namespace PaymentContext.Tests.Mocks
{
    public class FakeStudentRepository : IStudentRepository
    {
        public void CreateSubscription(Student student)
        {
            
        }

        public bool DocumentExists(string document)
        {
            if(document == "01849887217")
                return true;

            return false;
        }

        public bool EmailExists(string email)
        {
            if(email == "igordutra@gmail.com")
                return true;

            return false;
        }
    }
}
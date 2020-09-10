
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Domain.Queries;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentsQueriesTests
    {
        // Red, Green, Refactor

        private IList<Student> _students;

        public StudentsQueriesTests()
        {
           for(var i = 0; i<=10; i++)
           {
               _students.Add(new Student(
                   new Name("Aluno", i.ToString()),
                   new Document("12345678914" + i.ToString(), EDocumentType.CPF),
                   new Email(i.ToString()+ "@balta.io")
                   ));
           }
        }

        [TestMethod]
        public void ShoulReturnStudentErrorWhenDocumentNotExists()
        {
            var exp = StudentQueries.GetStudentInfo("12345678910");
            var student = _students.AsQueryable().Where(exp).FirstOrDefault();

            Assert.AreEqual(null, student);
        }
        
        [TestMethod]
        public void ShoulReturnStudentErrorWhenDocumentExists()
        {
            var exp = StudentQueries.GetStudentInfo("12345678914");
            var student = _students.AsQueryable().Where(exp).FirstOrDefault();

            Assert.AreNotEqual(null, student);
        }
    }
}
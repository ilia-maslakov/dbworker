using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbworker.Data.EF;
using Moq;
using Microsoft.EntityFrameworkCore;
using dbworker.Connection;

namespace UnitTests
{
    [TestClass]
    public class TestUserRepository
    {
        private readonly DBworkerContext _context;
        [TestMethod]
        public void GetUsers()
        {

            var data = new List<User>
            {
                new User { Name = "BBB" },
                new User { Name = "ZZZ" },
                new User { Name = "AAA" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            var mockContext = new Mock<DBworkerContext>();
            mockContext.Setup(c => c.User).Returns(mockSet.Object);
            var service = new UserRepository(mockContext.Object);

            var users = service.GetUsers();
            Assert.AreEqual(3, users.Count);
            Assert.AreEqual("AAA", users[0].Name);
            Assert.AreEqual("BBB", users[1].Name);
            Assert.AreEqual("ZZZ", users[2].Name);
        }
    }
}
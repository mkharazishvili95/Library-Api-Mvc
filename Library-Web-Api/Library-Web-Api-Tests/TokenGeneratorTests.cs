using Library_Web_Api.Identity;
using Library_Web_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeServices
{
    [TestFixture]
    public class FakeTokenGeneratorTests
    {
        private IAccessTokenGenerator _fakeTokenGenerator;

        [SetUp]
        public void Setup()
        {
            _fakeTokenGenerator = new FakeTokenGenerator();
        }

        [Test]
        public void GenerateToken_ReturnsFakeToken()
        {
            var user = new User
            {
                Id = 1,
                FirstName = "Misho",
                LastName = "Kharazishvili",
                UserName = "Misho999@gmail.com",
                Role = "User"
            };
            var generatedToken = _fakeTokenGenerator.GenerateToken(user);
            Assert.IsNotNull(generatedToken);
        }
    }
}

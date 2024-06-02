using Library_Web_Api.Identity;
using Library_Web_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeServices
{
    public class FakeTokenGenerator : IAccessTokenGenerator
    {
        public string GenerateToken(User user)
        {
            string fakeToken = "Fake token =)";
            return fakeToken;
        }
    }
}

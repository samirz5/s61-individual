using Login_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login_Service.Service.Interfaces
{
    public interface IUserService
    {
        public void Register(User user);
        public string Authenticate(string userName, string password);

    }
}

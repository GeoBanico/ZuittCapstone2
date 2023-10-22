using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capstone.Users
{
    internal class Admin : User
    {

        public Admin() { }

        public Admin(int id, string username, string email, string password, string address, string contactNumber)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
            Address = address;
            ContactNumber = contactNumber;
            IsAdmin = true;
        }
    }
}

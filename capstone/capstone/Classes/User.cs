using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capstone.Users
{
    internal abstract class User
    {
        private int id;
        private string username;
        private string email;
        private string password;
        private string address;
        private string contactNumber;
        private bool isAdmin;

        protected User() { }

        protected User(int id, string username, string email, string password, string address, string contactNumber)
        {
            this.id = id;
            this.username = username;
            this.email = email;
            this.password = password;
            this.address = address;
            this.contactNumber = contactNumber;
        }

        public int Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Address { get => address; set => address = value; }
        public string ContactNumber { get => contactNumber; set => contactNumber = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        
        public override string ToString()
        {
            return $"{Id},{username},{email},{password},{address},{contactNumber},{isAdmin}";
        }
    }
}

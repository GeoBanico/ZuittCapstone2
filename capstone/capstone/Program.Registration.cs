using capstone.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace capstone
{
    internal partial class Program
    {
        readonly static string userDirectory = $"{Environment.CurrentDirectory}\\db\\user.txt";
        private static List<User> users = ReadNotepad_User();
        private static User? loggedUser = null;

        // - MAIN METHODS
        private static void Login() 
        {
            while (loggedUser == null) 
            {
                Console.Clear();
                Console.WriteLine("--- LOGIN ---\n");

                string username = StringInputValidation("username");
                string password = StringInputValidation("password");

                loggedUser = loginValidation(username, password);

                if (loggedUser == null) 
                {
                    Console.WriteLine("\nIncorrect Username or Password.");

                    if (ExitConfirmation("Exit"))
                        break;
                }
                    
            }
        }

        private static void Register()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTER ---");

            string username = UsernameInputValidation();
            while (Username_EmailValidation("username", username)) 
            {
                Console.WriteLine("Username already exists. Enter a new username.\n");
                username = StringInputValidation("Username");
            }

            string email  = EmailInputValidation();
            while (Username_EmailValidation("email", email))
            {
                Console.WriteLine("Email already exists. Enter a new email.\n");
                email = EmailInputValidation();
            }

            string password = StringInputValidation("Password");
            string confirmPassword = StringInputValidation("Confirm Password");
            while (!password.Equals(confirmPassword, StringComparison.Ordinal)) 
            {
                Console.WriteLine("Passwords do not match. Please try again.\n");
                password = StringInputValidation("Password");
                confirmPassword = StringInputValidation("Confirm Password");
            }

            string address = StringInputValidation("Address");

            string contactNumber = ContactNumberInputValidation();

            int id = GetUserId();

            Regular newUser = new(id, username, email, password, address, contactNumber);
            users.Add(newUser);

            //Adds to user db
            WriteToNotepad(userDirectory, "user");

            Console.WriteLine("\nAccount successfully added\n");

            GetUserDetails(newUser.Id);

            MessageEnder("\nLogin to access your account.\n\nReturning Back . . .");
        }

        private static bool ExitConfirmation(string message) 
        {
            Console.WriteLine($"\nDo you want to {message} [y]?");
            string? userInput = Console.ReadLine();

            if(string.Equals("y", userInput, StringComparison.OrdinalIgnoreCase) || string.Equals("yes", userInput, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
        // - END OF MAIN METHODS


        // HELPER METHODS
        //  - LOGIN
        // validates if the user exists
        private static User loginValidation(string username, string password) 
        {
            User? user = null;
            foreach (var searchUser in users)
            {
                if (searchUser.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && searchUser.Password.Equals(password, StringComparison.Ordinal))
                {
                    user = searchUser;
                    break;
                }
            }

            return user;
        }
        //  - END LOGIN

        //  - REGISTER
        // validates if username/email already exists
        private static bool Username_EmailValidation(string username_email, string userInput) 
        {
            foreach (var searchUser in users) 
            {
                if (username_email == "username")
                {
                    if (searchUser.Username.Equals(userInput, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                else 
                {
                    if (searchUser.Email.Equals(userInput, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        private static string EmailInputValidation()
        {
            string? userInput;
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            while (true)
            {
                userInput = StringInputValidation("Email");

                if (Regex.IsMatch(userInput, pattern))
                    break;
                else
                    Console.WriteLine("Kindly enter a valid email address. (e.g. sample@domain.com)\n");
            }

            return userInput;
        }

        private static string UsernameInputValidation() 
        {
            string? userInput;

            while (true) 
            {
                userInput = StringInputValidation("Username");

                if (!userInput.Contains(' '))
                    break;
                else
                    Console.WriteLine("Username should not contain spaces.\n");
            }

            return userInput;
        }

        private static string ContactNumberInputValidation() 
        { 
            string? userInput;
            string pattern = @"^(?:\+?63|0)?[0-9]{10}$";

            while (true) 
            {
                userInput = StringInputValidation("Contact Number");

                if (Regex.IsMatch(userInput, pattern))
                    break;
                else
                    Console.WriteLine("Kindly enter a valid phone number. (e.g. 09123456789)\n");
            }

            return userInput;
        }

        private static int GetUserId()
        {
            return users[users.Count - 1].Id + 1;
        }
        //  - END REGISTER
        // - END OF HELPER METHODS 


        // - OTHER METHODS
        // Retrieve a user using Id
        private static void GetUserDetails(int userId)
        {
            int userIndex = 0;
            foreach (var user in users) 
            {
                if (user.Id == userId)
                    break;
                userIndex++;
            }

            Console.WriteLine("- User Information -");
            Console.WriteLine($"Username: {users[userIndex].Username}");
            Console.WriteLine($"Email: {users[userIndex].Email}");
            Console.WriteLine($"Password: {users[userIndex].Password}");
            Console.WriteLine($"Address: {users[userIndex].Address}");
            Console.WriteLine($"Contact Number: {users[userIndex].ContactNumber}");

            if (users[userIndex].IsAdmin)
                Console.WriteLine($"Access Type: Admin");
            else
                Console.WriteLine($"Access Type: Regular");
        }
        // - END OTHER METHODS


        // - USER DB METHODS
        // reads all user and fillup the local User List
        private static List<User> ReadNotepad_User() 
        {
            List<User> users = new();
            try
            {
                StreamReader sr = new(userDirectory);

                while (true)
                {
                    string? line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line)) break;
                    string[] userVal = line.Split(',');

                    if (userVal[userVal.Length - 1].Equals("true", StringComparison.OrdinalIgnoreCase))
                        users.Add(new Admin(Convert.ToInt32(userVal[0]), userVal[1], userVal[2], userVal[3], userVal[4], userVal[5]));
                    else
                        users.Add(new Regular(Convert.ToInt32(userVal[0]), userVal[1], userVal[2], userVal[3], userVal[4], userVal[5]));
                }
                
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return users;
        }
        // - END USER DB METHODS


        // - GLOBAL HELPER METHODS
        // asks and checks the user's input 
        private static string StringInputValidation(string label)
        {
            string? userInput = "";

            while (true)
            {
                Console.Write($"Enter {label}: ");
                userInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(userInput))
                    break;
                else
                    Console.WriteLine("WARNING! Empty field.\n");
            }

            return userInput;
        }

        // To end the message
        private static void MessageEnder(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadLine();
            Console.Clear();
        }
        // - END OF GLOBAL HELPER METHODS
    }
}

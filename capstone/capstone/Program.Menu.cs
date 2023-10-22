using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using capstone.Users;

namespace capstone
{
    internal partial class Program
    {

        readonly static string itemDirectory = $"{Environment.CurrentDirectory}\\db\\item.txt";
        private static List<Item> items = ReadNotepad_Item();

        // - MAIN METHODS
        // (both) Retreive all Items
        private static void GetAllItems(bool isAdmin) 
        {
            Console.WriteLine("- All Available Items -");

            int itemCount = 0;
            foreach (Item item in items)
            {
                if (isAdmin)
                {
                    Console.WriteLine($"{itemCount}. {item.Name} - {item.TotalBalance}Pc/s");
                    itemCount++;
                }
                else 
                {
                    if (item.TotalBalance != 0) 
                    {
                        Console.WriteLine($"{itemCount}. {item.Name} - {item.TotalBalance}Pc/s");
                        itemCount++;
                    }
                }
            }

            if(itemCount == 0)
                Console.WriteLine("No available Items");
        }

        // (both) Edit Profile
        private static void EditLoggedUserDetails(int userId) 
        {
            Console.Clear();
            Console.WriteLine("--- EDIT USER PROFILE ---");

            GetUserDetails(loggedUser.Id);

            int id = loggedUser.Id;

            string username = StringInputToEditValidation("New Username", loggedUser.Username);
            while (true) 
            {
                if (username.Equals(loggedUser.Username, StringComparison.OrdinalIgnoreCase))
                    break;
                else if (!Username_EmailValidation("username", username))
                    break;
                else 
                {
                    Console.WriteLine("Username already exists. Enter a new username.\n");
                    username = StringInputToEditValidation("New Username", loggedUser.Username);
                }
            }

            string email = EmailInputToEditValidation(loggedUser.Email);
            while (true)
            {
                if (email.Equals(loggedUser.Email, StringComparison.OrdinalIgnoreCase))
                    break;
                else if (!Username_EmailValidation("email", email))
                    break;
                else
                {
                    Console.WriteLine("Email already exists. Enter a new email.\n");
                    email = EmailInputToEditValidation(loggedUser.Email);
                }
            }

            string password = StringInputToEditValidation("New Password", loggedUser.Password);
            while (true)
            {
                if (password.Equals(loggedUser.Password, StringComparison.Ordinal))
                    break;
                else
                {
                    string confirmPassword = StringInputValidation("Confirm Password");
                    if (!password.Equals(confirmPassword, StringComparison.Ordinal))
                    {
                        Console.WriteLine("Passwords do not match. Please try again.\n");
                        password = StringInputToEditValidation("New Password", loggedUser.Password);
                    }
                    else
                        break;
                }
            }

            string address = StringInputToEditValidation("New Address", loggedUser.Address);

            string contactNumber = ContactNumberInputToEditValidation(loggedUser.ContactNumber);

            User? editedUser;
            if (loggedUser.IsAdmin)
                editedUser = new Admin(id, username, email, password, address, contactNumber);
            else
                editedUser = new Regular(id, username, email, password, address, contactNumber);

            users[users.IndexOf(loggedUser)] = editedUser;

            loggedUser = editedUser;

            //Adds to user db
            WriteToNotepad(userDirectory, "user");

            Console.WriteLine("\nUser successfully Updated\n");

            GetUserDetails(id);

            MessageEnder("\nReturning Back . . .");
        }

        // (admin) Create Item
        private static void CreateItem() 
        {
            Console.Clear();
            Console.WriteLine("--- CREATE ITEM ---");

            string name = StringInputValidation("Item Name");

            string branch = StringInputValidation("Item Branch");

            int beginningInventory = IntegerInputValidation("Item Beginning Inventory");

            int stockIn = IntegerInputValidation("Item Stock In");

            int stockOut = IntegerInputValidation("Item Stock Out");

            Item newItem = new(items[items.Count-1].Id+1, name, branch, beginningInventory, stockIn, stockOut);
            items.Add(newItem);

            WriteToNotepad(itemDirectory, "item");

            Console.WriteLine("\nItem Successfully Created\n");

            GetItemDetails(newItem.Id);

            MessageEnder("\nReturning Back . . .");
        }

        // (admin) Edit Item
        private static void EditItem() 
        {
            Console.Clear();
            Console.WriteLine("--- EDIT ITEM ---");

            GetAllItems(loggedUser.IsAdmin);

            int itemToEdit = IntegerInputValidation("Item to Edit");
            while (!(itemToEdit <= 0 || itemToEdit >= items.Count))
                itemToEdit = IntegerInputValidation("Item to Edit");

            GetItemDetails(items[itemToEdit].Id);

            int id = items[itemToEdit - 1].Id;

            string name = StringInputToEditValidation("New Item Name", items[itemToEdit - 1].Name);

            string branch = StringInputToEditValidation("New Item Branch", items[itemToEdit - 1].Branch);

            int beginningInventory = IntegerInputToEditValidation("New Item Beginning Inventory", items[itemToEdit - 1].BeginningInventory.ToString());

            int stockIn = IntegerInputToEditValidation("New Item Stock In", items[itemToEdit - 1].StockIn.ToString());

            int stockOut = IntegerInputToEditValidation("New Item Stock Out", items[itemToEdit - 1].StockOut.ToString());

            items[itemToEdit] = new Item(id, name, branch, beginningInventory, stockIn, stockOut);

            //Adds to item db
            WriteToNotepad(itemDirectory, "item");

            Console.WriteLine("\nItem Successfully Updated\n");

            GetItemDetails(items[itemToEdit].Id);

            MessageEnder("\nReturning Back . . .");
        }

        // (admin) Delete Item
        private static void DeleteItem()
        {
            Console.Clear();
            Console.WriteLine("--- DELETE ITEM ---");

            GetAllItems(loggedUser.IsAdmin);

            int itemToDelete = IntegerInputValidation("Item to Delete");
            while (!(itemToDelete <= 0 || itemToDelete >= items.Count))
                itemToDelete = IntegerInputValidation("Item to Delete");

            items.RemoveAt(itemToDelete-1);

            //Adds to user db
            WriteToNotepad(itemDirectory, "item");

            Console.WriteLine("\nItem Successfully Deleted\n");

            MessageEnder("\nReturning Back . . .");
        }

        // (admin) Retrieve all user details
        private static void GetAllUsersDetails()
        {
            Console.WriteLine("- All current users -");

            int userCount = 1;
            foreach (User user in users)
            {
                if (user.IsAdmin)
                    Console.WriteLine($"{userCount}. {user.Username} - Admin");
                else
                    Console.WriteLine($"{userCount}. {user.Username} - Regular");

                userCount++;
            }
        }

        // (regular) Retreive item details by list index
        private static void GetSelectedItemDetails() 
        {
            while (true) 
            {
                Console.Clear();
                GetAllItems(loggedUser.IsAdmin);

                int itemsListIndex = IntegerInputValidation("Item to View");

                GetItemDetails(itemsListIndex - 1);

                if (!ExitConfirmation("Choose another Item"))
                    break;
            }
        }
        // - END MAIN METHODS


        // - HELPER METHODS
        // Print current logged acount name
        private static void PrintAccountName() 
        {
            string accountType = loggedUser.IsAdmin ? "Admin" : "Regular";

            Console.WriteLine($"Account: {loggedUser.Username} ({accountType})");
        }

        // Get Item Details using Id
        private static void GetItemDetails(int itemId)
        {
            Console.WriteLine("- Item Details -");
            int itemIndex = 0;
            foreach (var item in items)
            {
                if (item.Id == itemId)
                    break;
                itemIndex++;
            }

            Console.WriteLine("- Item Information -");
            Console.WriteLine($"Name: {items[itemIndex].Name}");
            Console.WriteLine($"Branch: {items[itemIndex].Branch}");
            Console.WriteLine($"Beginning Inventory: {items[itemIndex].BeginningInventory}");
            Console.WriteLine($"Stock In: {items[itemIndex].StockIn}");
            Console.WriteLine($"Stock Out: {items[itemIndex].StockOut}");
            Console.WriteLine($"Total Balance: {items[itemIndex].TotalBalance}");
        }

        // Validates if user input is an integer
        private static int IntegerInputValidation(string label) 
        {
            string? userInput = StringInputValidation(label);
            while (!int.TryParse(userInput, out _))
                userInput = StringInputValidation(label);

            return Convert.ToInt32(userInput);
        }

        // For editing string input validations
        private static string StringInputToEditValidation(string label, string prevInput)
        {
            Console.Write($"If you want to retain the previous input [press 'Enter']\nEnter {label}: ");
            string? userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
                return prevInput;

            return userInput;
        }

        // For editing integer input validations
        private static int IntegerInputToEditValidation(string label, string prevInput) 
        {
            string? userInput = StringInputToEditValidation(label, prevInput);
            while (!int.TryParse(userInput, out _))
                userInput = StringInputToEditValidation(label, prevInput);

            return Convert.ToInt32(userInput);
        }

        // For editing emails input validations
        private static string EmailInputToEditValidation(string prevInput)
        {
            string? userInput;
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            while (true)
            {
                userInput = StringInputToEditValidation("New Email", prevInput);

                if (Regex.IsMatch(userInput, pattern))
                    break;
                else
                    Console.WriteLine("Kindly enter a valid email address. (e.g. sample@domain.com)\n");
            }

            return userInput;
        }

        //For editing contact number input validations
        private static string ContactNumberInputToEditValidation(string prevInput)
        {
            string? userInput;
            string pattern = @"^(?:\+?63|0)?[0-9]{10}$";

            while (true)
            {
                userInput = StringInputToEditValidation("New Contact Number", prevInput);

                if (Regex.IsMatch(userInput, pattern))
                    break;
                else
                    Console.WriteLine("Kindly enter a valid phone number. (e.g. 09123456789)\n");
            }

            return userInput;
        }
        // - END HELPER METHODS


        // - ITEM DB METHODS
        // reads all items and fillup the local Item List
        private static List<Item> ReadNotepad_Item()
        {
            List<Item> items = new();
            try
            {
                StreamReader sr = new(itemDirectory);

                while (true)
                {
                    string? line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line)) 
                        break;

                    string[] itemVal = line.Split(',');
                    items.Add(new Item(Convert.ToInt32(itemVal[0]), itemVal[1], itemVal[2], Convert.ToInt32(itemVal[3]), Convert.ToInt32(itemVal[4]), Convert.ToInt32(itemVal[5])));
                }

                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return items;
        }

        // Overwrite the contents of the notepad base on the directory
        private static void WriteToNotepad(string directory, string writeLocation)
        {
            try
            {
                StreamWriter sw = new(directory);

                if (writeLocation.Equals("user", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (User user in users)
                    {
                        sw.WriteLine(user.ToString());
                    }
                }
                else if (writeLocation.Equals("item", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (Item item in items)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
                else
                    throw (new CustomException($"Wrong Location name: {writeLocation}\n Options: user, item, user_item"));


                sw.Close();
            }
            catch (CustomException e) 
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        // - END ITEM DB METHODS
    }
}

using capstone.Users;
using System.Reflection;

namespace capstone
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            while (true) 
            {
                Registration();

                Menu();
            }
            
        }

        // Registration
        private static void Registration()
        {
            while (loggedUser == null)
            {
                Console.Clear();
                Console.WriteLine("- ZUITT ASSET MANAGEMENT APP -");
                Console.WriteLine("[1] Login\n[2] Register\n[3] Exit\n");
                Console.Write("Enter your option: ");
                string? userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    MessageEnder("\nWARNING! Input is empty.");
                    continue;
                }

                switch (userInput)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        Register();
                        break;
                    case "3":
                        if (ExitConfirmation("Exit"))
                            Environment.Exit(0);
                        break;
                    default:
                        MessageEnder("\nWarning! Kindly choose from the options.");
                        break;
                }
            }

            MessageEnder($"\nWelcome {loggedUser.Username}!\nRedirecting to Zuitt Asset Menu . . .");
        }

        // Main Menu
        private static void Menu()
        {
            bool loggedOut = false;
            while (!loggedOut)
            {
                Console.Clear();
                Console.WriteLine("- ZUITT ASSET MANAGEMENT APP -");
                PrintAccountName();
                if (loggedUser.IsAdmin)
                {
                    Console.WriteLine("\n[1] Retrieve All Items\n[2] Create Item\n[3] Edit Item\n[4] Delete Item\n[5] Retrieve All Users\n[6] View Profile\n[7] Edit Profile\n[8]  Log Out\n[9] Exit");
                    Console.Write("Enter your option: ");
                    string? userInput = Console.ReadLine();

                    switch (userInput)
                    {
                        case "1":
                            Console.Clear();
                            GetAllItems(loggedUser.IsAdmin);
                            MessageEnder("Returning home . . .");
                            break;
                        case "2":
                            CreateItem();
                            break;
                        case "3":
                            EditItem();
                            break;
                        case "4":
                            DeleteItem();
                            break;
                        case "5":
                            Console.Clear();
                            GetAllUsersDetails();
                            MessageEnder("Returning home . . .");
                            break;
                        case "6":
                            Console.Clear();
                            GetUserDetails(loggedUser.Id);
                            MessageEnder("Returning home . . .");
                            break;
                        case "7":
                            EditLoggedUserDetails(loggedUser.Id);
                            break;
                        case "8":
                            if (ExitConfirmation("Log Out"))
                            {
                                loggedOut = true;
                                loggedUser = null;
                            }
                            break;
                        case "9":
                            if (ExitConfirmation("Exit"))
                                Environment.Exit(0);
                            break;
                        default:
                            MessageEnder("\nWarning! Kindly choose from the options.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\n[1] Retrieve All Items\n[2] View Selected Item\n[3] View Profile\n[4] Edit Profile\n[5]  Log Out\n[6] Exit");
                    Console.Write("Enter your option: ");
                    string? userInput = Console.ReadLine();

                    switch (userInput)
                    {
                        case "1":
                            Console.Clear();
                            GetAllItems(loggedUser.IsAdmin);
                            MessageEnder("Returning to home . . .");
                            break;
                        case "2":
                            GetSelectedItemDetails();
                            break;
                        case "3":
                            Console.Clear();
                            GetUserDetails(loggedUser.Id);
                            MessageEnder("Returning home . . .");
                            break;
                        case "4":
                            EditLoggedUserDetails(loggedUser.Id);
                            break;
                        case "5":
                            if (ExitConfirmation("Log Out"))
                            {
                                loggedOut = true;
                                loggedUser = null;
                            }
                            break;
                        case "6":
                            if (ExitConfirmation("Exit"))
                                Environment.Exit(0);
                            break;
                        default:
                            MessageEnder("\nWarning! Kindly choose from the options.");
                            break;
                    }
                }
            }
        }
    }
}
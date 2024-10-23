using System;
using LoginForm;
namespace UserManagementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var userService = new UserService();
            bool isLoggedIn = false;
            string loggedInUser = null;

            while (true)
            {
                Console.Write(">");
                var input = Console.ReadLine();
                var tokens = input.Split("--");

                var command = tokens[0].Trim();
                var parameters = new Dictionary<string, string>();

                for (int i = 1; i < tokens.Length; i++)
                {
                    var keyValue = tokens[i].Trim().Split(' ');
                    parameters[keyValue[0]] = keyValue[1];
                }

                switch (command)
                {
                    case "Register":
                        var username = parameters["username"];
                        var password = parameters["password"];
                        userService.Register(username, password);
                        break;

                    case "Login":
                        username = parameters["username"];
                        password = parameters["password"];
                        if (userService.Login(username, password))
                        {
                            isLoggedIn = true;
                            loggedInUser = username;
                        }
                        break;

                    case "Change":
                        if (!isLoggedIn)
                        {
                            Console.WriteLine("Please login first!");
                            break;
                        }
                        if (parameters.ContainsKey("status"))
                        {
                            var status = parameters["status"];
                            userService.ChangeStatus(loggedInUser, status);
                        }
                        break;

                    case "Search":
                        if (!isLoggedIn)
                        {
                            Console.WriteLine("Please login first!");
                            break;
                        }
                        var searchUsername = parameters["username"];
                        userService.SearchUsers(searchUsername);
                        break;

                    case "ChangePassword":
                        if (!isLoggedIn)
                        {
                            Console.WriteLine("Please login first!");
                            break;
                        }
                        var oldPassword = parameters["old"];
                        var newPassword = parameters["new"];
                        userService.ChangePassword(loggedInUser, oldPassword, newPassword);
                        break;

                    case "Logout":
                        if (!isLoggedIn)
                        {
                            Console.WriteLine("Please login first!");
                            break;
                        }
                        isLoggedIn = false;
                        loggedInUser = "";
                        Console.WriteLine("Successfully logged out.");
                        break;

                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }
    }
}

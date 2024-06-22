using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace cmdApp
{
    public class CommandHandler
    {
        public static int numBlanks = 0;
        public static bool isRoot = false;

        public static bool HandleCommand(string command)
        {
            InputHandler.CommandType commandType = InputHandler.HandleInput(command);
            command = command.Trim();
            string[] args = command.Split(' ');

            bool commandInvalid = false;

            // I kinda blacked out writing this '<>' checker
            string longArgStart = "";
            string longArgEnd = "";

            foreach (string arg in args)
            {
                if (arg.Contains('<'))
                {
                    longArgStart = arg;
                }
                
                if (arg.Contains('>'))
                {
                    longArgEnd = arg;
                }
            }

            if (longArgStart != "" && longArgEnd == "" ||
                longArgStart == "" && longArgEnd != "")
            {
                commandInvalid = true;
            }

            if (!string.IsNullOrEmpty(longArgStart) && !string.IsNullOrEmpty(longArgEnd))
            {
                int indexOfStart = Array.IndexOf(args, longArgStart);
                int indexOfEnd = Array.IndexOf(args, longArgEnd);

                string finalArg = "";

                for (int i = indexOfStart; i <= indexOfEnd; i++)
                {
                    finalArg += $"{args[i]} ";
                }

                finalArg = finalArg.Trim();
                finalArg = finalArg.Replace("<", "").Replace(">", "");

                List<string> newArgsList = new List<string>();

                for (int i = 0; i < args.Length; i++)
                {
                    if (i == indexOfStart)
                    {
                        newArgsList.Add(finalArg);
                        i = indexOfEnd;
                    }
                    else
                        newArgsList.Add(args[i]);
                }

                args = newArgsList.ToArray();
            }

            // This is art
            switch (commandType)
            {
                case InputHandler.CommandType.ClearCommand:
                    Console.Clear();
                    break;

                case InputHandler.CommandType.InvalidCommand:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unknown Command (use 'help' command)\n");
                    Console.ResetColor();
                    commandInvalid = true;
                    break;

                case InputHandler.CommandType.EmptyCommand:
                    numBlanks += 1;
                    break;

                case InputHandler.CommandType.ListCommand:
                    ListAvailableCommands();
                    break;

                case InputHandler.CommandType.DescriptionCommand:
                    DescribeCommand(args);
                    break;

                case InputHandler.CommandType.LoopCommand:
                    Loop(args);
                    break;

                case InputHandler.CommandType.GenerateRandomCommand:
                    GenerateRandom(args);
                    break;

                case InputHandler.CommandType.PrintCommand:
                    Print(args);
                    break;

                case InputHandler.CommandType.AddCommand:
                    Add(args);
                    break;

                case InputHandler.CommandType.SubCommand:
                    Subtract(args);
                    break;

                case InputHandler.CommandType.MultCommand:
                    Multiply(args);
                    break;

                case InputHandler.CommandType.DivCommand:
                    Divide(args);
                    break;

                case InputHandler.CommandType.CountCommand:
                    Count(args);
                    break;

                case InputHandler.CommandType.CoinFlipCommand:
                    CoinFlip(args);
                    break;

                case InputHandler.CommandType.RockPaperScissorsCommand:
                    RockPaperScissors(args);
                    break;

                case InputHandler.CommandType.RandomChoiceCommand:
                    RandomChoice(args);
                    break;

                case InputHandler.CommandType.FibonacciCommand:
                    Fibonacci(args);
                    break;

                case InputHandler.CommandType.TitleCommand:
                    SetTitle(args);
                    break;

                case InputHandler.CommandType.RandomStringCommand:
                    RandomString(args);
                    break;


                case InputHandler.CommandType.RootCommand:
                    Root(args);
                    break;

                case InputHandler.CommandType.CreditsCommand:
                    PrintCredits();
                    break;
            }

            return commandInvalid;
        }

        private static void ListAvailableCommands()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Available commands:");
            foreach (string command in InputHandler.validCommands.Keys)
            {
                Console.WriteLine($"- {command}");
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void DescribeCommand(string[] args)
        {
            if (!InputHandler.validCommands.ContainsKey(args[1].ToLower()))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unknown Command: {args[1]}.\n");
                Console.ResetColor();
                return;
            }

            InputHandler.CommandType commandType = InputHandler.validCommands[args[1].ToLower()];
            string commandDescription = InputHandler.commandDescriptions[commandType];

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"- {args[1].ToLower()}: {commandDescription}.");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void GenerateRandom(string[] args)
        {
            if (args.Length == 4 && int.TryParse(args[1], out int min) && int.TryParse(args[2], out int max) && (args[3].ToLower() == "true" || args[3] == "false"))
            {
                Random rand = new Random();
                int randomNumber = rand.Next(min, max + 1);
                Console.ForegroundColor = ConsoleColor.Yellow;

                // This is pretty gross, might remove later
                if (args[3].ToLower() == "false")
                    Console.WriteLine($"Random number between {min} and {max}: {randomNumber}\n");
                else if (args[3].ToLower() == "true")
                    Console.WriteLine($"{randomNumber}\n");

                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(int) min> <(int) max> <(bool) print num only>\n");
                Console.ResetColor();
            }
        }

        // Really basic but, I love it too much to get rid of
        private static void Print(string[] args)
        {
            if (args.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(string) msg, ...>\n");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 1; i < args.Length; i++)
            {
                Console.Write($"{args[i]} ");
            }
            Console.WriteLine("\n");
            Console.ResetColor();
        }

        private static void Add(string[] args)
        {
            if (args.Length < 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(int / float) number> <(int / float) number, ...>\n");
                Console.ResetColor();
                return;
            }

            float sum = 0;

            // Summations made easy
            if (args[2] == "..")
            {
                if (float.TryParse(args[1], out float min) && float.TryParse(args[3], out float max))
                {
                    for (float i = min; i <= max; i++)
                    {
                        sum += i;
                    }
                }
            }
            else
            {
                for (int i = 1; i < args.Length; i++)
                {
                    if (float.TryParse(args[i], out float num))
                        sum += num;
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error parsing '{args[i]}' as a number. Skipping...");
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{sum}\n");
            Console.ResetColor();
        }

        private static void Subtract(string[] args)
        {
            if (args.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(int / float) number> <(int / float) number>\n");
                Console.ResetColor();
                return;
            }

            // I handled this terribly but hey, I wrote this a while ago
            if (!float.TryParse(args[1], out float num1))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error parsing '{args[1]}' as a number.");
                Console.ResetColor();
                return;
            }

            if (!float.TryParse(args[2], out float num2))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error parsing '{args[2]}' as a number.");
                Console.ResetColor();
                return;
            }

            float difference = num1 - num2;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{difference}\n");
            Console.ResetColor();
        }

        private static void Multiply(string[] args)
        {
            if (args.Length < 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(int / float) number> <(int / float) number, ...>\n");
                Console.ResetColor();
                return;
            }

            float product = 1;

            // Multiplication summation
            if (args[2] == "..")
            {
                if (float.TryParse(args[1], out float min) && float.TryParse(args[3], out float max))
                {
                    for (float i = min; i <= max; i++)
                    {
                        product *= i;
                    }
                }
            }
            else
            {
                for (int i = 1; i < args.Length; i++)
                {
                    if (float.TryParse(args[i], out float num))
                        product *= num;
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error parsing '{args[i]}' as a number. Skipping...");
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{product}\n");
            Console.ResetColor();
        }

        private static void Divide(string[] args)
        {
            if (args.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(int / float) number> <(int / float) number>\n");
                Console.ResetColor();
                return;
            }

            // I handled this terribly but hey, I wrote this a while ago
            if (!float.TryParse(args[1], out float num1))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error parsing '{args[1]}' as a number.");
                Console.ResetColor();
                return;
            }

            if (!float.TryParse(args[2], out float num2))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error parsing '{args[2]}' as a number.");
                Console.ResetColor();
                return;
            }

            float quotient = num1 / num2;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{quotient}\n");
            Console.ResetColor();
        }

        // I really think I coulda did this better
        private static void Count(string[] args)
        {
            if (args.Length != 4 && args.Length != 5)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(int / float) min> <(int / float) max> <(int / float) increment> [<(int / float) delay>]\n");
                Console.ResetColor();
                return;
            }

            if (args.Length == 4)
            {
                if (float.TryParse(args[1], out float min) && float.TryParse(args[2], out float max) && float.TryParse(args[3], out float increment))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    if (increment > 0)
                    {
                        for (float i = min; i <= max; i += increment)
                        {
                            Console.WriteLine(i);
                        }
                    }
                    else if (increment < 0)
                    {
                        for (float i = min; i >= max; i += increment)
                        {
                            Console.WriteLine(i);
                        }
                    }
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid command format. Use: {args[0]} <(int / float) min> <(int / float) max> <(int / float) increment> [<(int / float) delay>]\n");
                    Console.ResetColor();
                    return;
                }
            }
            else
            {
                if (float.TryParse(args[1], out float min) && float.TryParse(args[2], out float max) && float.TryParse(args[3], out float increment) && float.TryParse(args[4], out float delay))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    int intDelay = (int)(delay * 1000);

                    if (increment > 0)
                    {
                        for (float i = min; i <= max; i += increment)
                        {
                            Console.WriteLine(i);
                            Thread.Sleep(intDelay);
                        }
                    }
                    else if (increment < 0)
                    {
                        for (float i = min; i >= max; i += increment)
                        {
                            Console.WriteLine(i);
                            Thread.Sleep(intDelay);
                        }
                    }
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid command format. Use: {args[0]} <(int / float) min> <(int / float) max> <(int / float) increment> [<(int / float) delay>]\n");
                    Console.ResetColor();
                    return;
                }
            }
            Console.WriteLine();
        }

        private static void CoinFlip(string[] args)
        {
            if (args.Length > 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(int) amount>\n");
                Console.ResetColor();
                return;
            }

            Random rand = new Random();

            Console.ForegroundColor = ConsoleColor.Yellow;

            if (args.Length == 2)
            {
                if (int.TryParse(args[1], out int max))
                {
                    for (int i = 0; i < max; i++)
                    {
                        int num = rand.Next(0, 2);
                        Console.WriteLine(num > 0 ? "Heads" : "Tails");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid command format. Use: {args[0]} <(int) amount>\n");
                    Console.ResetColor();
                    return;
                }
            }
            else
            {
                int num = rand.Next(0, 2);
                Console.WriteLine(num > 0 ? "Heads" : "Tails");
            }

            Console.ResetColor();
            Console.WriteLine();
        }

        private static void RockPaperScissors(string[] args)
        {
            if (args.Length > 3 || args.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(string) choice> <(bool) countdown>\n");
                Console.ResetColor();
                return;
            }

            string[] choices = { "rock", "paper", "scissors" };

            string userChoice = Array.Find(choices, choice => choice.Equals(args[1], StringComparison.OrdinalIgnoreCase));
            if (userChoice == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid choice. Please select either 'rock', 'paper', or 'scissors'.\n");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;

            if (args.Length == 3)
            {
                bool countdown;

                if (!bool.TryParse(args[2], out countdown))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid show countdown value. Please use 'true' or 'false'.\n");
                    Console.ResetColor();
                    return;
                }

                if (countdown)
                {
                    for (int i = 3; i > 0; i--)
                    {
                        Console.WriteLine(i);
                        Thread.Sleep(1000);
                    }
                    Console.WriteLine("Go!\n");
                }
            }

            Random rand = new Random();
            string computerChoice = choices[rand.Next(choices.Length)];

            Console.WriteLine($"You chose: {userChoice}");
            Console.WriteLine($"Computer chose: {computerChoice}");

            if (userChoice == computerChoice)
            {
                Console.WriteLine("It's a tie!");
            }
            else if ((userChoice == "rock" && computerChoice == "scissors") ||
                     (userChoice == "scissors" && computerChoice == "paper") ||
                     (userChoice == "paper" && computerChoice == "rock"))
            {
                Console.WriteLine("You win!");
            }
            else
            {
                Console.WriteLine("You lose!");
            }

            Console.ResetColor();

            Console.WriteLine();
        }

        private static void RandomChoice(string[] args)
        {
            if (args.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(string) choices, ...>\n");
                Console.ResetColor();
                return;
            }

            Random rand = new Random();
            string[] newArgs = new string[args.Length];

            for (int i = 1; i < args.Length; i++)
            {
                if (!string.IsNullOrEmpty(args[i]))
                    newArgs[i - 1] = args[i];
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error parsing '{args[i]}'. Skipping...");
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{newArgs[rand.Next(newArgs.Length)]}\n");
            Console.ResetColor();
        }

        // The bane of my existence
        private static void Loop(string[] args)
        {
            if (args.Length > 4 || args.Length < 3 || int.TryParse(args[1], out int _) || !int.TryParse(args[2], out int loopAmount))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(cmd list) <command (args, ...)>> <(int) loop amount> [<(int) delay>]\n");
                Console.ResetColor();
                return;
            }

            int intDelay = 0;

            if (args.Length == 4)
            {
                if (float.TryParse(args[3], out float delay))
                    intDelay = (int) (delay * 1000);
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid command format. Use: {args[0]} <(cmd list) <command (args, ...)>> <(int) loop amount> [<(int) delay>]\n");
                    Console.ResetColor();
                    return;
                }
            }

            for (int i = 0; i < loopAmount; i++)
            {
                // If the command between '< >' has invalid args
                // it prints the error several times

                // Im not fixing that anytime soon
                if (HandleCommand(args[1]))
                    break;

                Thread.Sleep(intDelay);
            }
        }

        private static void Fibonacci(string[] args)
        {
            if (args.Length != 2 || !int.TryParse(args[1], out int n))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(+int) length>\n");
                Console.ResetColor();
                return;
            }

            if (n < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error parsing '{args[1]}': Length must be positive.");
                Console.ResetColor();
                return;
            }

            List<int> nums = new List<int> { 0, 1 };

            for (int i = 1; i < n - 1; i++)
            {
                nums.Add(nums[i - 1] + nums[i]);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;

            foreach (int num in nums)
                Console.Write($"{num} ");

            Console.WriteLine();
            Console.ResetColor();
        }

        private static void SetTitle(string[] args)
        {
            if (args.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(string) title>\n");
                Console.ResetColor();
                return;
            }

            if (string.IsNullOrWhiteSpace(args[1]))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error parsing '{args[1]}': Title cannot be empty.");
                Console.ResetColor();
                return;
            }

            Console.Title = string.Join(" ", args.Skip(1));
            Console.WriteLine();
        }

        // PASSWORDD GENERATOR!!1
        private static void RandomString(string[] args)
        {
            if (args.Length < 3 || args.Length > 5 || !int.TryParse(args[2], out int stringLength))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(string) charList> <(int) stringLength> [<(int) amount>] [<(int) caps percent>]\n");
                Console.ResetColor();
                return;
            }

            Random rand = new Random();

            List<string> randomString = new List<string>();
            char[] charList = args[1].ToLower() == "charlist" ? "qwertyuiopasdfghjklzxcvbnm1234567890!@#$%*".ToCharArray() : args[1].ToCharArray();
            
            int amount = 1;
            int capsPercent = 0;

            if (args.Length >= 4)
            {
                if (!int.TryParse(args[3], out amount))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid command format. Use: {args[0]} <(string) charList> <(int) stringLength> [<(int) amount>] [<(int) caps percent>]\n");
                    Console.ResetColor();
                    return;
                }

                if (amount < 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error parsing '{amount}': Amount must be greater than 0.");
                    Console.ResetColor();
                    return;
                }
            }

            if (args.Length == 5)
            {
                if (!int.TryParse(args[4], out capsPercent))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid command format. Use: {args[0]} <(string) charList> <(int) stringLength> [<(int) amount>] [<(int) caps percent>]\n");
                    Console.ResetColor();
                    return;
                }
                
                if (capsPercent < 0 || capsPercent > 100)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error parsing '{capsPercent}': Percent must be an int between 0 and 100.");
                    Console.ResetColor();
                    return;
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;

            for (int i = 0; i < amount; i++)
            {
                for (int j = 0; j < stringLength; j++)
                {
                    string nextChar = capsPercent > rand.Next(101) ? charList[rand.Next(charList.Length)].ToString().ToUpper(): charList[rand.Next(charList.Length)].ToString();
                    randomString.Add(nextChar);
                }
                    
                Console.WriteLine($"{string.Join("", randomString)}\n");
                randomString.Clear();
            }

            Console.ResetColor();
        }

        // idk what i was thinking, prob gonna remove this
        private static void Root(string[] args)
        {
            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command format. Use: {args[0]} <(string) password>\n");
                Console.ResetColor();
                return;
            }

            // We love security
            if (args[1] != "Password")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid password.\n");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Access Granted!\n");
            Console.ResetColor();

            isRoot = true;
        }

        private static void PrintCredits()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"███████╗██╗     ███████╗███╗   ███╗██████╗ ███████╗███████╗\r\n██╔════╝██║     ██╔════╝████╗ ████║██╔══██╗██╔════╝██╔════╝\r\n█████╗  ██║     █████╗  ██╔████╔██║██║  ██║█████╗  █████╗  \r\n██╔══╝  ██║     ██╔══╝  ██║╚██╔╝██║██║  ██║██╔══╝  ██╔══╝  \r\n███████╗███████╗███████╗██║ ╚═╝ ██║██████╔╝███████╗███████╗\r\n╚══════╝╚══════╝╚══════╝╚═╝     ╚═╝╚═════╝ ╚══════╝╚══════╝\r\n                                                           \n");
            Console.ResetColor();
        }
    }
}
using System;
using System.Linq;

namespace cmdApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ElEmDee's Command Prompt";

            InputHandler.CommandType[] ignoreCommands = {
                InputHandler.CommandType.InvalidCommand,
                InputHandler.CommandType.EmptyCommand,
                InputHandler.CommandType.CreditsCommand,
                InputHandler.CommandType.RootCommand
            };

            foreach (InputHandler.CommandType commandType in Enum.GetValues(typeof(InputHandler.CommandType)))
            {
                if (!ignoreCommands.Contains(commandType) && !InputHandler.commandDescriptions.ContainsKey(commandType))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Warning: No description found for command type '{commandType}'");
                    Console.ResetColor();
                }
            }

            while (true)
            {
                // My favorite
                if (CommandHandler.numBlanks >= 10)
                    Console.ForegroundColor = ConsoleColor.Blue;

                Console.Write(">>> ");
                string cmd = Console.ReadLine();

                if (cmd == "exit")
                    break;

                CommandHandler.HandleCommand(cmd);
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace cmdApp
{
    public class InputHandler
    {
        public enum CommandType
        {
            InvalidCommand,
            EmptyCommand,
            ClearCommand,
            ListCommand,
            DescriptionCommand,
            GenerateRandomCommand,
            PrintCommand,
            AddCommand,
            SubCommand,
            MultCommand,
            DivCommand,
            CountCommand,
            CoinFlipCommand,
            RockPaperScissorsCommand,
            RandomChoiceCommand,
            LoopCommand,
            FibonacciCommand,
            TitleCommand,
            RandomStringCommand,

            RootCommand,
            CreditsCommand
        }

        public static Dictionary<string, CommandType> validCommands = new Dictionary<string, CommandType>
        {
            { "clear", CommandType.ClearCommand },
            { "clr", CommandType.ClearCommand },
            { "help", CommandType.ListCommand },
            { "cmds", CommandType.ListCommand },
            { "listcommands", CommandType.ListCommand },
            { "listcmds", CommandType.ListCommand },

            { "describe", CommandType.DescriptionCommand },
            { "describecommand", CommandType.DescriptionCommand },
            { "desc", CommandType.DescriptionCommand },
            { "describecmd", CommandType.DescriptionCommand },
            { "define", CommandType.DescriptionCommand },
            { "definecommand", CommandType.DescriptionCommand },
            { "definecmd", CommandType.DescriptionCommand },
            { "def", CommandType.DescriptionCommand },
            { "info", CommandType.DescriptionCommand },

            { "random", CommandType.GenerateRandomCommand },
            { "rand", CommandType.GenerateRandomCommand },

            { "print", CommandType.PrintCommand },
            { "write", CommandType.PrintCommand },
            { "echo", CommandType.PrintCommand },

            { "add", CommandType.AddCommand },
            { "subtract", CommandType.SubCommand },
            { "sub", CommandType.SubCommand },
            { "multiply", CommandType.MultCommand },
            { "mult", CommandType.MultCommand },
            { "divide", CommandType.DivCommand },
            { "div", CommandType.DivCommand },

            { "count", CommandType.CountCommand },

            { "flipcoin", CommandType.CoinFlipCommand },
            { "coinflip", CommandType.CoinFlipCommand },

            { "rockpaperscissors", CommandType.RockPaperScissorsCommand },
            { "rps", CommandType.RockPaperScissorsCommand },

            { "randomchoice", CommandType.RandomChoiceCommand },
            { "randchoice", CommandType.RandomChoiceCommand },
            { "rchoice", CommandType.RandomChoiceCommand },
            { "wheelspin", CommandType.RandomChoiceCommand },
            { "wspin", CommandType.RandomChoiceCommand },
            { "spinwheel", CommandType.RandomChoiceCommand },

            { "fibonacci", CommandType.FibonacciCommand },
            { "fib", CommandType.FibonacciCommand },

            { "loop", CommandType.LoopCommand },
            { "repeat", CommandType.LoopCommand },

            { "title", CommandType.TitleCommand },

            { "randomstring", CommandType.RandomStringCommand },
            { "randstring", CommandType.RandomStringCommand },
            { "rstring", CommandType.RandomStringCommand },
            { "randstr", CommandType.RandomStringCommand },


            { "credit", CommandType.CreditsCommand },
            { "credits", CommandType.CreditsCommand },
        };

        public static Dictionary<string, CommandType> hiddenCommands = new Dictionary<string, CommandType>
        {
            { "root", CommandType.RootCommand }
        };

        public static Dictionary<CommandType, string> commandDescriptions = new Dictionary<CommandType, string>
        {
            { CommandType.ClearCommand, "Clears the console" },
            { CommandType.ListCommand, "Lists all valid commands" },
            { CommandType.DescriptionCommand, "Describes command given" },
            { CommandType.GenerateRandomCommand, "Generates a random number" },
            { CommandType.PrintCommand, "Prints a message to the console" },
            { CommandType.AddCommand, "Adds a variable set of numbers" },
            { CommandType.SubCommand, "Subtracts the second number from the first number" },
            { CommandType.MultCommand, "Multiplies a variable set of numbers" },
            { CommandType.DivCommand, "Divides the first number by the second number" },
            { CommandType.CountCommand, "Counts from a minimum to a maximum number" },
            { CommandType.CoinFlipCommand, "Flips a coin and returns Heads or Tails" },
            { CommandType.RockPaperScissorsCommand, "Plays a game of Rock, Paper, Scissors" },
            { CommandType.RandomChoiceCommand, "Selects a random choice from a list" },
            { CommandType.LoopCommand, "Repeat another command a certain amount of times" },
            { CommandType.FibonacciCommand, "Lists the numbers in the Fibonacci Sequence" },
            { CommandType.TitleCommand, "Sets the title of the command prompt window" }
        };

        public static CommandType HandleInput(string input)
        {
            input = input.Trim();

            if (string.IsNullOrEmpty(input))
                return CommandType.EmptyCommand;

            string[] args = input.ToLower().Split(' ');
            string command = args[0];

            if (validCommands.TryGetValue(command, out CommandType commandType))
                return commandType;

            if (hiddenCommands.TryGetValue(command, out CommandType hiddenCommandType))
                return hiddenCommandType;

            return CommandType.InvalidCommand;
        }
    }
}

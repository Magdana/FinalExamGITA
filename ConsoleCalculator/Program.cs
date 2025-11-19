using System.Globalization;

namespace ConsoleCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                double num1 = GetFirstNumber();
                double num2 = GetSecondNumber();
                string? operation = GetOperator();
                try
                {
                    CalculationOperation(num1, num2, operation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }

                Console.WriteLine();
                Console.Write("Press Enter to perform another calculation or type 'exit' to quit: ");
                string? prompt = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(prompt) && prompt.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                Console.Clear();
            }
        }

        private static string? GetOperator()
        {
            return ReadOperator("Enter operation (+, -, *, /): ");
        }

        private static string? ReadOperator(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input required. Please enter a right operator.");
                    continue;
                }

                if (input == "+" || input == "-" || input == "*" || input == "/")
                {
                    return input;
                }

                Console.WriteLine("Invalid operator format. Please try again.");
            }
        }

        private static double GetSecondNumber()
        {
            return ReadDouble("Enter second number: ");
        }

        private static double GetFirstNumber()
        {
            return ReadDouble("Enter first number: ");
        }

        private static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input required. Please enter a number.");
                    continue;
                }

                if (double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out double value))
                {
                    return value;
                }

                Console.WriteLine("Invalid number format. Please try again.");
            }
        }

        private static void CalculationOperation(double num1, double num2, string? operation)
        {
            if (string.IsNullOrEmpty(operation))
            {
                Console.WriteLine("No operation provided.");
                return;
            }

            switch (operation)
            {
                case "+":
                    Console.WriteLine($"Result: {num1 + num2}");
                    break;
                case "-":
                    Console.WriteLine($"Result: {num1 - num2}");
                    break;
                case "*":
                    Console.WriteLine($"Result: {num1 * num2}");
                    break;
                case "/":
                    if (num2 == 0)
                    {
                        Console.WriteLine("Cannot divide by zero.");
                    }
                    else
                    {
                        Console.WriteLine($"Result: {num1 / num2}");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid operation.");
                    break;
            }
        }
    }
}

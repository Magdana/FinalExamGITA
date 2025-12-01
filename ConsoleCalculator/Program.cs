using System.Globalization;

namespace ConsoleCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        double num1 = GetFirstNumber();
                        double num2 = GetSecondNumber();
                        string? operation = GetOperator();
                        
                        CalculationOperation(num1, num2, operation);
                    }
                    catch (DivideByZeroException ex)
                    {
                        Console.WriteLine($"\n✗ Division Error: {ex.Message}");
                    }
                    catch (OverflowException ex)
                    {
                        Console.WriteLine($"\n✗ Calculation Overflow: {ex.Message}");
                    }
                    catch (ArgumentNullException ex)
                    {
                        Console.WriteLine($"\n✗ Invalid Input: {ex.Message}");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"\n✗ Operation Error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\n✗ An unexpected error occurred: {ex.Message}");
                    }

                    Console.WriteLine();
                    Console.Write("Press Enter to perform another calculation or type 'exit' to quit: ");
                    string? prompt = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(prompt) && prompt.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("\nThank you for using the calculator. Goodbye!");
                        break;
                    }

                    Console.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Fatal error in calculator application: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static string? GetOperator()
        {
            return ReadOperator("Enter operation (+, -, *, /): ");
        }

        private static string? ReadOperator(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                throw new ArgumentNullException(nameof(prompt), "Prompt cannot be null or empty.");
            }

            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    string? input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Input required. Please enter a valid operator.");
                        continue;
                    }

                    if (input == "+" || input == "-" || input == "*" || input == "/")
                    {
                        return input;
                    }

                    Console.WriteLine("Invalid operator. Please enter one of: +, -, *, /");
                }
                catch (OutOfMemoryException)
                {
                    throw new InvalidOperationException("System is out of memory. Please restart the application.");
                }
                catch (IOException ex)
                {
                    throw new InvalidOperationException($"Input/Output error: {ex.Message}");
                }
            }
        }

        private static double GetSecondNumber()
        {
            return GetNumberFromPrompt("Enter second number: ");
        }

        private static double GetFirstNumber()
        {
            return GetNumberFromPrompt("Enter first number: ");
        }

        private static double GetNumberFromPrompt(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                throw new ArgumentNullException(nameof(prompt), "Prompt cannot be null or empty.");
            }

            while (true)
            {
                try
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
                        if (double.IsInfinity(value))
                        {
                            throw new OverflowException("The number is too large or too small.");
                        }

                        if (double.IsNaN(value))
                        {
                            throw new InvalidOperationException("The input resulted in an invalid number.");
                        }

                        return value;
                    }

                    Console.WriteLine("Invalid number format. Please try again.");
                }
                catch (OutOfMemoryException)
                {
                    throw new InvalidOperationException("System is out of memory. Please restart the application.");
                }
                catch (IOException ex)
                {
                    throw new InvalidOperationException($"Input/Output error: {ex.Message}");
                }
            }
        }

        private static void CalculationOperation(double num1, double num2, string? operation)
        {
            if (string.IsNullOrEmpty(operation))
            {
                throw new ArgumentNullException(nameof(operation), "No operation provided.");
            }

            try
            {
                switch (operation)
                {
                    case "+":
                        double addResult = num1 + num2;
                        if (double.IsInfinity(addResult))
                        {
                            throw new OverflowException("The result of addition is too large.");
                        }
                        Console.WriteLine($"\nResult: {num1} + {num2} = {addResult}");
                        break;

                    case "-":
                        double subtractResult = num1 - num2;
                        if (double.IsInfinity(subtractResult))
                        {
                            throw new OverflowException("The result of subtraction is too large.");
                        }
                        Console.WriteLine($"\nResult: {num1} - {num2} = {subtractResult}");
                        break;

                    case "*":
                        double multiplyResult = num1 * num2;
                        if (double.IsInfinity(multiplyResult))
                        {
                            throw new OverflowException("The result of multiplication is too large.");
                        }
                        Console.WriteLine($"\nResult: {num1} × {num2} = {multiplyResult}");
                        break;

                    case "/":
                        if (num2 == 0)
                        {
                            throw new DivideByZeroException("Cannot divide by zero. Please enter a non-zero divisor.");
                        }

                        double divideResult = num1 / num2;
                        if (double.IsInfinity(divideResult))
                        {
                            throw new OverflowException("The result of division is too large.");
                        }
                        Console.WriteLine($"\nResult: {num1} ÷ {num2} = {divideResult}");
                        break;

                    default:
                        throw new InvalidOperationException($"Invalid operation: '{operation}'. Supported operations are: +, -, *, /");
                }
            }
            catch (OverflowException)
            {
                throw;
            }
            catch (DivideByZeroException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Calculation error: {ex.Message}", ex);
            }
        }
    }
}

using System.Runtime.CompilerServices;

namespace GameGuessTheNumber;

internal class GameService
{
    private static bool isWinner = false;
    public int Play()
    {
        int tries = 1;
        var randomNumber = GetRandomNumberInRange();
        if (randomNumber == 0)
        {
            return 0;
        }
        Console.Clear();
        do
        {
            
            for (int attempts = 10; attempts > 0; attempts--)
            {
                Console.WriteLine($"You have {attempts} attempts left. Enter your guess:");
                var raw = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(raw, out var guess))
                {

                    if (guess == randomNumber)
                    {
                        Console.WriteLine($"Congratulations! You've guessed the number in {tries} attempts!");
                        isWinner = true;
                    }
                    else if(attempts - 1 == 0)
                    {
                        Console.WriteLine($"Game over! You have lost the game. Correct number was {randomNumber}.");
                    }
                    else
                    {
                        Console.WriteLine(guess < randomNumber ? "Too low!" : "Too high!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
                if (isWinner)
                {
                    break;
                }
                
                    tries++;
            }
            return tries;
        }
        while (!isWinner);
    }



    private static int GetRandomNumberInRange()
    {
        var level = SelectGamingLevel();
        switch (level)
        {
            case 1:
                return GetRandomNumber(1, 15);
            case 2:
                return GetRandomNumber(1, 25);
            case 3:
                return GetRandomNumber(1, 50);
            default:
                Console.WriteLine("Invalid level selection.");
                return 0;
        }
    }
    private static int GetRandomNumber(int min, int max)
    {
        var random = new Random();
        return random.Next(min, max + 1);
    }

    private static int SelectGamingLevel()
    {
        while (true)
        {
            Console.WriteLine("Select game level to play:");
            Console.WriteLine("Easy level (numbers in 1-15), enter '1'\nMedium level (numbers in 1-25), enter '2'\nHard level (numbers in 1 - 50), enter '3')");
            var raw = Console.ReadLine() ?? string.Empty;
            if (int.TryParse(raw, out var input) && (input >= 1 && input <= 3))
            {
                return input;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 1, 2, or 3.");
                Console.WriteLine();
            }
        }
    }
}

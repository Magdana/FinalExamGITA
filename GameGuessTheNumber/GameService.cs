namespace GameGuessTheNumber;

internal class GameService
{
    public int Play()
    {
        try
        {
            var randomNumber = GetRandomNumberInRange();
            Console.Clear();
            if (randomNumber == 0)
            {
                Console.WriteLine("Could not start the game due to an invalid level selection.");
                return -1;
            }

            int tries = 0;
            bool isWinner = false;

            for (int attempts = 10; attempts > 0; attempts--)
            {
                Console.WriteLine($"You have {attempts} attempts left. Enter your guess:");
                var raw = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(raw, out var guess))
                {
                    tries++;
                    if (guess == randomNumber)
                    {
                        Console.WriteLine($"Congratulations! You've guessed the number in {tries} attempt!");
                        isWinner = true;
                        break;
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
            }

            if (!isWinner)
            {
                Console.WriteLine($"Sorry, you've run out of attempts! The number was {randomNumber}.");
            }

            return tries;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred during the game: {ex.Message}");
            return -1;
        }
    }

    private int GetRandomNumberInRange()
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while selecting the level: {ex.Message}");
            return 0;
        }
    }

    private int GetRandomNumber(int min, int max)
    {
        var random = new Random();
        return random.Next(min, max + 1);
    }

    private int SelectGamingLevel()
    {
        Console.Clear();
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

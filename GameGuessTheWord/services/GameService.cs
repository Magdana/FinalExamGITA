
using GameGuessTheWord.Entities;
using System.Data;

namespace GameGuessTheWord.services;

internal class GameService
{
    private readonly UserService _userService;
    public GameService(UserService userService)
    {
        _userService = userService;
    }
    public void StartGame()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== GUESS THE NUMBER =====");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Leaderboard");
            Console.WriteLine("4. Exit");
            Console.Write("Choose: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _userService.Register();
                    break;
                case "2":
                    var user = _userService.Login();
                    if (user != null)
                    {
                        Console.Clear();
                        Console.WriteLine($"Welcome, {user.UserName}!");

                        Console.WriteLine("\nPress ENTER to start the game...");
                        Console.ReadLine();

                        int attempts = Play();

                        if (attempts >= 0)
                        {
                            _userService.UpdateUserScore(user, attempts);
                        }
                    }
                    break;
                case "3":
                    _userService.ShowLeaderboard();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }

    }

    private int Play()
    {
        try
        {
            var randomWord = GetRandomWord();
            var asteriskedWord = ShowWordPartilly(randomWord);
            Console.Clear();

            int tries = 0;
            bool isWinner = false;

            for (int attempts = 6; attempts > 0; attempts--)
            {
                Console.WriteLine($"You have {attempts} attempts left. Enter the letter: ");
                var raw = Console.ReadLine().ToLower() ?? string.Empty;
                int rowLength = raw.Length;

                tries++;
                if (string.IsNullOrWhiteSpace(raw))
                {
                    Console.WriteLine("Please enter a letter(s).");
                    continue;
                }
                for (int i = 0; i < randomWord.Length; i++)
                {
                    if (asteriskedWord[i] != '*')
                    {
                        continue;
                    }
                    if (randomWord.Contains(raw))
                    {
                        asteriskedWord = asteriskedWord.Remove(i, rowLength).Insert(i, raw);
                        Console.WriteLine($"Congratulations! You've guessed the letter, the letter is in the {randomWord[i + 1]} place in the word");
                    }
                    if (asteriskedWord.Equals(randomWord, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Congratulations! You've guessed the word: {randomWord}");
                        isWinner = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("wrong letter, try again!");
                        break;
                    }
                }
            }
            Console.WriteLine(asteriskedWord);

            if (!isWinner)
            {
                Console.WriteLine($"Sorry, you've run out of attempts! The number was {randomWord}.");
            }

            return tries;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred during the game: {ex.Message}");
            return -1;
        }
    }

    private string ShowWordPartilly(string wordToGuess)
    {
        string asteriskedWord = new string('*', wordToGuess.Length);
        return asteriskedWord;
    }


    private string GetRandomWord()
    {
        List<string> words = new List<string>
        {
            "apple", "banana", "orange", "grape", "kiwi",
            "strawberry", "pineapple", "blueberry", "peach", "watermelon"
        };
        var random = new Random();
        return words[random.Next(0, words.Count)];
    }
}

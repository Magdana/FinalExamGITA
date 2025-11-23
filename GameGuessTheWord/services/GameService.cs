using System.Text;

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
            var guessedLetters = new StringBuilder(new string('*', randomWord.Length));
            var wrongGuesses = new List<char>();
            Console.Clear();

            bool isWinner = false;
            int attempts = 6;
            int tries = 0;

            while (attempts > 0 && !isWinner)
            {
                Console.WriteLine($"The word is a name of fruit: {guessedLetters}\n");
                Console.WriteLine($"Wrong guesses: {string.Join(", ", wrongGuesses)}\n");
                Console.WriteLine($"You have {attempts} attempts left. Enter a letter or the full word: \n");
                var input = Console.ReadLine()?.ToLower();
                attempts--;
                tries++;
                Console.Clear();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Please enter a letter or the full word.");
                    continue;
                }
                
                if (input.Length > 1)
                {
                    if (input.Equals(randomWord, StringComparison.OrdinalIgnoreCase))
                    {
                        isWinner = true;
                    }
                    else
                    {
                        Console.WriteLine("That's not the correct word. Try again!");
                    }
                }
                else
                {
                    char guessedChar = input[0];
                    if (!char.IsLetter(guessedChar))
                    {
                        Console.WriteLine("Please enter a valid letter.");
                        continue;
                    }

                    bool alreadyGuessed = guessedLetters.ToString().Contains(guessedChar) || wrongGuesses.Contains(guessedChar);

                    if (alreadyGuessed)
                    {
                        Console.WriteLine("You have already guessed that letter. Try another one.");
                        continue;
                    }

                    bool found = false;
                    for (int i = 0; i < randomWord.Length; i++)
                    {
                        if (randomWord[i] == guessedChar)
                        {
                            guessedLetters[i] = guessedChar;
                            found = true;
                        }
                    }

                    if (found)
                    {
                        Console.WriteLine("Good guess!");
                        if (guessedLetters.ToString().Equals(randomWord, StringComparison.OrdinalIgnoreCase))
                        {
                            isWinner = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong letter, try again!");
                        wrongGuesses.Add(guessedChar);
                    }
                }
                Console.WriteLine();
            }

            Console.Clear();
            if (isWinner)
            {
                Console.WriteLine($"Congratulations! You've guessed the word: {randomWord} in {tries} attempts");
            }
            else
            {
                Console.WriteLine($"Sorry, you've run out of attempts! The word was: {randomWord}.");
            }

            return tries;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred during the game: {ex.Message}");
            return -1;
        }
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

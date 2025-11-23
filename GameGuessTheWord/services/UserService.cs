using GameGuessTheWord.Entities;
using GameGuessTheWord.Repositories;

namespace GameGuessTheWord.services;

internal class UserService
{
    private readonly UserRepository _userRepository;
    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User? GetUserByUsername(string username)
    {
        try
        {
            return _userRepository.GetUserByUsername(username);
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public List<User> GetTopTenUsers()
    {
        try
        {
            return _userRepository.GetTopTenUsers();
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
            return new List<User>();
        }
    }

    public void Register()
    {
        try
        {
            Console.Clear();
            Console.Write("Enter your name: ");
            string? name = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter username: ");
            string? username = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter password: ");
            string? password = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("All fields are required!");
                return;
            }

            User user = new User
            {
                Name = name,
                UserName = username,
                Password = password,
                HigestScore = null
            };

            _userRepository.AddUser(user);


            Console.WriteLine("Registration successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }


    public User? Login()
    {
        try
        {
            Console.Clear();
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? string.Empty;

            var user = _userRepository.GetUserByUsername(username);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return null;
            }

            Console.Write("Password: ");
            string pass = Console.ReadLine() ?? string.Empty;

            if (user.Password != pass)
            {
                Console.WriteLine("Wrong password.");
                return null;
            }

            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during login: {ex.Message}");
            return null;
        }
    }
    public void UpdateUserScore(User user, int newScore)
    {
        try
        {
            _userRepository.UpdateUserScore(user, newScore);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user score: {ex.Message}");
        }
    }

    public void ShowLeaderboard()
    {
        Console.Clear();
        Console.WriteLine("===== LEADERBOARD =====\n");

        var topTen = GetTopTenUsers();

        if (topTen.Count == 0)
        {
            Console.WriteLine("No scores yet!");
            return;
        }

        int rank = 1;
        Console.WriteLine($"{"Rank",-7}{"User",-13}{"Best Score"}");
        Console.WriteLine(new string('-', 35));

        foreach (var u in topTen)
        {
            Console.WriteLine($"{rank,-7}{u.Name,-13}{u.HigestScore}");
            rank++;
        }

    }
}
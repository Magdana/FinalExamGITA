namespace GameGuessTheNumber;

internal class UserService 
{
    private readonly string _csvFile;
    private readonly List<User> _users;

    public UserService(string csvFile)
    {
        _csvFile = csvFile;
        _users = LoadUsers();
    }

    private List<User> LoadUsers()
    {
        var list = new List<User>();

        if (!File.Exists(_csvFile))
            return list;

        foreach (var line in File.ReadAllLines(_csvFile))
        {
            var parts = line.Split(',');

            list.Add(new User
            {
                Name = parts[0],
                UserName = parts[1],
                Password = parts[2],
                HigestScore = int.TryParse(parts[3], out int s) ? s : (int?)null
            });
        }

        return list;
    }

    public void SaveUsers()
    {
        var lines = _users
            .Select(u => $"{u.Name},{u.UserName},{u.Password},{u.HigestScore}")
            .ToArray();

        File.WriteAllLines(_csvFile, lines);
    }

    public void Register()
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

        if (_users.Any(u => u.UserName == username))
        {
            Console.WriteLine("Username already exists!");
            return;
        }

        _users.Add(new User
        {
            Name = name,
            UserName = username,
            Password = password,
            HigestScore = null
        });

        SaveUsers();

        Console.WriteLine("Registration successful!");
    }

    public User Login()
    {
        Console.Clear();
        Console.Write("Username: ");
        string username = Console.ReadLine() ?? string.Empty;

        var user = _users.FirstOrDefault(u => u.UserName == username);

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

    public void UpdateBestScore(User user, int newScore)
    {
        if (user.HigestScore == null || newScore < user.HigestScore)
        {
            user.HigestScore = newScore;
            Console.WriteLine($"New best score: {newScore}!");
        }
    }

    public void ShowLeaderboard()
    {
        Console.Clear();
        Console.WriteLine("===== LEADERBOARD =====\n");

        var ranked = _users
            .Where(u => u.HigestScore != null)
            .OrderBy(u => u.HigestScore)
            .Take(10)
            .ToList();

        if (ranked.Count == 0)
        {
            Console.WriteLine("No scores yet!");
            return;
        }

        int rank = 1;
        Console.WriteLine($"{"Rank",-7}{"User",-13}{"Best Score"}");
        Console.WriteLine(new string('-', 35));

        foreach (var u in ranked)
        {
            Console.WriteLine($"{rank,-7}{u.Name, -13}{u.HigestScore}");
            rank++;
        }

    }
}

namespace GameGuessTheNumber;

internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            const string csvFile = "C:\\Users\\magda\\Desktop\\FinalExamGITA\\GameGuessTheNumber\\Records.csv";
            var userService = new UserService(csvFile);
            var gameService = new GameService();


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
                        userService.Register();
                        break;
                    case "2":
                        var user = userService.Login();
                        if (user != null)
                        {
                            Console.Clear();
                            Console.WriteLine($"Welcome, {user.UserName}!");

                            Console.WriteLine("\nPress ENTER to start the game...");
                            Console.ReadLine();

                            int attempts = gameService.Play();

                            if (attempts >= 0)
                            {
                                userService.UpdateBestScore(user, attempts);
                                userService.SaveUsers();
                            }
                        }
                        break;
                    case "3":
                        userService.ShowLeaderboard();
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
        catch (Exception ex)
        {
            Console.WriteLine($"A critical error occurred in the main application loop: {ex.Message}");
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}

namespace GameGuessTheNumber;

internal class Program
{
    static void Main(string[] args)
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

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                userService.Register();
            }
            else if (choice == "2")
            {
                var user = userService.Login();
                if (user != null)
                {
                    Console.Clear();
                    Console.WriteLine($"Welcome, {user.UserName}!");
                    int attempts = gameService.Play();

                    userService.UpdateBestScore(user, attempts);
                    userService.SaveUsers();
                }
            }
            else if (choice == "3")
            {
                userService.ShowLeaderboard();
            }
            else if (choice == "4")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }

    }
}

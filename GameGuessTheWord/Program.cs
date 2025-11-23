using GameGuessTheWord.Entities;
using GameGuessTheWord.services;

namespace GameGuessTheWord;

internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            string baseDirectory = AppContext.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\.."));
            string filePath = Path.Combine(projectRoot, "users.xml");

            var userService = new UserService(new Repositories.UserRepository(filePath));
            var gameService = new GameService(userService);
            gameService.StartGame();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}

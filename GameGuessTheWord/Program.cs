using GameGuessTheWord.Entities;
using GameGuessTheWord.services;

namespace GameGuessTheWord;

internal class Program
{
    static void Main(string[] args)
    {
        string baseDirectory = AppContext.BaseDirectory;
        string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\.."));
        string filePath = Path.Combine(projectRoot, "users.xml");

        var userService = new UserService(new Repositories.UserRepository(filePath));
        var gameService = new GameService(userService);
        gameService.StartGame();
    }
}

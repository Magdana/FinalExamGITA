using GameGuessTheWord.Entities;
using GameGuessTheWord.services;

namespace GameGuessTheWord;

internal class Program
{
    static void Main(string[] args)
    {
        var userService = new UserService(new Repositories.UserRepository(new List<User>()));
        var gameService = new GameService(userService);
        gameService.StartGame();
    }
}

using ATMOperations.Services;

namespace ATMOperations;

internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            string baseDirectory = AppContext.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\.."));
            string filePathForUSersInfo = Path.Combine(projectRoot, "users.json");
            string filePathForOperationsInfo = Path.Combine(projectRoot, "operations.json");

            var userService = new UserService(new Repositories.UserRepository(filePathForUSersInfo, new Repositories.OperationsRepository(filePathForOperationsInfo)));
            var opService = new OperationService(userService);
            opService.StartATM();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}

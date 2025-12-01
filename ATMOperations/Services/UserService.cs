using ATMOperations.Entities;
using ATMOperations.Repositories;

namespace ATMOperations.Services;

internal class UserService
{
    private readonly UserRepository _userRepository;
    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public User? GetUserByPersonalId(string id)
    {
        try
        {
            return _userRepository.GetUserByPersonalNumber(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public void Register()
    {
        try
        {
            Console.Clear();
            Console.Write("Enter your name: ");
            string? name = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter lastname: ");
            string? lastname = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(lastname))
            {
                Console.WriteLine("All fields are required!");
                return;
            }
            string? personalid = string.Empty;
            while (true)
            {
                Console.Write("Enter your personal number (11 digits): ");
                personalid = Console.ReadLine() ?? string.Empty;
                
                if (personalid.Length == 11 && personalid.All(char.IsDigit))
                {
                    break;
                }
                Console.WriteLine("Invalid format. Personal number must be exactly 11 digits.");
            }

            if (_userRepository.GetUserByPersonalNumber(personalid) != null)
            {
                Console.WriteLine("Username already exists!");
                return;
            }
            User user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                LastName = lastname,
                PersonalNumber = personalid
            }; 

            _userRepository.AddUser(user);

            Console.WriteLine($"Registration successful! Your password is {user.Password}");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during registration: {ex.Message}");
        }
    }


    public User? Login()
    {
        try
        {
            Console.Clear();
            Console.Write("personal ID: ");
            string personal = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(personal))
            {
                Console.WriteLine("Personal ID cannot be empty.");
                return null;
            }

            var user = _userRepository.GetUserByPersonalNumber(personal);

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

    public double BalanceCheck(User user)
    {
        try
        {
            return _userRepository.BalanceCheck(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking balance: {ex.Message}");
            throw;
        }
    }
    public void Deposit(User user, double amount)
    {
        try
        {
            _userRepository.Deposit(user, amount);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during deposit: {ex.Message}");
            throw;
        }
    }
    public void Withdraw(User user, double amount)
    {
        try
        {
            _userRepository.Withdraw(user, amount);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during withdrawal: {ex.Message}");
            throw;
        }
    }
    public string ViewHistoryOfOperations(string personalid)
    {
        try
        {
            return _userRepository.ViewHistoryOfOperations(personalid);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving operation history: {ex.Message}");
            throw;
        }
    }
}


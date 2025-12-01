using ATMOperations.Entities;
using System.Text.Json;
namespace ATMOperations.Repositories;

internal class UserRepository
{
    private List<User> _users;
    private readonly string _filePath;
    private readonly OperationsRepository _operationsRepository;
    
    public UserRepository(string filePath, OperationsRepository operationsRepository)
    {
        _filePath = filePath;
        _operationsRepository = operationsRepository;
        _users = LoadUsers();
    }
    private List<User> LoadUsers()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return new List<User>();
            }

            var users = new List<User>();
            string jsonString = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return new List<User>();
            }
            users = JsonSerializer.Deserialize<List<User>>(jsonString) ?? new List<User>();
            return users;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON error while loading users: {ex.Message}");
            return new List<User>();
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File error while loading users: {ex.Message}");
            return new List<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while loading users: {ex.Message}");
            return new List<User>();
        }
    }
    private void SaveUsers()
    {
        try
        {
            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string jsonString = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonString);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File error while saving users: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while saving users: {ex.Message}");
        }
    }
    public User? GetUserByPersonalNumber(string id)
    {
        try
        {
            return _users.FirstOrDefault(u => u.PersonalNumber.Equals(id, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving user: {ex.Message}");
            return null;
        }
    }

    public User AddUser(User user)
    {
        try
        {
            if (GetUserByPersonalNumber(user.PersonalNumber) != null)
            {
                throw new Exception("User with the same Personal ID already exists.");
            }
            _users.Add(user);
            SaveUsers();
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding user: {ex.Message}");
            throw;
        }
    }


    public double BalanceCheck(User user)
    {
        try
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            var existingUser = GetUserByPersonalNumber(user.PersonalNumber);
            if (existingUser != null)
            {
                _operationsRepository.LogOperation(OperationType.BalanceInquiry, 0, user.PersonalNumber);
                return existingUser.Balance;
            }
            throw new Exception("User not found.");
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
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero.", nameof(amount));
            }

            var userToUpdate = GetUserByPersonalNumber(user.PersonalNumber);
            if (userToUpdate == null)
            {
                throw new Exception("User not found.");
            }

            userToUpdate.Balance += amount;
            SaveUsers();
            _operationsRepository.LogOperation(OperationType.Deposit, (decimal)amount, user.PersonalNumber);
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
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be greater than zero.", nameof(amount));
            }

            var userToUpdate = GetUserByPersonalNumber(user.PersonalNumber);
            if (userToUpdate == null)
            {
                throw new Exception("User not found.");
            }

            if (userToUpdate.Balance < amount)
            {
                throw new Exception("Balance is not enough to withdraw!");
            }

            userToUpdate.Balance -= amount;
            SaveUsers();
            _operationsRepository.LogOperation(OperationType.Withdraw, (decimal)amount, user.PersonalNumber);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string ViewHistoryOfOperations(string personalid)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(personalid))
            {
                throw new ArgumentException("Personal ID cannot be null or empty.", nameof(personalid));
            }

            var existingUser = GetUserByPersonalNumber(personalid);
            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            var userOperations = _operationsRepository.GetOperationsByPersonalNumber(personalid);
            
            if (userOperations.Count == 0)
            {
                return "No operations found for this user.";
            }

            return string.Join(Environment.NewLine, userOperations.Select(op => $"user - {existingUser.Name} {existingUser.LastName}, {op.OperationType}, amount {op.Amount}, time {op.Timestamp}"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving operation history: {ex.Message}");
            throw;
        }
    }

}

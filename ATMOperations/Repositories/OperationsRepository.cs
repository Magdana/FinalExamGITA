using ATMOperations.Entities;
using System.Text.Json;

namespace ATMOperations.Repositories;

internal class OperationsRepository
{
    private readonly string _filePath;
    private List<Operation> _operations;

    public OperationsRepository(string filePath)
    {
        _filePath = filePath;
        _operations = LoadOperations();
    }

    private List<Operation> LoadOperations()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return new List<Operation>();
            }

            string jsonString = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return new List<Operation>();
            }

            var operations = JsonSerializer.Deserialize<List<Operation>>(jsonString);
            return operations ?? new List<Operation>();
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON error while loading operations: {ex.Message}");
            return new List<Operation>();
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File error while loading operations: {ex.Message}");
            return new List<Operation>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while loading operations: {ex.Message}");
            return new List<Operation>();
        }
    }

    private void SaveOperations()
    {
        try
        {
            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string jsonString = JsonSerializer.Serialize(_operations, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonString);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File error while saving operations: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while saving operations: {ex.Message}");
        }
    }

    public void LogOperation(OperationType operationType, decimal amount, string personalNumber)
    {
        try
        {
            var logEntry = new Operation
            {
                OperationType = operationType,
                Amount = amount,
                PersonalNumber = personalNumber
            };
            
            _operations.Add(logEntry);
            SaveOperations();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to log operation: {ex.Message}");
        }
    }

    public List<Operation> GetOperationsByPersonalNumber(string personalNumber)
    {
        try
        {
            return _operations
                .Where(op => op.PersonalNumber.Equals(personalNumber, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while reading operations: {ex.Message}");
            return new List<Operation>();
        }
    }
}

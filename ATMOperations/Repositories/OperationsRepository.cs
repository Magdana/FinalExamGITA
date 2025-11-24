using ATMOperations.Entities;
using System.Text.Json;

namespace ATMOperations.Repositories;

internal class OperationsRepository
{
    private readonly string _filePath;
    public OperationsRepository(string filePath)
    {
        _filePath = filePath;
    }


    public void LogOperation(OperationType operationType, decimal amount, string personalNumber)
    {
        var logEntry = new Operation
        {
            OperationType = operationType,
            Amount = amount,
            PersonalNumber = personalNumber
        };
        string jsonLog = JsonSerializer.Serialize(logEntry);
        LogToJson(jsonLog);
    }

    private void LogToJson(string log)
    {
        try
        {
            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using (StreamWriter sw = new StreamWriter(_filePath, append: true))
            {
                sw.WriteLine(log);
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File error while logging operation: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while logging operation: {ex.Message}");
        }
    }

    public List<Operation> GetOperationsByPersonalNumber(string personalNumber)
    {
        var operations = new List<Operation>();
        
        try
        {
            if (!File.Exists(_filePath))
            {
                return operations;
            }

            var lines = File.ReadAllLines(_filePath);
            
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var operation = JsonSerializer.Deserialize<Operation>(line);
                    if (operation != null && operation.PersonalNumber.Equals(personalNumber, StringComparison.OrdinalIgnoreCase))
                    {
                        operations.Add(operation);
                    }
                }
                catch (JsonException)
                {
                    continue;
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File error while reading operations: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while reading operations: {ex.Message}");
        }

        return operations;
    }
}

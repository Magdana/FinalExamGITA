using System.ComponentModel.DataAnnotations;

namespace ATMOperations.Entities;

internal class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PersonalNumber { get; set; } = string.Empty;
    public string Password { get; set; } = GeneratePassword();
    public double Balance { get; set; } = 0;

    private static string GeneratePassword()
    {
        Random random = new Random();
        return random.Next(1000, 10000).ToString();
    }
}
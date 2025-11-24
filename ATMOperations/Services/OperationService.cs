using ATMOperations.Entities;

namespace ATMOperations.Services;

internal class OperationService
{
    private readonly UserService _userService;

    public OperationService(UserService userService)
    {
        _userService = userService;
    }


    public void StartATM()
    {
        try
        {
            while (true)
            {
                ShowOperationsMenuForStartAtmOperations();

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _userService.Register();
                        Console.WriteLine("\nPress ENTER to continue...");
                        break;
                    case "2":
                        var user = _userService.Login();
                        if (user != null)
                        {
                            Console.Clear();
                            Console.WriteLine($"Welcome, {user.Name}!");

                            Console.WriteLine("\nPress ENTER to start operation...");
                            Console.ReadLine();

                            StartOperations(user);

                        }
                        break;
                    case "3":
                        Console.WriteLine("Thank you for using our ATM. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");
                        break;
                }
                Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred in the ATM system: {ex.Message}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private void ShowOperationsMenuForStartAtmOperations()
    {
        Console.Clear();
        Console.WriteLine("===== ATM Operations =====");
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Login");
        Console.WriteLine("3. Exit");
        Console.Write("Choose: ");
    }

    private void StartOperations(User user)
    {
        try
        {
            while (true)
            {
                ShowOperationsMenuForStartinOperations();
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BalanceCheckOp(user);
                        break;
                    case "2":
                        DepositOp(user);
                        break;
                    case "3":
                        WitdrawOp(user);
                        break;
                    case "4":
                        HistoryCheckOp(user);
                        break;
                    case "5":
                        Console.WriteLine("Logging out. Thank you!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please select 1-5.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.WriteLine("Press ENTER to return to main menu...");
            Console.ReadLine();
        }
    }

    private void HistoryCheckOp(User user)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("===== Operation History =====\n");
            
            string history = _userService.ViewHistoryOfOperations(user.PersonalNumber);
            Console.WriteLine(history);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Failed to retrieve operation history: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
    }

    private void WitdrawOp(User user)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("===== Withdraw =====\n");
            
            var operationTypeOfWithdraw = OperationType.Withdraw;
            var requestedamountTowithdraw = RequestForAmount(operationTypeOfWithdraw);
            
            double balanceBeforeWithdraw = user.Balance;
            _userService.Withdraw(user, requestedamountTowithdraw);
            
            var updatedUser = _userService.GetUserByPersonalId(user.PersonalNumber);
            if (updatedUser != null)
            {
                user.Balance = updatedUser.Balance;
            }
            
            Console.WriteLine($"\n✓ Withdrawal successful!");
            Console.WriteLine($"Amount withdrawn: {requestedamountTowithdraw:F2} GEL");
            Console.WriteLine($"Previous balance: {balanceBeforeWithdraw:F2} GEL");
            Console.WriteLine($"New balance: {user.Balance:F2} GEL");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Withdrawal failed: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
    }

    private void DepositOp(User user)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("===== Deposit =====\n");
            
            var operationTypeOfDeposit = OperationType.Deposit;
            var requestedamounttodeposit = RequestForAmount(operationTypeOfDeposit);
            
            double balanceBeforeDeposit = user.Balance;
            _userService.Deposit(user, requestedamounttodeposit);
            
            var updatedUser = _userService.GetUserByPersonalId(user.PersonalNumber);
            if (updatedUser != null)
            {
                user.Balance = updatedUser.Balance;
            }
            
            Console.WriteLine($"\n✓ Deposit successful!");
            Console.WriteLine($"Amount deposited: {requestedamounttodeposit:F2} GEL");
            Console.WriteLine($"Previous balance: {balanceBeforeDeposit:F2} GEL");
            Console.WriteLine($"New balance: {user.Balance:F2} GEL");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Deposit failed: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
    }

    private void BalanceCheckOp(User user)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("===== Balance Check =====\n");
            
            double balance = _userService.BalanceCheck(user);
            
            var updatedUser = _userService.GetUserByPersonalId(user.PersonalNumber);
            if (updatedUser != null)
            {
                user.Balance = updatedUser.Balance;
            }
            
            Console.WriteLine($"Account holder: {user.Name} {user.LastName}");
            Console.WriteLine($"Personal Number: {user.PersonalNumber}");
            Console.WriteLine($"Current balance: {balance:F2} GEL");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Balance check failed: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
    }

    private void ShowOperationsMenuForStartinOperations()
    {
        Console.Clear();
        Console.WriteLine("===== Choose operation =====");
        Console.WriteLine("1. Check your balance");
        Console.WriteLine("2. Deposit");
        Console.WriteLine("3. Withdraw");
        Console.WriteLine("4. Check your operations history");
        Console.WriteLine("5. Logout");
        Console.Write("Choose: ");
    }

    private double RequestForAmount(OperationType operation)
    {
        try
        {
            double amount;
            Console.Write($"Enter amount to {operation}: ");
            while (!double.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                Console.Write("Invalid amount. Please enter a positive number: ");
            }
            return amount;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading amount: {ex.Message}");
            throw;
        }

    }
}

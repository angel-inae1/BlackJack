using BlackJack.BussinesLogic.BlackJack.Models;

namespace BlackJack.BussinesLogic.Money;

public class MoneyLogic
{
    public decimal InitialBet(User user)
    {
        while (true)
        {
            Console.Write("Сделайте ставку: \t");
            string? input = Console.ReadLine();

            if (decimal.TryParse(input, out decimal bet))
            {
                if (bet > 0 && bet <= user.Balance)
                {
                    Console.WriteLine($"Вы сделали ставку {bet} руб.\tВаш баланс {user.Balance-bet} руб.");
                    Console.WriteLine();
                    return bet;
                }
                else
                {
                    Console.WriteLine("Ставка должна быть больше 0 и не превышать ваш текущий баланс.");
                }
            }
            else
            {
                Console.WriteLine("Попробуйте снова");
            }
            
        }
    }

    public void ResultBet(User user, decimal bet, string result, UserManager userManager)
    {
        switch (result)
        {
            case "win":
                user.Balance += bet * 2m;
                break;
            case "lose":
                break;
            case "blackjack":
                decimal winBj = bet + bet * 1.5m;
                user.Balance += winBj;
                break;
            case "draw":
                user.Balance += bet;
                break;
            default:
                Console.WriteLine("Неизвестный результат.");
                break;
        }

        userManager.SaveUsers();
    }

    public void TopUpBalance(User user, UserManager userManager)
    {
        Console.Write("Хотите пополнить баланс? y/n\t");
        string? choice = Console.ReadLine()?.ToLower();
        if (choice != "y")
            return;
        while (true)
        {
            Console.Write("Введите сумму пополнения...\t");
            string? input = Console.ReadLine();
            if ((decimal.TryParse(input, out decimal amount) && amount > 0))
            {
                user.Balance += amount;
                Console.WriteLine($"Баланс успешно пополнен на {amount} руб. Ваш баланс составляет {user.Balance} руб.");
                Console.WriteLine();
                userManager.SaveUsers();
                break;
            }
            else
            {
                Console.WriteLine("Сумма должна быть положительной.");
            }
        }
    }
}
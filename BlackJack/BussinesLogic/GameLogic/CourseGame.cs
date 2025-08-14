using BlackJack.BussinesLogic.Base;
using BlackJack.BussinesLogic.BlackJack.Models;
using BlackJack.BussinesLogic.Members;
using BlackJack.BussinesLogic.Money;

namespace BlackJack.BussinesLogic.GameLogic;

public class CourseGame
{
    public void Hit(Player player, Deck deck, Player dealer)
    {
        while (true)
        {
            player.Hands[0].DisplayHands();

            int value = player.Hands[0].GetTotalValue();
            if (player.Hands[0].IsBust(value))
            {
                Console.WriteLine("Перебор очков. Вы проиграли.");
                Console.WriteLine();
                break;
            }
            else if (player.Hands[0].IsBlackJack(value))
            {
                Console.WriteLine("Блэкджэк!");
                Console.WriteLine();
                break;
            }

            Console.WriteLine();
            Console.Write("Взять карту? y/n\t");
            string? input = Console.ReadLine()?.ToLower();

            if (input == "y")
            {
                var card = deck.DrawCard();
                player.Hands[0].AddCard(card);
            }
            else if (input == "n")
            {
                Console.WriteLine("Набор карт в руку завершен.");
                Console.WriteLine();
                return;
            }
            else
            {
                Console.WriteLine("Выберите корректный ответ! y/n");
            }
        }
    }

    public static string? CheckForBlackJack(Player player, Player dealer)
    {
        bool playerBj = player.Hands[0].IsBlackJack(player.Hands[0].GetTotalValue());
        bool dealerBj = dealer.Hands[0].IsBlackJack(dealer.Hands[0].GetTotalValue());

        if (playerBj && dealerBj)
        {
            Console.WriteLine("У игрока и у дилера Блэк Джэк! Ничья!");
            return "draw";
        }
        else if (playerBj)
        {
            Console.WriteLine("У игрока Блэк Джэк! Вы выиграли.");
            return "blackjack";
        }
        else if (dealerBj)
        {
            Console.WriteLine("У дилера Блэк Джэк! Вы проишрали.");
            return "lose";
        }

        return null;
    }

    public void DoubleJack(Player player, Deck deck)
    {
        Console.WriteLine("Ставка удвоена! Вы получаете еще одну карту.");
        Console.WriteLine();

        var card = deck.DrawCard();
        player.Hands[0].AddCard(card);

        player.Hands[0].DisplayHands();

        int value = player.Hands[0].GetTotalValue();
        Console.WriteLine($"Сумма очков игрока:{value}");

        if (player.Hands[0].IsBust(value))
        {
            Console.WriteLine("Перебор очков. Вы проиграли.");
        }
        else if (player.Hands[0].IsBlackJack(value))
        {
            Console.WriteLine("Блэкджэк!");
        }


    }

    public void Split(Player player, Deck deck, Player dealer, decimal bet, MoneyLogic moneyLogic, User user,
        UserManager userManager)
    {
        if (user.Balance < bet)
        {
            Console.WriteLine("Сплит невозможен, у вас недостатачно денег для ставки.");
            Console.WriteLine();
            return;
        }

        user.Balance -= bet;
        userManager.SaveUsers();
        Console.WriteLine($"Ставка удвоенна, ваш баланс {user.Balance} руб.");
        Console.WriteLine();


        var originalHand = player.Hands[0];
        var card1 = originalHand.Cards[0];
        var card2 = originalHand.Cards[1];
        player.Hands.Clear();

        var hand1 = new Hand();
        hand1.AddCard(card1);
        hand1.AddCard(deck.DrawCard());

        var hand2 = new Hand();
        hand2.AddCard(card2);
        hand2.AddCard(deck.DrawCard());

        player.Hands.Add(hand1);
        player.Hands.Add(hand2);

        if (hand1.IsBlackJack(hand1.GetTotalValue()))
            Console.WriteLine("В руке 1 Блэк Джэк!");
        if (hand2.IsBlackJack(hand2.GetTotalValue()))
            Console.WriteLine("В руке 2 Блэк Джэк!");

        for (int i = 0; i < player.Hands.Count; i++)
        {
            Console.WriteLine($"Рука {i + 1}:");
            Hand currentHand = player.Hands[i];
            while (true)
            {
                currentHand.DisplayHands();

                int value = currentHand.GetTotalValue();
                if (currentHand.IsBust(value))
                {
                    Console.WriteLine("Перебор очков.");
                    Console.WriteLine();
                    break;
                }
                else if (player.Hands[0].IsBlackJack(value))
                {
                    Console.WriteLine("Блэкджэк!");
                    Console.WriteLine();
                    break;
                }

                Console.Write("Взять карту? y/n:\t");
                string? input = Console.ReadLine()?.ToLower();

                if (input == "y")
                {
                    var card = deck.DrawCard();
                    currentHand.AddCard(card);
                }
                else if (input == "n")
                {
                    Console.WriteLine("Набор карт в руку завершен.");
                    Console.WriteLine();
                    break;
                }
                else
                {
                    Console.WriteLine("Выберите корректный ответ! y/n");
                }
            }
        }

        var dealerLogic = new Dealer();
        dealerLogic.DealerTurn(player, dealer, deck);
        
        ResultGame(user, dealer, player, moneyLogic, userManager, bet);
    }

    public static bool AskPlayAgain()
    {
        while (true)
        {
            Console.Write("Хотите сыграть еще?(y/n):\t");
            string? input = Console.ReadLine()?.ToLower();

            switch (input)
            {
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    Console.WriteLine("пожалуйста введите y/n");
                    break;
            }
        }
    }

    public static string DetermineResult(Player player, Player dealer)
    {
        int playerValue = player.Hands[0].GetTotalValue();
        int dealerValue = dealer.Hands[0].GetTotalValue();

        bool playerBust = player.Hands[0].IsBust(playerValue);
        bool dealerBust = dealer.Hands[0].IsBust(dealerValue);

        if (playerBust)
            return "lose";
        if (dealerBust)
            return "win";
        if (playerValue > dealerValue)
            return "win";
        if (playerValue < dealerValue)
            return "lose";
        else
            return "draw";
    }

    public void ResultGame(User user, Player dealer, Player player, MoneyLogic moneyLogic, UserManager userManager, decimal bet)
    { 
        for (int i = 0; i < player.Hands.Count; i++)
        {
            Hand currentHand = player.Hands[i];
            int playerValue = currentHand.GetTotalValue();
            int dealerValue = dealer.Hands[0].GetTotalValue();

            bool playerBust = currentHand.IsBust(playerValue);
            bool dealerBust = dealer.Hands[0].IsBust(dealerValue);
            bool playerBj = currentHand.IsBlackJack(playerValue);

            string reason;
            string result;

            if (playerBj)
            {
                reason = "БлэкДжэк";
                result = "blackjack";
            }
            else if (playerBust)
            {
                reason = "Перебор у игрока";
                result = "lose";
            }
            else if (dealerBust)
            {
                reason = "У дилера перебор";
                result = "win";
            }
            else if (playerValue > dealerValue)
            {
                reason = "больше очков";
                result = "win";
            }
            else if (playerValue < dealerValue)
            {
                reason = "У дилера больше очков";
                result = "lose";
            }
            else
            {
                reason = "Ничья";
                result = "draw";
            }

            decimal beforBalance = user.Balance;
            moneyLogic.ResultBet(user, bet, result, userManager);
            decimal afterBalance = user.Balance;

            if (result == "win" || result == "blackjack")
            {
                Console.WriteLine(
                    $"Рука {i + 1} выиграла {reason}. Ваш выигрыш {afterBalance - beforBalance} руб. Ваш баланс {afterBalance} руб.");
                Console.WriteLine();
            }
            else if (result == "lose")
            {
                Console.WriteLine(
                    $"Рука {i + 1} проиграла {reason}. Ваш проигрыш {bet} руб. Ваш баланс {afterBalance} руб.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Рука {i + 1}: {reason}. Ваш баланс {afterBalance} руб.");
                Console.WriteLine();
            } 
        } 
    } 
}
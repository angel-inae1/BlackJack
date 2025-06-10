using BlackJack.BussinesLogic.Members;

namespace BlackJack.BussinesLogic.GameLogic;

public class CourseGame
{
    public void Hit(Player player, Deck deck, Player dealer)
    {
        while (true)
        { 
            Console.WriteLine("Карты игрока:");
            player.Hands[0].DisplayHands();

            int value = player.Hands[0].GetTotalValue();
            if (player.Hands[0].IsBust(value))
            {
                Console.WriteLine("Перебор очков. Вы проиграли.");
                break;
            }
            
            Console.WriteLine("Взять карту? y/n");
            string? input = Console.ReadLine()?.ToLower();

            if (input == "y")
            {
                var card = deck.DrawCard();
                player.Hands[0].AddCard(card);
                Console.WriteLine($"Взятая карта: {card}");
            }
            else if (input == "n")
            {
                Console.WriteLine("Набор карт в руку завершен.");
                var dealerLogic = new Dealer();
                dealerLogic.DealerTurn(player, dealer, deck);
                return;
            }
            else
            {
                Console.WriteLine("Выберите корректный ответ! y/n"); 
            }
        }
    }

    public static bool CheckForBlackJack(Player player, Player dealer)
    {
        bool playerBj = player.Hands[0].IsBlackJack(player.Hands[0].GetTotalValue());
        bool dealerBj = dealer.Hands[0].IsBlackJack(player.Hands[0].GetTotalValue());

        if (playerBj && dealerBj)
        {
            Console.WriteLine("У игрока и у дилера Блэк Джэк! Ничья!");
            return true;
        }
        else if (playerBj)
        {
            Console.WriteLine("У игрока Блэк Джэк! Вы выиграли.");
            return true;
        }
        else if (dealerBj)
        {
            Console.WriteLine("У дилера Блэк Джэк! Вы проишрали.");
            return true;
        }
        return false;
    }

    public void DoubleJack(Player player, Deck deck)
    {
        Console.WriteLine("Ставка удвоена! Вы получаете еще одну карту.");
        
        var card = deck.DrawCard();
        player.Hands[0].AddCard(card);
        Console.WriteLine($"Взятая карта: {card}");
        
        Console.WriteLine("Карты игрока:");
        player.Hands[0].DisplayHands();

        int value = player.Hands[0].GetTotalValue();
        Console.WriteLine($"Сумма очков игрока:{value}");
        
        if (player.Hands[0].IsBust(value))
        {
            Console.WriteLine("Перебор очков. Вы проиграли.");
        }
        
        
    }

    public void Split(Player player, Deck deck, Player dealer)
    {
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

        Console.WriteLine("Сплит. вы играете в две руки.");

        for (int i = 0; i < player.Hands.Count; i++)
        {
            Console.WriteLine("Рука {i + 1}:");
            while (true)
            {
                Console.WriteLine("Карты игрока:");
                player.Hands[i].DisplayHands();

                int value = player.Hands[i].GetTotalValue();
                if (player.Hands[i].IsBust(value))
                {
                    Console.WriteLine("Перебор очков. Рука проиграна.");
                    break;
                }

                Console.WriteLine("Взять карту? y/n");
                string? input = Console.ReadLine()?.ToLower();

                if (input == "y")
                {
                    var card = deck.DrawCard();
                    player.Hands[i].AddCard(card);
                    Console.WriteLine($"Взятая карта: {card}");
                }
                else if (input == "n")
                {
                    Console.WriteLine("Набор карт в руку завершен.");
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
    }

    public static bool CheckHand(Player player)
    {
        foreach (var hand in player.Hands)
        {
            int value = hand.GetTotalValue();
            if (!hand.IsBust(value))
            {
             return true;   
            }
        }
        return false;
    }
}
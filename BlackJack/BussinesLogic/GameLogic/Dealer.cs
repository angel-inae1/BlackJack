using BlackJack.BussinesLogic.Members;

namespace BlackJack.BussinesLogic.GameLogic;

public class Dealer
{
    public void DealerTurn(Player player, Player dealer, Deck deck)
    {
        if (!CourseGame.CheckHand(player))
        {
            Console.WriteLine("Вы приграли. Ставку забирает дилер.");
            return;
        }
        
        Console.WriteLine("Ход дилера. Открывается первая карта дилера.");
        dealer.Hands[0].DisplayHands();
        
        var dealerHand = dealer.Hands[0];
        int dealerPoints = dealerHand.GetTotalValue();

        while (dealerPoints < 17)
        {
            var card = deck.DrawCard();
            dealerHand.AddCard(card);
            Console.WriteLine($"Дилер берет карту: {card}");
            dealerHand.DisplayHands();
            dealerPoints = dealerHand.GetTotalValue();
        }
        Console.WriteLine($"Сумма очков дилера : {dealerPoints}");

        for (int i = 0; i < dealer.Hands.Count; i++)
        {
            var hand = player.Hands[i];
            int playerPoints = hand.GetTotalValue();
            Console.WriteLine($"Рука игрока {i+1}: {playerPoints} очков.");

            if (hand.IsBust(playerPoints))
            {
                Console.WriteLine($"В руке {i+1} перебор. Вы проиграли эту руку, ставку забирает дилер.");
            }
            else if (dealerPoints > 21)
            {
                Console.WriteLine($"У дилера перебор. Рука {i+1} выигрывает!");
            }
            else if (dealerPoints > playerPoints)
            {
                Console.WriteLine($"У дилера больше очков, Рука {i+1} проигрывает!");
            }
            else if (dealerPoints < playerPoints)
            {
                Console.WriteLine($"У вас больше очков, Рука {i+1} выигрывает!");
            }
            else if (dealerPoints == playerPoints)
            {
                Console.WriteLine("Ничья! Ставка возвращается.");
            }
        }
    }
}
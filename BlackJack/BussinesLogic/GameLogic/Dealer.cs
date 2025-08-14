using BlackJack.BussinesLogic.Base;
using BlackJack.BussinesLogic.Members;

namespace BlackJack.BussinesLogic.GameLogic;

public class Dealer
{
    public List<string> DealerTurn(Player player, Player dealer, Deck deck)
    {
        List<string> dealerTurn = new List<string>();

        foreach (var hand in player.Hands)
        {
            int playerPoints = hand.GetTotalValue();
            if (hand.IsBust(playerPoints))
            {
                dealerTurn.Add("lose");
            }
            else
            {
                dealerTurn.Add("");
            }
        }

        if (dealerTurn.All(r => r == "lose"))
        {
            return dealerTurn;
        }
        
        Console.WriteLine("Ход дилера. Открывается первая карта дилера.");
        dealer.Hands[0].DisplayHands();
        
        var dealerHand = dealer.Hands[0];
        int dealerPoints = dealerHand.GetTotalValue();
        Console.WriteLine();

        while (dealerPoints < 17)
        {
            var card = deck.DrawCard();
            dealerHand.AddCard(card);
            Console.WriteLine("Дилер берет карту");
            dealerHand.DisplayHands();
            dealerPoints = dealerHand.GetTotalValue();
            Console.WriteLine();
        }

        for (int i = 0; i < player.Hands.Count; i++)
        {
            if (dealerTurn[i]!="") continue;
            var hand = player.Hands[i];
            int playerPoints = hand.GetTotalValue();
            string result;

            if (hand.IsBust(playerPoints))
            {
                result = "lose";
            }
            else if (dealerPoints > 21)
            {
                result = "win";
            }
            else if (dealerPoints > playerPoints)
            {
                result = "lose";
            }
            else if (dealerPoints < playerPoints)
            {
               result = "win";
            }
            else if (dealerPoints == 21)
            {
                result = "lose";
            }
            else
            {
                result = "draw";
            }

            dealerTurn.Add(result);
        }

        return dealerTurn;
    }
}
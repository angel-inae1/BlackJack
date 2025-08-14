using BlackJack.BussinesLogic.Base;

namespace BlackJack.BussinesLogic.Members;

public class Hand
{
    private List<PlayingCard> _cards;
    public Hand()
    {
        _cards = new List<PlayingCard>();
    }

    public void AddCard(PlayingCard card)
    {
        _cards.Add(card);
    }

    public void AddCards(List<PlayingCard> newcards)
    {
        _cards.AddRange(newcards);
    }
    public IReadOnlyList<PlayingCard> Cards => _cards.AsReadOnly();

    public int GetTotalValue()
    {
        int total = 0;
        int aceCount = 0;

        foreach (var card in _cards)
        {
            int value = card.GetValue();
            total += value;
            
            if (card.FaceValue == FaceValueOfCard.Ace)
                aceCount++;
        }

        while ( total > 21 && aceCount > 0 )
        {
            total -= 10;
            aceCount--;
        }
        return total;
    }

    public bool IsBust(int value) => value > 21;
    public bool IsBlackJack(int value) => value == 21 && _cards.Count >= 2;

    public void DisplayHands(bool initial = false)
    {
        Console.Write("Карты:");

        for (int i = 0; i < _cards.Count; i++)
        {
            var card = _cards[i];
            if (initial&& i==0)
            {
                Console.Write("\t[Скрытая карта]");
            }
            else
            {
                Console.Write($"\t{card}");
            }
        }

        if (!initial)
        {
            Console.WriteLine();
            Console.WriteLine($"Сумма очков:{GetTotalValue()}");
        }
    }

    public bool CanSplit()
    {
        return _cards.Count == 2 && _cards[0].FaceValue == _cards[1].FaceValue;
    }
}
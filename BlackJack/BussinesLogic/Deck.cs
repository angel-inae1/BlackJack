namespace BlackJack.BussinesLogic;

public class Deck
{
    public List<PlayingCard> Cards { get; }
    public Random random = new Random();
    
    public Deck()
    {
        Cards = new List<PlayingCard>();
        foreach (SuitOfCard suit in Enum.GetValues<SuitOfCard>())
        {
            foreach (FaceValueOfCard face in Enum.GetValues<FaceValueOfCard>())
            {
                Cards.Add(new PlayingCard(suit, face));
            }
        }
    }

    public PlayingCard DrawCard()
    {
        int index = random.Next(Cards.Count);
        PlayingCard card = Cards[index];
        Cards.RemoveAt(index);
        return card;
    }
}
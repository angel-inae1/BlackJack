namespace BlackJack.BussinesLogic;

public class PlayingCard
{
    public SuitOfCard Suit { get; }
    public FaceValueOfCard FaceValue { get; }

    public PlayingCard(SuitOfCard suit, FaceValueOfCard faceValue)
    {
        Suit = suit;
        FaceValue = faceValue;
    }

    public int GetValue()
    {
        switch (FaceValue)
        {
            case FaceValueOfCard.Two: return 2;
            case FaceValueOfCard.Three: return 3;
            case FaceValueOfCard.Four: return 4;
            case FaceValueOfCard.Five: return 5;
            case FaceValueOfCard.Six: return 6;
            case FaceValueOfCard.Seven: return 7;
            case FaceValueOfCard.Eight: return 8;
            case FaceValueOfCard.Nine: return 9;
            case FaceValueOfCard.Ten: return 10;
            case FaceValueOfCard.Jack: return 10;
            case FaceValueOfCard.Queen: return 10;
            case FaceValueOfCard.King: return 10;
            case FaceValueOfCard.Ace: return 11;
            default:
                throw new ArgumentOutOfRangeException(nameof(FaceValue),$"Неизвестное значение.");
        }
    }
    public override string ToString()
    {
        return $"{Suit} {FaceValue}";
    }

}
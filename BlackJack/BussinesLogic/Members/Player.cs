namespace BlackJack.BussinesLogic;

public class Player
{
    public List<Hand> Hands { get; set; } = new();
    public Player()
    {
        Hands.Add(new Hand());
    }
}
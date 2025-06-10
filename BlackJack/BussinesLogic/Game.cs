using BlackJack.BussinesLogic.GameLogic;
using BlackJack.BussinesLogic.Members;

namespace BlackJack.BussinesLogic;

public class Game
{
    private Deck _deck;
    private Player _player;
    private Player _dealer;

    public Game()
    {
        _deck = new Deck();
        _player = new Player();
        _dealer = new Player();
    }

    public void StartGame()
    {
        _player.Hands[0].AddCards(new List<PlayingCard>{_deck.DrawCard(), _deck.DrawCard()});
        _dealer.Hands[0].AddCards(new List<PlayingCard>{_deck.DrawCard(), _deck.DrawCard()});
        Console.WriteLine("Игра началась. У игрока и дилера по две карты.");

       Console.WriteLine("Игрок:");
       _player.Hands[0].DisplayHands();
       Console.WriteLine("Дилер:");
       _dealer.Hands[0].DisplayHands(true);

       if (CourseGame.CheckForBlackJack(_player, _dealer))
       {
           return;
       }

       while (true)
       {
           Console.WriteLine("Игрок выбирает следующий ход: Хит(h), Стэнд(s), Дабл(d), Сплит(2), Сдаться(0)");
           string? input = Console.ReadLine()?.ToLower();
           switch (input)
           {
               case "h":
                   CourseGame courseGame = new CourseGame();
                   courseGame.Hit(_player, _deck, _dealer);
                   break;
               case "s":
                   Console.WriteLine("Ваши карты набраны, ход дилера.");
                   var dealerLogic1 = new Dealer();
                   dealerLogic1.DealerTurn(_player, _dealer, _deck);
                   return;
               case "d":
                   CourseGame doubleGame = new CourseGame();
                   doubleGame.DoubleJack(_player, _deck);
                   var dealerLogic2 = new Dealer();
                   dealerLogic2.DealerTurn(_player, _dealer, _deck);
                   break;
               case "2":
                   if (_player.Hands[0].CanSplit())
                   {
                       CourseGame splitGame = new CourseGame();
                       splitGame.Split(_player, _deck, _dealer);
                   }
                   else
                   {
                       Console.WriteLine("Сплит невозможен.");
                   }

                   continue;
               case "0":
                   Console.WriteLine("Вы сдались. Вам возвращается половина ставки.");
                   return;
               default:
                   Console.WriteLine("Выберите действие: h/s/d/2/0");
                   break;
           }
       }
    }
}
using BlackJack.BussinesLogic.Base;
using BlackJack.BussinesLogic.BlackJack.Models;
using BlackJack.BussinesLogic.GameLogic;
using BlackJack.BussinesLogic.Members;
using BlackJack.BussinesLogic.Money;

namespace BlackJack.BussinesLogic;

public class Game
{
    private Deck _deck;
    private Player _player;
    private Player _dealer;
    private MoneyLogic _moneyLogic;
    private User _user;
    private UserManager _userManager;

    public Game(User user, UserManager userManager)
    {
        _deck = new Deck();
        _player = new Player();
        _dealer = new Player();
        _moneyLogic = new MoneyLogic();

        _userManager = userManager;
        _user = user;
    }

    public void StartGame()
    {
        Console.WriteLine("Добро пожаловать в игру!");
        Console.WriteLine();
        do
        {
            if (_user.Balance <= 0)
            {
                Console.WriteLine("У вас закончились деньги!");
                Console.WriteLine();
                _moneyLogic.TopUpBalance(_user, _userManager);
            }

            decimal bet = _moneyLogic.InitialBet(_user);
            _user.Balance -= bet;
            _userManager.SaveUsers();

            _deck = new Deck();
            _player = new Player();
            _dealer = new Player();

            _player.Hands[0].AddCards(new List<PlayingCard> { _deck.DrawCard(), _deck.DrawCard() });
            _dealer.Hands[0].AddCards(new List<PlayingCard> { _deck.DrawCard(), _deck.DrawCard() });
            Console.WriteLine("Игра началась. У игрока и дилера по две карты.");
            Console.WriteLine();

            Console.Write("Игрок:");
            _player.Hands[0].DisplayHands();
            Console.WriteLine();
            Console.Write("Дилер:");
            _dealer.Hands[0].DisplayHands(true);
            Console.WriteLine(".");

            string? blackjackResult = CourseGame.CheckForBlackJack(_player, _dealer);
            if (blackjackResult != null)
            {
                _moneyLogic.ResultBet(_user, bet, blackjackResult, _userManager);
                if (!CourseGame.AskPlayAgain())
                    break;
                else
                    continue;
            }

            while (true)
            {
                Console.Write("Игрок выбирает следующий ход: Хит(h), Стэнд(s), Дабл(d), Сплит(2), Сдаться(0):\t");
                string? input = Console.ReadLine()?.ToLower();
                Console.WriteLine();
                CourseGame courseGame = new CourseGame();
                switch (input)
                {
                    case "h":
                        courseGame.Hit(_player, _deck, _dealer);
                        var dealerLogic1 = new Dealer();
                        dealerLogic1.DealerTurn(_player, _dealer, _deck);
                        string result = CourseGame.DetermineResult(_player, _dealer);
                        _moneyLogic.ResultBet(_user, bet, result, _userManager);
                        courseGame.ResultGame(_user, _dealer, _player, _moneyLogic, _userManager, bet);

                        break;

                    case "s":
                        Console.WriteLine("Ваши карты набраны, ход дилера.");
                        Console.WriteLine();
                        var dealerLogic2 = new Dealer();
                        dealerLogic2.DealerTurn(_player, _dealer, _deck);
                        string result1 = CourseGame.DetermineResult(_player, _dealer);
                        _moneyLogic.ResultBet(_user, bet, result1, _userManager);
                        courseGame.ResultGame(_user, _dealer, _player, _moneyLogic, _userManager, bet);
                        break;

                    case "d":
                        CourseGame doubleGame = new CourseGame();
                        doubleGame.DoubleJack(_player, _deck);
                        var dealerLogic3 = new Dealer();
                        dealerLogic3.DealerTurn(_player, _dealer, _deck);
                        string result2 = CourseGame.DetermineResult(_player, _dealer);
                        _moneyLogic.ResultBet(_user, bet, result2, _userManager);
                        courseGame.ResultGame(_user, _dealer, _player, _moneyLogic, _userManager, bet);
                        break;

                    case "2":
                        if (_player.Hands[0].CanSplit()&& _user.Balance >= bet)
                        {
                            CourseGame splitGame = new CourseGame();
                            splitGame.Split(_player, _deck, _dealer, bet, _moneyLogic, _user, _userManager);
                        }
                        else
                        {
                            Console.WriteLine("Сплит невозможен.");
                            Console.WriteLine();
                            continue;
                        }
                        break;

                    case "0":
                        Console.WriteLine("Вы сдались. Вам возвращается половина ставки.");
                        Console.WriteLine();
                        _user.Balance += bet / 2;
                        _userManager.SaveUsers();
                        break;
                    default:
                        Console.WriteLine("Выберите действие: h/s/d/2/0");
                        continue;
                }

                break;
            }
        } while (CourseGame.AskPlayAgain());

        Console.WriteLine("Спасибо за игру!");
    }
}
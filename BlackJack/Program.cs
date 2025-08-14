using BlackJack.BussinesLogic;
using BlackJack.BussinesLogic.BlackJack.Models;

namespace BlackJack
{
    class Program
    {
        static void Main()
        {
            var userManager = new UserManager();
            Authentication auth = new Authentication(userManager);
            Console.WriteLine("Здравствуйте, игрок! Войдите в систему.");
            Console.WriteLine();
            
            User currentUser = auth.Authenticate();
            Console.WriteLine($"Привет, {currentUser.Login}! Ваш баланс {currentUser.Balance} руб ");
            Console.WriteLine();
            
            Game game = new Game(currentUser, userManager);
            game.StartGame();
        }
    }
}



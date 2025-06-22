using BlackJack.BussinesLogic;
using BlackJack.BussinesLogic.User;

namespace BlackJack
{
    class Program
    {
        static void Main()
        {
            var userManager = new UserManager();
            var auth = new Authentication(userManager);
            Console.WriteLine("Здравствуйте, игрок! Войдите в систему.");
            
            User currentUser = auth.Authenticate();
            Console.WriteLine($"Привет, {currentUser.Login}! Ваш баланс {currentUser.Balance}$");
            
            var game = new Game();
            game.StartGame();
        }
    }
}



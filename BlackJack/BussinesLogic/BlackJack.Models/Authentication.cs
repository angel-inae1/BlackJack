namespace BlackJack.BussinesLogic.BlackJack.Models;

public class Authentication
{
    private UserManager _userManager;

    public Authentication(UserManager userManager)
    {
        this._userManager = userManager;
    }

    public User Authenticate()
    {
        while (true)
        {
            Console.Write("Введите ваш логин:\t");
            string? login = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(login))
            {
                Console.WriteLine("Логин не может быть пустым!");
                continue;
            }

            if (_userManager.IsUserRegistered(login))
            {
                User? user = null;

                while (user == null)
                {
                    Console.Write("Пользователь найден. Введите пароль:\t");
                    string? password = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(password))
                    {
                        Console.WriteLine("Пароль не может быть пустым.");
                        continue;
                    }

                    user = _userManager.Login(login, password);

                    if (user == null)
                    {
                        Console.WriteLine("Неверный пароль! Попробуйте снова!");
                    }
                }
                return user;
            }
            else
            {
                Console.WriteLine("Пользователь не найден! зарегестрироваться? (y/n)\t");
                string? answer = Console.ReadLine();
                
                if (answer?.ToLower() == "y")
                {
                    Console.Write("Введите новый пароль:\t");
                    string? password = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        Console.WriteLine("Пароль не может быть пустым.");
                        continue;
                    }

                    if (_userManager.RegisterUser(login, password))
                    {
                        Console.WriteLine("Регистрация прошла успешно! Ваш начальный баланс 1000 руб. Войдите в систему с новым логином и паролем!");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Логин уже занят! Попробуйте зарегестрироваться снова.");
                    }
                }
            }
        }
    }
}
namespace BlackJack.BussinesLogic.User;

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
            Console.WriteLine("Введите ваш логин:");
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
                    Console.WriteLine("Пользователь найден. Введите пароль:");
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

                Console.WriteLine($"Успешный вход! Баланс: {user.Balance}");
                return user;
            }
            else
            {
                Console.WriteLine("Пользователь не найде! зарегестрироваться? (y/n)");
                string? answer = Console.ReadLine();
                
                if (answer?.ToLower() == "y")
                {
                    Console.WriteLine("Введите новый пароль:");
                    string? password = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        Console.WriteLine("Пароль не может быть пустым.");
                        continue;
                    }

                    if (_userManager.RegisterUser(login, password))
                    {
                        Console.WriteLine("Регистрация прошла успешно! Ваш начальный баланс 1000$. Войдите в систему с новым логином и паролем!");
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
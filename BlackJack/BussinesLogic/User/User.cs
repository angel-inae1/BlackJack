
namespace BlackJack.BussinesLogic.User;

public class User
{
    public string? Login { get; set; }
    public string? Password { get; set; }
    public decimal Balance { get; set; }
    
    public User() { }

    public User(string login, string password, decimal balance)
    {
        Login = login;
        Password = password;
        Balance = balance;
    }
    
    
}
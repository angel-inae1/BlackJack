using System.Text.Json;

namespace BlackJack.BussinesLogic.User;

public class UserManager
{
    private const decimal InitialBalance = 1000m;
    public List<User> Users { get; set; } = new List<User>();
    
    private readonly string _filePath = "users.json";

    public UserManager()
    {
        LoadUsers();
    }

    public bool IsUserRegistered(string login)
    {
        return Users.Any(u => u.Login == login);
    }

    public bool RegisterUser(string login, string password)
    {
        if (IsUserRegistered(login))
            return false;
        Users.Add(new User(login, password, InitialBalance));
        SaveUsers();
        return true;
    }

    public User? Login(string login, string password)
    {
        return Users.FirstOrDefault(u => u.Login == login && u.Password == password);
    }

    public void SaveUsers()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(Users, options);
        File.WriteAllText(_filePath, json);
    }

    public void LoadUsers()
    {
        string json = File.ReadAllText(_filePath);
        var loadedUsers = JsonSerializer.Deserialize<List<User>>(json);
        if (loadedUsers != null)
        {
            Users = loadedUsers;
        }
    }
}
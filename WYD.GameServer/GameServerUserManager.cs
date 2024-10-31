namespace WYD.GameServer;

public class GameServerUserManager
{
    private readonly IDictionary<int, GameServerUser> _users = new Dictionary<int, GameServerUser>();
    
    public bool AddUser(GameServerUser user) => _users.TryAdd(user.Id, user);

    public bool RemoveUser(int id) => _users.Remove(id);
    
    public GameServerUser? GetUser(int id)
    {
        _users.TryGetValue(id, out var user);
        return user;
    }
}
using WYD.Network;

namespace WYD.GameServer;

public class GameServerUser
{
    public GameServerUser(ISession session)
    {
        Session = session;
    }
    
    public ISession Session { get; }
    public int Id => Session.Id;
}
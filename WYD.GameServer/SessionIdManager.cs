namespace WYD.GameServer;

public class SessionIdManager(int maxSessions)
{
    private readonly Queue<int> _availableIds = [];
    private readonly HashSet<int> _usedIds = [];
    private int _currentId = 1;
    
    public bool HasAvailableSessionId => _availableIds.Count > 0 || _usedIds.Count < maxSessions;
    
    public int GetNextId()
    {
        if (!HasAvailableSessionId)
        {
            throw new InvalidOperationException("No available session IDs");
        }
        
        if (_availableIds.TryDequeue(out var id))
        {
            _usedIds.Add(id);
            return id;
        }

        id = _currentId++;
        _usedIds.Add(id);
        
        return id;
    }

    public void Release(int id)
    {
        if (_usedIds.Remove(id))
        {
            _availableIds.Enqueue(id);
        }
    }
}
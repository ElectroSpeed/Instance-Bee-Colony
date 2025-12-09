using System.Collections.Generic;

public class Blackboard
{
    private Dictionary<string, object> _blackboard = new Dictionary<string, object>();

    public void AddValue(string key, object value)
    {
        if (!_blackboard.ContainsKey(key))
        {
            _blackboard.Add(key, value);
        }
    }

    public object GetValue(string key)
    {
        if (_blackboard.ContainsKey(key))
        {
            return _blackboard[key];
        }
        return null;
    }

    public void ModifyValue(string key, object newValue)
    {
        if (_blackboard.ContainsKey(key))
        {
            _blackboard[key] = newValue;
        }
    }

    public string GetKey(object value)
    {
        foreach (var keyValue in _blackboard)
        {
            if (keyValue.Value.Equals(value))
            {
                return keyValue.Key;
            }
        }
        return null;
    }
}
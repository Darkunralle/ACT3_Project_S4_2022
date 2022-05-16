using Unity;
using UnityEngine;

public class Event : ScriptableObject
{
    public delegate void TriggeredDelegate(Vector3 p_position);

    public event TriggeredDelegate onTriggered;
    public void Raise(Vector3 p_pos)
    {
        onTriggered?.Invoke(p_pos);
    }
}

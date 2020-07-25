using UnityEngine;

/// <summary>
/// Scriptable object class to speicfy the protocol of a bomb hit.
/// </summary>
[CreateAssetMenu(fileName = "New BombHitData", menuName = "Bomb Hit Data", order = 51)]
public class BombHitData : ScriptableObject
{
    public string type;
    public int damage;
}
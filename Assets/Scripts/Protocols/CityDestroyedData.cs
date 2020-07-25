using UnityEngine;

/// <summary>
/// Scriptable object class to speicfy the protocol of a 'city destroyed' message.
/// </summary>
[CreateAssetMenu(fileName = "New CityDestroyedData", menuName = "CityDestroyed Data", order = 51)]
public class CityDestroyedData : ScriptableObject
{
    public string id;
}
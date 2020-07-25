using UnityEngine;

/// <summary>
/// Scriptable object class to speicfy the protocol of an award given to the user.
/// </summary>
[CreateAssetMenu(fileName = "New AwardData", menuName = "Award Data", order = 51)]
public class AwardData : ScriptableObject
{
    public string type;
    public string subType;
    public int award;
}
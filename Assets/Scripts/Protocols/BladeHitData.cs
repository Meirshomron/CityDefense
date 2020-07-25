using UnityEngine;

/// <summary>
/// Scriptable object class to speicfy the protocol of the data of the hit  by the blade.
/// </summary>
public class BladeHitData : ScriptableObject
{
    public Vector3 bladeDir;
    public Vector3 bladePos;
    public int hitID;
}
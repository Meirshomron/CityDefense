using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Booster : MonoBehaviour 
{

    [SerializeField] private AwardData awardData;
    [SerializeField] private Vector3 direction;
    private float startForce;
    private Rigidbody2D rb;

    private UnityAction<string> EventListener;

    public void Start()
    {
        startForce = 1;

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * startForce, ForceMode2D.Impulse);

        EventListener = new UnityAction<string>(OnBoosterHit);
        EventManager.StartListening("BLADE_HIT_DATA_EVENT", EventListener);
    }

    /// <summary>
    /// Callback called once the booster is hit by the blade.
    /// </summary>
    /// <param name="jsonVars"> Blade hit data.</param>
    void OnBoosterHit(string jsonVars)
    {
        //print("Booster: OnBoosterHit");

        BladeHitData objVars = ScriptableObject.CreateInstance<BladeHitData>();
        JsonUtility.FromJsonOverwrite(jsonVars, objVars);

        // Only award for the booster hit by the blade.
        if (objVars.hitID == gameObject.GetInstanceID())
        {
            string jsonAwardVars = JsonUtility.ToJson(awardData);
            EventManager.TriggerEvent("AWARD_DATA_EVENT", jsonAwardVars);

            ObjectPooler.Instance.ReturnToPool(gameObject);
        }
    }
}

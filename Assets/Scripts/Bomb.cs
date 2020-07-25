using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bomb : MonoBehaviour 
{

    [SerializeField] private GameObject bombSlicedPrefab;
    [SerializeField] private BombHitData bombHitData;
    [SerializeField] private AwardData awardData;
    [SerializeField] private int startLife;
    [SerializeField] private Vector3 direction;

    [SerializeField] private float startForce;
    private int currentLife;
    private Rigidbody2D rb;

    private UnityAction<string> EventListener;

    /// <summary>
    /// OnEnable is the initial function because this bomb is activated from the ObjectPooled by the BombSpawner that activates this bomb.
    /// </summary>
    public void OnEnable()
    {
        //print("Bomb: OnEnable");
        
        // Set the startforce in relation to the screen width so the farther it has to fly - the more force.
        float baseForce = 2f;
        startForce = (Screen.width / 250.0f) + baseForce;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * startForce, ForceMode2D.Impulse);

        currentLife = startLife;

        EventListener = new UnityAction<string>(OnBombGotHit);
        EventManager.StartListening("BLADE_HIT_DATA_EVENT", EventListener);
    }

    public void OnDisable()
    {
        EventManager.StopListening("BLADE_HIT_DATA_EVENT", EventListener);
    }

    /// <summary>
    /// Callback called once the bomb is hit by the blade.
    /// </summary>
    /// <param name="jsonVars"> Blade hit data. </param>
    void OnBombGotHit(string jsonVars)
    {
        BladeHitData objVars = ScriptableObject.CreateInstance<BladeHitData>();
        JsonUtility.FromJsonOverwrite(jsonVars, objVars);

        if (objVars.hitID == gameObject.GetInstanceID())
        {
            //print("Bomb: OnBombGotHit");

            currentLife--;

            // This bomb has no my life - destroy it and award the player for it.
            if (currentLife == 0)
            {
                // Create the sliced bomb according the blade's hit direction.
                Quaternion rotation = Quaternion.LookRotation(objVars.bladeDir);
                GameObject slicedBomb = Instantiate(bombSlicedPrefab, transform.position, rotation);

                string jsonAwardVars = JsonUtility.ToJson(awardData);
                EventManager.TriggerEvent("AWARD_DATA_EVENT", jsonAwardVars);

                ObjectPooler.Instance.ReturnToPool(gameObject);

                UserModel.Instance.BombsSliced++;
                
                Destroy(slicedBomb, 3f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Hit the target so post an event for the health of the target is updated, and play 'boom' animation.
        if (col.tag == "City")
        {
            //print("Bomb:OnTriggerEnter2D hit City");

            string jsonVars = JsonUtility.ToJson(bombHitData);

            // Passing the json string as a parameter.
            EventManager.TriggerEvent("BOMB_HIT_DATA_EVENT", jsonVars);

            ObjectPooler.Instance.ReturnToPool(gameObject);
        }
    }

}

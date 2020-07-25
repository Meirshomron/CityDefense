using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public GameObject bladeTrailPrefab;
    private GameObject currentBladeTRail;
    public float minCutVelocity = 0.001f;
    private bool isCutting = false;

    private Vector2 prevPosition;
    private Vector3 direction;
    private Rigidbody2D rb;
    private Camera cam;
    private CircleCollider2D circleCollider;

    private BladeHitData bladeHitData;

    public void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void Update()
    {
        // Mouse down - start cutting.
        // Mouse up - stop cutting.
        if (Input.GetMouseButtonDown(0))
        {
            StartCutting();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }
        if (isCutting)
        {
            UpdateCut();
        }
    }

    /// <summary>
    /// Update the direction and velocity of the cutting.
    /// </summary>
    void UpdateCut()
    {
        Vector2 newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        rb.position = newPosition;

        float velocity = (newPosition - prevPosition).magnitude / Time.deltaTime;
        direction = newPosition - prevPosition;
        if (velocity > minCutVelocity)
        {
            circleCollider.enabled = true;
        }
        else
        {
            circleCollider.enabled = false;
        }
        prevPosition = newPosition;
    }

    void StartCutting()
    {
        isCutting = true;
        currentBladeTRail = Instantiate(bladeTrailPrefab, rb.transform, false);
        prevPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        circleCollider.enabled = false;
    }

    void StopCutting()
    {
        isCutting = false;
        currentBladeTRail.transform.SetParent(null);
        Destroy(currentBladeTRail, 2f);
        circleCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Unique id of the object hit.
        int hitID = col.gameObject.GetInstanceID();

        // Create the scriptable data object of a hit.
        bladeHitData = ScriptableObject.CreateInstance<BladeHitData>();
        bladeHitData.bladeDir = direction;
        bladeHitData.bladePos = transform.position;
        bladeHitData.hitID = hitID;

        string jsonVars = JsonUtility.ToJson(bladeHitData);
        EventManager.TriggerEvent("BLADE_HIT_DATA_EVENT", jsonVars);
    }
}

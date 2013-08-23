using UnityEngine;
using System.Collections.Generic;

public class BikeAI : MonoBehaviour
{
    public List<GameObject> allTargets;
    private int curTarget;

    public float rotSpeed = 20.0f;
    public float forwardSpeed = 22.0f;
    public float maxSpeed = 25.0f;
    public float steerAngle = 10.0f;
    public float deccelSpeed = 40.0f;

    public Transform backTireTrans;
    public Transform bikeBody;

    public bool hasCrashed = false;

    public float accelFactor = 0.0f;
    private float curSpeed = 0.0f;

    public int steerDir = 1;

    private Quaternion initRot;

    private Vector3 moveDir;
    private Vector3 rotDir;

    private CheckPoints checkPoints;
    private DrewBackTire backTire;

    void Awake()
    {
        backTire = backTireTrans.GetComponent<DrewBackTire>();
        checkPoints = GetComponent<CheckPoints>();
    }

	// Use this for initialization
	void Start ()
    {
        allTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Waypoint"));

        allTargets.Sort(delegate(GameObject a1, GameObject a2) { return a1.name.CompareTo(a2.name); });

        curSpeed = maxSpeed;
        initRot = bikeBody.rotation;
	}
	
	// Update is called once per frame
	void Update ()
    {
        AIPhysics();
	}

    void AIPhysics()
    {
        if (hasCrashed)
        {
            accelFactor = 0;

            if (!IsInvoking("Respawn"))
            {
                Invoke("Respawn", 0);
            }
        }
        else
        {
            accelFactor = Mathf.MoveTowards(accelFactor, curSpeed, forwardSpeed * Time.deltaTime);
        }
                
        if (curTarget <= allTargets.Count - 1)
        {
            Vector3 dir = allTargets[curTarget].transform.position - transform.position;
            float dist = Vector3.Distance(allTargets[curTarget].transform.position, transform.position);

            if (dist < 5)
            {
                curTarget++;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        }

        else
        {
            curTarget = 0;
            //accelFactor = Mathf.MoveTowards(accelFactor, 0, deccelSpeed * Time.deltaTime);
        }
        
        moveDir = new Vector3(0, rigidbody.velocity.y, accelFactor);
        if (!LevelScripts.isGreen)
        {
            transform.Translate(moveDir * Time.deltaTime);
        }
    }

    void Respawn()
    {
        bikeBody.localRotation = initRot;
        transform.rotation = checkPoints.currentCheckpoint.rotation;
        transform.position = checkPoints.currentCheckpoint.position;

        hasCrashed = false;
    }

    //limit the angle
    float ClampAngle(float angle, float limit)
    {
        if (angle > 180)
        {
            float angleB = 360 - angle;

            if (angleB > limit)
            {
                angle = 360 - limit;
            }
            else
            {
                return angle;
            }
        }
        if (angle < 180)
        {
            if (angle > limit && angle < 360 - limit)
            {
                angle = limit;
            }
            else
            {
                return angle;
            }
        }

        return angle;
    }
}

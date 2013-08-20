﻿using UnityEngine;
using System.Collections.Generic;

public class BikeAI : MonoBehaviour
{
    public float rampLimit = 5.0f;
    public float rampMag = 0.4f;

    public List<GameObject> allTargets;
    public GameObject finishTarget;
    public int curTarget;

    public Transform backTireTrans;
    public Transform bikeBody;

    public bool hasCrashed = false;

    private Quaternion initRot;

    private CheckPoints checkPoints;
    private DrewBackTire backTire;

    private NavMeshAgent agent;

    void Awake()
    {
        backTire = backTireTrans.GetComponent<DrewBackTire>();
        checkPoints = GetComponent<CheckPoints>();
    }

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.autoBraking = false;

        allTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Checkpoint"));
        allTargets.Add(finishTarget);

        allTargets.Sort(delegate(GameObject a1, GameObject a2) { return a1.name.CompareTo(a2.name); });

        agent.destination = allTargets[curTarget].transform.position;

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
            //isResetting = true;

            //accelFactor = 0;
            bikeBody.localRotation = initRot;
            transform.rotation = checkPoints.currentCheckpoint.rotation;
            transform.position = checkPoints.currentCheckpoint.position;

            hasCrashed = false;
        }

        if (curTarget < allTargets.Count - 1)
        {
            float dist = Vector3.Distance(allTargets[curTarget].transform.position, transform.position);

            if (dist < 20)
            {
                curTarget++;
                agent.destination = allTargets[curTarget].transform.position;
            }
        }

        if (curTarget == allTargets.Count)
        {
            agent.autoBraking = true;
        }

        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 3) && backTire.isGrounded)
        {
            Vector3 surfaceNormal = hitInfo.normal;
            surfaceNormal.Normalize();

            transform.rotation = Quaternion.FromToRotation(transform.up, surfaceNormal) * transform.rotation;
        }
    }
}

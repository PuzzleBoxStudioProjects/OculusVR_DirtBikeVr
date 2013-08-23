using UnityEngine;
using System.Collections.Generic;

public class CamLogic : MonoBehaviour
{
    private GameObject[] allCrowds;
    private GameObject crowd;

    public Transform bike;

    private DrewBikePhysics bikePhysics;

	// Use this for initialization
	void Start ()
    {
        bikePhysics = bike.GetComponent<DrewBikePhysics>();
        allCrowds = GameObject.FindGameObjectsWithTag("Crowd");
	}
	
	// Update is called once per frame
	void Update ()
    {
        TurboControl();
	}

    void TurboControl()
    {
        crowd = allCrowds[0];
        float dist = Vector3.Distance(transform.position, crowd.transform.position);

        for (int i = 0; i < allCrowds.Length; i++)
        {
            //get direction to current crowd
            Vector3 dirToCrowd = allCrowds[i].transform.position - transform.position;
            //closest distance to crowd
            float crowdDist = Vector3.Distance(allCrowds[i].transform.position, transform.position);

            //the dot product
            float dotProduct = Vector3.Dot(transform.forward, dirToCrowd.normalized);

            if (crowdDist < dist)
            {
                //looking in general direction
                if (dotProduct > 0.9f)
                {
                    //set current stand we are looking at
                    crowd = allCrowds[i];

                    //if (Time.timeScale == 1.0f)
                    //{
                    //    //slow time
                    //    Time.timeScale = 0.4f;
                    //}
                    //Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    //apply turbo
                    if (bikePhysics.turboBar < bikePhysics.maxTurboBar)
                    {
                        bikePhysics.turboBar += bikePhysics.turboBoostSpeed;
                    }
                }
                Vector3 oldCrowdPos = crowd.transform.position - transform.position;

                //looking away from stands
                if (Vector3.Dot(transform.forward, oldCrowdPos.normalized) < 0.2f)
                {
                    //if (Time.timeScale != 1.0f)
                    //{
                    //    Time.timeScale = 1.0f;
                    //}
                    //Time.fixedDeltaTime = 0.02f * Time.timeScale;
                }
            }
        }
    }
}

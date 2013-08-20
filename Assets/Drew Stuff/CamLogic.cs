using UnityEngine;
using System.Collections.Generic;

public class CamLogic : MonoBehaviour
{
    public GameObject[] allCrowds;
    public GameObject crowd;

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
        for (int i = 0; i < allCrowds.Length; i++)
        {
            //get direction to current crowd
            Vector3 dirToCrowd = allCrowds[i].transform.position - transform.position;
            //get forward direction
            Vector3 forwardDir = transform.TransformDirection(Vector3.forward);

            //the dot product
            float dotProduct = Vector3.Dot(transform.forward, dirToCrowd.normalized);
            
            //looking in general direction
            if (dotProduct > 0.9f)
            {
                //set current stand we are looking at
                crowd = allCrowds[i];
                //apply turbo
                if (bikePhysics.turboBar < bikePhysics.maxTurboBar)
                {
                    bikePhysics.turboBar += bikePhysics.turboBoostSpeed * Time.deltaTime;
                }
            }
        }
    }
}

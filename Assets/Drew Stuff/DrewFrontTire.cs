using UnityEngine;
using System.Collections;

public class DrewFrontTire : MonoBehaviour
{
    public Transform bike;
    public Transform backTireTrans;

    private DrewBackTire backTire;
    private DrewBikePhysics bikePhysics;

	void Awake ()
    {
        bikePhysics = bike.GetComponent<DrewBikePhysics>();
        backTire = backTireTrans.GetComponent<DrewBackTire>();
	}

    void Update()
    {
        CrashGroundCheck();
    }

    void CrashGroundCheck()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1) && !backTire.isGrounded)
        {
            bikePhysics.hasCrashed = true;
        }
    }
}

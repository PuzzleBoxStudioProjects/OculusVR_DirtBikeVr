using UnityEngine;
using System.Collections;

public class DrewBackTire : MonoBehaviour
{
    //[HideInInspector]
    public bool isGrounded = false;
    
    void LateUpdate()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 1))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}

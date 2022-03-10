using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    public Transform[] satellites;
    
    const float gravity = 9.81f;

    void FixedUpdate() {
        for (int i=0; i<satellites.Length; ++i)
        {
            Rigidbody otherRigidbody = satellites[i].gameObject.GetComponent<Rigidbody>();
            if (otherRigidbody) {
                Vector3 difference = this.gameObject.transform.position - satellites[i].gameObject.transform.position;

                float dist = difference.magnitude;
                Vector3 gravityDirection = difference.normalized;
                Vector3 gravityVector = (gravityDirection * gravity) * (GetComponent<Rigidbody>().mass * otherRigidbody.mass) / (dist * dist) ;

                otherRigidbody.AddForce(gravityVector, ForceMode.Acceleration);
            }
        }
    }
}

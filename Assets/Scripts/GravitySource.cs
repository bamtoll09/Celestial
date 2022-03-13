using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    public Transform[] satellites;
    
    const float G = 6.67f;

    void FixedUpdate() {
        for (int i=0; i<satellites.Length; ++i)
        {
            Rigidbody otherRigidbody = satellites[i].gameObject.GetComponent<Rigidbody>();
            if (otherRigidbody) {
                Vector3 difference = this.gameObject.transform.position - satellites[i].gameObject.transform.position;

                float dist = difference.magnitude;
                Vector3 gravityDirection = difference.normalized;
                Vector3 gravityVector = (gravityDirection * G * 0.001f) * (GetComponent<Rigidbody>().mass * otherRigidbody.mass) / (dist * dist) ;

                otherRigidbody.AddForce(gravityVector, ForceMode.Acceleration);
            }
        }
    }
}

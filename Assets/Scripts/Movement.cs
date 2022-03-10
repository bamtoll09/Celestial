using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        if (vel.magnitude == 0f)
        {
            vel = new Vector3(0f, 0f, 10f);
        }

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = vel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

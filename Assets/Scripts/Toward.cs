using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toward : MonoBehaviour
{
    public Transform traget;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 toward = traget.position - this.transform.position;
        
        transform.LookAt(toward);
        transform.Rotate(new Vector3(90f, 0f, 0f));
        transform.Rotate(new Vector3(0f, -15f, 0f), Space.Self);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 toward = traget.position - this.transform.position;
        
        transform.LookAt(toward);
        transform.Rotate(new Vector3(90f, 0f, 0f));
        transform.Rotate(new Vector3(0f, -15f, 0f), Space.Self);
    }
}

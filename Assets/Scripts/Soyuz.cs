using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soyuz : MonoBehaviour
{
    public GameObject canvasRotation;
    public GameObject line;
    LineRenderer renderer;
    int count;
    float eTime;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = Instantiate(line, Vector3.zero, Quaternion.identity) as GameObject;
        renderer = obj.GetComponent<LineRenderer>();
        renderer.positionCount = 200;
    }

    // Update is called once per frame
    void Update()
    {
        canvasRotation.transform.LookAt(Camera.main.transform);

        if (count < 200)
        {
            if (eTime >= 0.05f)
            {
                renderer.SetPosition(count, this.transform.position);
                eTime = 0f;
                count++;
            }
            eTime += Time.deltaTime;
        }
    }
}

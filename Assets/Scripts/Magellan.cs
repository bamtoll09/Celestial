using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magellan : MonoBehaviour
{
    public TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        tr.startColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
        tr.endColor = tr.startColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

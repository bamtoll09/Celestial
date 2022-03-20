using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommunicator : MonoBehaviour
{
    public SpinFree spinFree;
    public Slider sliderSpinSpeed;
    public Slider progressTLELoad;
    public Text textSatelliteCount;
    public Text textFPS;

    float deltaTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        // textFPS.text = (1f / Time.deltaTime).ToString();
        textFPS.text = (1f / deltaTime).ToString();
    }

    public void ChangeSpinSpeed()
    {
        spinFree.speed = sliderSpinSpeed.value * 19 + 1 ;
    }

    public void ChangeTLELoadProgress(float value)
    {
        progressTLELoad.value = value;
    }

    public void ChangeSatelliteCount(string str)
    {
        textSatelliteCount.text = str;
    }
}

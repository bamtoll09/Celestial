using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public GameObject objEarth, objMoon;
    public float timeScale = 1;

    Planet earth, moon;

    float G = 6.6738480f; // e-11

    // Start is called before the first frame update
    void Start()
    {
        G *= Mathf.Pow(OrbitUtility.meter2unit, 3); // e-29

        earth = new Planet();
        earth.position = Vector3.zero;
        earth.mass = 597.2f; // e+22
        earth.velocity = Vector3.zero;

        moon = new Planet();
        moon.position = new Vector3(38.5f, 0f, 0f) * OrbitUtility.meter2unit; // e+7-6 e+1
        moon.mass = 7.347f; // e+22
        moon.velocity = new Vector3(0f, 102.2f, 0f) * OrbitUtility.meter2unit; // e+1-6 = e-5

        earth.velocity = -moon.velocity * moon.mass / earth.mass; // e-5;

        objMoon.transform.position = moon.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 r = earth.position - moon.position; // e+1
        
        moon.force = G * earth.mass * moon.mass / Mathf.Pow(r.magnitude, 2) * Vector3.Normalize(r); // e-29+22+22-1-1 e+13
        earth.force = -moon.force; // e+13

        moon.velocity = moon.velocity + moon.force / moon.mass * Time.deltaTime * timeScale * Mathf.Pow(10, -4); // e-5 / e+13-22 e-9 => e-5
        earth.velocity = earth.velocity + earth.force / earth.mass * Time.deltaTime * timeScale * Mathf.Pow(10, -4); // e-5

        moon.position = moon.position + moon.velocity * Time.deltaTime * timeScale * Mathf.Pow(10, -6); // e+1 / e-5 => e+1
        earth.position = earth.position + earth.velocity * Time.deltaTime * timeScale * Mathf.Pow(10, -6); // e+1

        objMoon.transform.position = moon.position;
        objEarth.transform.position = earth.position;
    }
}

class Planet
{
    float m_Mass;
    Vector3 m_Position;
    Vector3 m_Velocity;
    Vector3 m_Acceleration;
    Vector3 m_Force;

    public float mass { get => m_Mass; set => m_Mass = value; }
    public Vector3 position { get => m_Position; set => m_Position = value; }
    public Vector3 velocity { get => m_Velocity; set => m_Velocity = value; }
    public Vector3 accelertation { get => m_Acceleration; set => m_Acceleration = value; }
    public Vector3 force { get => m_Force; set => m_Force = value; }
}
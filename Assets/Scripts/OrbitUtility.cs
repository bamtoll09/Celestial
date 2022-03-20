using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitUtility
{
    public static float meter2unit = 3.125f; // e-6

    public static Vector3 eci2unity(Vector3 eci)
    {
        Vector3 temp = new Vector3(eci.x, eci.z, eci.y);
        return temp;
    }
}
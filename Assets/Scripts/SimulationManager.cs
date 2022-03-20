//
// This is a demonstration program that uses the Zeptomoby.OrbitTools
// namespace to calculate position, velocity, and look angles of
// a satellite in earth orbit.
//
// Copyright (c) 2003-2014 Michael F. Henry
// Version 07/2014
//
using System;
using System.Collections.Generic;
using Zeptomoby.OrbitTools;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
   public float timeScale = 1f;
   public GameObject satellite;

   public Transform ECI_COORD;
   public UICommunicator communicator;

   List<GameObject> m_Objects;
   List<Satellite> m_Satellites;
   float eTime;

   void Update() {
      for (int i=0; i<m_Objects.Count; ++i)
      {
         Eci eci = m_Satellites[i].PositionEci(eTime / 60.0);
         Vector3 pos = new Vector3((float)eci.Position.X, (float)eci.Position.Z, (float)eci.Position.Y);
         pos *= OrbitUtility.meter2unit;
         pos *= Mathf.Pow(10, -3);
         m_Objects[i].transform.position = pos;
      }

      eTime += Time.deltaTime * timeScale;   
   }

   public void AddSatellite(string str1, string str2, string str3)
   {
      Tle t = new Tle(str1, str2, str3);
      Satellite s = new Satellite(t);
      m_Satellites.Add(s);

      Eci e = s.PositionEci(0.0);
      Vector3 pos = new Vector3((float)e.Position.X, (float)e.Position.Z, (float)e.Position.Y);
      pos *= OrbitUtility.meter2unit;
      pos *= 0.001f;

      GameObject obj = Instantiate(satellite, pos, Quaternion.identity) as GameObject;
      obj.transform.SetParent(ECI_COORD);
      m_Objects.Add(obj);

      communicator.ChangeSatelliteCount(m_Objects.Count.ToString());
   }

   // /////////////////////////////////////////////////////////////////////
   void Start()
   {
      m_Objects = new List<GameObject>();
      m_Satellites = new List<Satellite>();

      // Sample code to test the SGP4 and SDP4 implementation. The test
      // TLEs come from the NORAD document "Space Track Report No. 3".

      // Test SDP4
      string str1 = "SDP4 Test";
      string str2 = "1 11801U          80230.29629788  .01431103  00000-0  14311-1       8";
      string str3 = "2 11801  46.7916 230.4354 7318036  47.4722  10.4117  2.28537848     6";

      Tle tle2 = new Tle(str1, str2, str3);
      // m_Satellites.Add(new Satellite(tle2));

      // Console.WriteLine("\nExample output:");
      // Debug.LogFormat("\nExample output:");

      // Example: Define a location on the earth, then determine the look-angle
      // to the SDP4 satellite defined above.

      // Create an orbit object using the SDP4 TLE object.
      Satellite satSDP4 = new Satellite(tle2);

      // Get the location of the satellite from the Orbit object. The 
      // earth-centered inertial information is placed into eciSDP4.
      // Here we ask for the location of the satellite 90 minutes after
      // the TLE epoch.
      EciTime eciSDP4 = satSDP4.PositionEci(90.0);

      // Now create a site object. Site m_Objects represent a location on the 
      // surface of the earth. Here we arbitrarily select a point on the
      // equator.
      Site siteEquator = new Site(0.0, -100.0, 0); // 0.00 N, 100.00 W, 0 km altitude

      // Now get the "look angle" from the site to the satellite. 
      // Note that the ECI object "eciSDP4" has a time associated
      // with the coordinates it contains; this is the time at which
      // the look angle is valid.
      Topo topoLook = siteEquator.GetLookAngle(eciSDP4);

      // Print out the results. Note that the Azimuth and Elevation are
      // stored in the CoordTopo object as radians. Here we convert
      // to degrees using Rad2Deg()
      // Console.Write("AZ: {0:f3}  EL: {1:f3}\n", topoLook.AzimuthDeg, topoLook.ElevationDeg);
      /*
      Debug.Log("AZ: " + topoLook.AzimuthDeg
      + "   EL: " + topoLook.ElevationDeg);
      */
   }

   // //////////////////////////////////////////////////////////////////////////
   //
   // Routine to output position and velocity information of satellite
   // in orbit described by TLE information.
   //
   /* static async */ void PrintPosVel(Tle tle)
   {
      const int Step = 360;

      Satellite sat = new Satellite(tle);
      List<Eci> coords = new List<Eci>();

      // Calculate position, velocity
      // mpe = "minutes past epoch"
      for (int mpe = 0; mpe <= (Step * 4); mpe += Step)
      {
         // Get the position of the satellite at time "mpe".
         // The coordinates are placed into the variable "eci".
         Eci eci = sat.PositionEci(mpe);

         // Add the coordinate object to the list
         coords.Add(eci);
      }

      // Print TLE data
      // Console.Write("{0}\n", tle.Name);
      // Console.Write("{0}\n", tle.Line1);
      // Console.Write("{0}\n", tle.Line2);
      Debug.LogFormat("{0}\n", tle.Name);
      Debug.LogFormat("{0}\n", tle.Line1);
      Debug.LogFormat("{0}\n", tle.Line2);

      // Header
      // Console.Write("\n  TSINCE            X                Y                Z\n\n");
      Debug.LogFormat("\n  TSINCE            X                Y                Z\n\n");

      // Iterate over each of the ECI position m_Objects pushed onto the
      // coordinate list, above, printing the ECI position information
      // as we go.
      for (int i = 0; i < coords.Count; i++)
      {
         Eci e = coords[i] as Eci;

         /* Console.Write("{0,8}.00 {1,16:f8} {2,16:f8} {3,16:f8}\n",
                     i * Step,
                     e.Position.X,
                     e.Position.Y,
                     e.Position.Z); */
         Debug.LogFormat("{0,8}.00 {1,16:f8} {2,16:f8} {3,16:f8}\n",
                     i * Step,
                     e.Position.X,
                     e.Position.Y,
                     e.Position.Z);
      }

      // Console.Write("\n                  XDOT             YDOT             ZDOT\n\n");
      Debug.LogFormat("\n                  XDOT             YDOT             ZDOT\n\n");

      // Iterate over each of the ECI position m_Objects in the coordinate
      // list again, but this time print the velocity information.
      for (int i = 0; i < coords.Count; i++)
      {
         Eci e = coords[i] as Eci;

         /*
         Console.Write("{0,24:f8} {1,16:f8} {2,16:f8}\n",
                     e.Velocity.X,
                     e.Velocity.Y,
                     e.Velocity.Z);
                     */
         Debug.LogFormat("{0,24:f8} {1,16:f8} {2,16:f8}\n",
                     e.Velocity.X,
                     e.Velocity.Y,
                     e.Velocity.Z);
      }
   }
}

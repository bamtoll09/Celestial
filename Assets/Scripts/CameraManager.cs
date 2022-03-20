using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public SpinFree spinFree;
    public List<Transform> cameraPoints;
    
    List<bool> m_GoBack;
    Vector3 m_OriginPos;
    Quaternion m_OriginRot;
    Vector3 m_CameraVel = Vector3.zero;
    float m_CameraMoveSpeed = 0.3f;
    float m_CameraRotateSpeed = 0.35f;
    bool m_IsArrived;

    // Start is called before the first frame update
    void Start()
    {
        m_GoBack = new List<bool>();
        m_OriginPos = Camera.main.transform.position;
        m_OriginRot = Camera.main.transform.rotation;

        for (int i=0; i<cameraPoints.Count; ++i)
        {
            m_GoBack.Add(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            m_GoBack[0] = !m_GoBack[0];
            m_IsArrived = false;
        }

        if (m_GoBack[0])
        {
            MoveSmooth(cameraPoints[0]);
            RotateSmooth(cameraPoints[0]);
        }
        else
        {
            ResetSmooth();
        }
    }

    void MoveSmooth(Transform tf)
    {
        if (m_IsArrived) Camera.main.transform.position = tf.position;

        else
        {
            Camera.main.transform.position
                = Vector3.SmoothDamp(Camera.main.transform.position, tf.position, ref m_CameraVel, m_CameraMoveSpeed / Mathf.Pow(spinFree.speed, 1/3f));

            if ((Camera.main.transform.position - tf.position).sqrMagnitude < 0.005f)
                m_IsArrived = true;
        }
    }

    void RotateSmooth(Transform tf)
    {
        if (m_IsArrived) Camera.main.transform.rotation = tf.rotation;

        else
        {
            Camera.main.transform.rotation
                = Quaternion.Lerp(Camera.main.transform.rotation, tf.rotation, m_CameraRotateSpeed * spinFree.speed * 0.3f);

            // if (Quaternion.Angle(Camera.main.transform.rotation, tf.rotation) < 0.5f)
        }
    }

    void ResetSmooth()
    {
        if ((Camera.main.transform.position - m_OriginPos).sqrMagnitude < 0.003f)
        {
            Camera.main.transform.position = m_OriginPos;
            Camera.main.transform.rotation = m_OriginRot;
        }
        else
        {
            Camera.main.transform.position
                = Vector3.SmoothDamp(Camera.main.transform.position, m_OriginPos, ref m_CameraVel, m_CameraMoveSpeed);

            Camera.main.transform.rotation
                = Quaternion.Lerp(Camera.main.transform.rotation, m_OriginRot, m_CameraRotateSpeed);
        }
    }
}

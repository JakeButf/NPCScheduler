using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if (UNITY_EDITOR)
using UnityEngine;
#endif

public class NPCPath : MonoBehaviour
{
    [SerializeField]
    public List<Transform> pathingPoints = new List<Transform>();
    public float LineLength;

    private void Awake()
    {
        LineLength = CalculateLineLength();
    }

    public List<Transform> GetPathPoints()
    {
        //CalculateLineLength();
        return pathingPoints;
    }

    private float CalculateLineLength()
    {
        float length = 0;
        for(int i = 0; i < pathingPoints.Count; i++)
        {
            if(i != 0)
            {
                length += (pathingPoints[i].position.magnitude - pathingPoints[i - 1].position.magnitude);
            }
        }

        return length;
    }

#if (UNITY_EDITOR)
    private void OnDrawGizmos()
    {
        for (int i = 0; i < pathingPoints.Count; i++)
        {
            Handles.Label(pathingPoints[i].position, "NPC Path Point");
            if (i != 0)
            {
                Gizmos.DrawLine(pathingPoints[i - 1].position, pathingPoints[i].position);
            }
        }
    }

#endif
}

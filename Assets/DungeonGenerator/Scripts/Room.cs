using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform[] connectionPoints;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        foreach(Transform point in connectionPoints)
            Gizmos.DrawWireCube(point.position, new Vector3(5, 5, 5));
    }
}

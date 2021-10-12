﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class LevelGenerator : MonoBehaviour
{
    [Range(5, 25)]
    [SerializeField]
    private int numMaxOfRooms;

    private Transform[] availableRooms;
    private List<Transform> rooms = new List<Transform>();
    private List<Transform> connectionPoints = new List<Transform>();
 
    void Start()
    {
        if(PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            availableRooms = transform.Find("AvailableRooms").Cast<Transform>().ToArray();
            StartCoroutine("Generate");
        }
    }


    /// <summary>Generate procedural level.</summary>
    void Generate()
    {
        while(rooms.Count < numMaxOfRooms)
        {
            Transform roomPoint = connectionPoints.Count > 0 ? connectionPoints[Random.Range(0, connectionPoints.Count - 1)] : transform;
            CreateRoom(roomPoint); 
        }
    }  

    /// <summary>Try create room at  <paramref name="point"/>.</summary>
    /// <param name="pooint">Point of new Room</param>
    void CreateRoom(Transform point)
    {
        List<RoomConfiguration> compatibleRooms = new List<RoomConfiguration>();

        foreach(Transform tempRoom in availableRooms)
        {

            tempRoom.transform.position = point.position;

            foreach(Transform tempConnection in tempRoom.GetComponent<Room>().connectionPoints)
            {
                Quaternion rot = Quaternion.FromToRotation(tempConnection.forward, -point.forward);
                tempRoom.rotation = rot * tempRoom.rotation;
                tempRoom.rotation = Quaternion.Euler(tempRoom.eulerAngles.x, tempRoom.eulerAngles.y, 0);
                tempRoom.position = point.position - (tempConnection.position - tempRoom.position);

                if(CheckRoomInstersects(tempRoom))
                {
                    compatibleRooms.Add(new RoomConfiguration(tempRoom, tempRoom.position, tempRoom.rotation));
                }
            }
        }

        if(compatibleRooms.Count > 0)
        {
            RoomConfiguration compatibleRoom = compatibleRooms[Random.Range(0, compatibleRooms.Count)];
            Transform newRoom = PhotonNetwork.Instantiate(compatibleRoom.room.name, compatibleRoom.position, compatibleRoom.rotation).transform;
            newRoom.gameObject.SetActive(true);
            rooms.Add(newRoom);
            connectionPoints.AddRange(newRoom.GetComponent<Room>().connectionPoints);
        }

    }

    /// <summary>
    /// Check if <paramref name="tempRoom"/> intersects another room bounds.
    /// </summary>
    /// <param name="tempRoom">Room to check intersects</param>
    bool CheckRoomInstersects(Transform tempRoom)
    {
        bool flag = true;
        Bounds tempRoomBounds = tempRoom.gameObject.GetComponent<Renderer>().bounds;


        foreach(Transform room in rooms)
        {
            Bounds roomBounds = room.gameObject.GetComponent<Renderer>().bounds;

            if(tempRoomBounds.Intersects(roomBounds))
            {
                flag = false;
                break;
            }
        } 

        return flag;
    }
    
}

[System.Serializable]
public class RoomConfiguration 
{
    public Transform room;
    public Vector3 position;
    public Quaternion rotation;

    public RoomConfiguration(Transform room, Vector3 position, Quaternion rotation) 
    {
        this.room = room;
        this.position = position;
        this.rotation = rotation;
    }

}

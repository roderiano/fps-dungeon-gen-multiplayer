using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class DungeonGenerator : MonoBehaviour
{
    [Range(5, 25)]
    [SerializeField]
    private int numMaxOfRooms;

    private Transform[] availableRooms;
    private List<Transform> rooms = new List<Transform>();
    private List<Transform> connectionPoints = new List<Transform>();
 
    void Start()
    {
        if(PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient )
        {
            availableRooms = transform.Find("AvailableRooms").Cast<Transform>().ToArray();
            StartCoroutine("Generate");
        }
    }


    /// <summary>Generate procedural dungeon.</summary>
    IEnumerator Generate()
    {
        while(rooms.Count < numMaxOfRooms)
        {
            Transform roomPoint = connectionPoints.Count > 0 ? connectionPoints[Random.Range(0, connectionPoints.Count - 1)] : transform;
            yield return StartCoroutine(CreateRoom(roomPoint));
        }

        ActiveDoors(); 
    }  

    /// <summary>Try create room at  <paramref name="point"/>.</summary>
    /// <param name="connectionPoint">Point of new Room</param>
    IEnumerator CreateRoom(Transform connectionPoint)
    {
        List<RoomConfiguration> compatibleRooms = new List<RoomConfiguration>();

        foreach(Transform tempRoom in availableRooms)
        {
            tempRoom.gameObject.SetActive(true);

            tempRoom.transform.position = connectionPoint.position;

            foreach(Transform tempConnection in tempRoom.GetComponent<Room>().connectionPoints)
            {
                Quaternion rot = Quaternion.FromToRotation(tempConnection.forward, -connectionPoint.forward);
                tempRoom.rotation = rot * tempRoom.rotation;
                tempRoom.rotation = Quaternion.Euler(tempRoom.eulerAngles.x, tempRoom.eulerAngles.y, 0);
                tempRoom.position = connectionPoint.position - (tempConnection.position - tempRoom.position);

                if(!CheckRoomInstersects(tempRoom, connectionPoint))
                {
                    compatibleRooms.Add(new RoomConfiguration(tempRoom, tempRoom.position, tempRoom.rotation));
                }

                yield return null;
            }

            tempRoom.gameObject.SetActive(false);
        }

        if(compatibleRooms.Count > 0)
        {
            RoomConfiguration compatibleRoom = compatibleRooms[Random.Range(0, compatibleRooms.Count)];
            Transform newRoom = PhotonNetwork.Instantiate(compatibleRoom.room.name, compatibleRoom.position, compatibleRoom.rotation).transform;
            newRoom.gameObject.SetActive(true);
            rooms.Add(newRoom);
            
            // Add new connection points 
            connectionPoints.AddRange(newRoom.GetComponent<Room>().connectionPoints);

            // Remove connection points with the same position as the new room 
            if(rooms.Count > 1)
                connectionPoints.RemoveAll(t => t.position == connectionPoint.position);
        }
    }

    /// <summary>
    /// Check if <paramref name="tempRoom"/> intersects another room bounds.
    /// </summary>
    /// <param name="tempRoom">Room to check intersects</param>
    /// <param name="sourceTempConnectionPoint">Source Room to ignore check intersects</param>
    bool CheckRoomInstersects(Transform tempRoom, Transform sourceTempConnectionPoint)
    {
        bool flag = false;
        Bounds tempRoomBounds = tempRoom.gameObject.GetComponent<MeshRenderer>().bounds;

        // Check if exists room for sourceConnectionPoint
        Transform sourceRoom = sourceTempConnectionPoint.parent;

        foreach(Transform room in rooms)
        {
            Bounds roomBounds = room.gameObject.GetComponent<MeshRenderer>().bounds;

            // Check if is source room and it isn`t inside the source
            if(room == sourceRoom)
            {
                Vector3 offset = new Vector3(3f, 3f, 3f);
                if((!roomBounds.Contains(tempRoomBounds.min + offset) && !roomBounds.Contains(tempRoomBounds.max  - offset)) &&
                !tempRoomBounds.Contains(roomBounds.min + offset) && !tempRoomBounds.Contains(roomBounds.max  - offset))
                    continue;
            }

            if(tempRoomBounds.Intersects(roomBounds))
            {
                flag = true;
                break;
            }
        } 

        return flag;
    }

    /// <summary>Active Doors by Connection Points.</summary>
    void ActiveDoors()
    {
        foreach(Transform point in connectionPoints)
            point.Find("Door/door").gameObject.SetActive(true);
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

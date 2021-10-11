using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [Range(5, 25)]
    [SerializeField]
    private int numMaxOfRooms;
    [SerializeField]
    private Transform[] availableRooms;
    
    private List<Transform> rooms = new List<Transform>();
    private List<Transform> connectionPoints = new List<Transform>();
 
    void Start()
    {
        availableRooms = transform.Find("AvailableRooms").Cast<Transform>().ToArray();
        StartCoroutine("Generate");
    }


    IEnumerator Generate()
    {
        while(rooms.Count < numMaxOfRooms)
        {
            yield return new WaitForSeconds(0.02f);

            Transform roomPoint = connectionPoints.Count > 0 ? connectionPoints[Random.Range(0, connectionPoints.Count - 1)] : transform;
            CreateRoom(roomPoint); 
        }
    }  

    void CreateRoom(Transform point)
    {
        List<Transform> compatibleRooms = new List<Transform>();

        foreach(Transform tempRoom in availableRooms)
        {

            tempRoom.transform.position = point.position;

            foreach(Transform tempConnection in tempRoom.GetComponent<Room>().connectionPoints)
            {
                Quaternion rot = Quaternion.FromToRotation(tempConnection.forward, -point.forward);
                tempRoom.rotation = rot * tempRoom.rotation;
                tempRoom.rotation = Quaternion.Euler(tempRoom.eulerAngles.x, tempRoom.eulerAngles.y, 0);
                tempRoom.position = point.position - (tempConnection.position - tempRoom.position);
            }
            
               
            if(CheckRoomInstersects(tempRoom))
            {
                compatibleRooms.Add(tempRoom);
            }
        }

        if(compatibleRooms.Count > 0)
        {
            Transform compatibleRoom = compatibleRooms[Random.Range(0, compatibleRooms.Count - 1)];
            Transform newRoom = Instantiate(compatibleRoom, compatibleRoom.position, compatibleRoom.rotation);
            newRoom.gameObject.SetActive(true);
            rooms.Add(newRoom);
            connectionPoints.AddRange(newRoom.GetComponent<Room>().connectionPoints);
        }

    }

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

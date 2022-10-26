using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room/RoomSO", fileName = "RoomSO_")]
public class RoomSO : ScriptableObject
{
    public int width;
    public int height;
    public int radius;
    public Vector3 FixedPosition;
    public int FixedRoomID;
    public GameObject virtualRoom;

    public List<GameObject> EntityRooms;
}

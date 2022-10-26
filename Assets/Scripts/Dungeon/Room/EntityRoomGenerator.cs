using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRoomGenerator
{

    public static void GenerateEntity(Vector3 position, VirtualRoom vRoom, Dictionary<int, Room> dic_roomID, GameObject entityParent)
    {
        List<GameObject> entities = vRoom.roomSO.EntityRooms;
        int index = Random.Range(0, entities.Count);
        var room = GameObject.Instantiate(entities[index], entityParent.transform).GetComponent<Room>();
        room.Init(vRoom.ID, vRoom.roomType, vRoom.transform.position);
        room.CreatePathFindingGraph();
        DungeonManager.Instance.Record(room, vRoom.roomType);
        if (!dic_roomID.ContainsKey(vRoom.ID))
        {
            dic_roomID.Add(vRoom.ID, room);
        }
    }

    public static void ConnectRoom(int Id, int targetId, Dictionary<int, Room> dic_roomID)
    {
        if (dic_roomID.TryGetValue(Id, out Room room))
        {
            if (dic_roomID.TryGetValue(targetId, out Room targetRoom))
            {
                room.AddRoomConnection(CreateRoomConnection(targetRoom));
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("Target room not found Id: " + targetId);
#endif
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("room not found Id: " + Id);
#endif
        }
    }
    static RoomConnection CreateRoomConnection(Room room)
    {
        return new RoomConnection(room);
    }


}

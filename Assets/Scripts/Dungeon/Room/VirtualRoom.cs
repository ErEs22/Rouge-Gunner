using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualRoom : MonoBehaviour
{
    public int ID;

    public bool separating = true;

    public Vector2Int position;
    public float radius { get; private set; }
    public float squaredRadius { get => radius * radius; }

    public List<VirtualRoomConnection> connections;

    public List<VirtualRoom> neighbors = new List<VirtualRoom>();

    public RoomType roomType;

    public RoomSO roomSO;

    private void Awake()
    {
        connections = new List<VirtualRoomConnection>();
        separating = true;
    }

    public bool AddRoomConnection(VirtualRoomConnection connection)
    {

        if (!connections.Contains(connection))
        {
            foreach (var item in connections)
            {
                if (item.connectedRoom == connection.connectedRoom)
                {
                    return false;
                }
            }
            connections.Add(connection);
            return true;
        }
        return false;
    }

    public void Init(float radius , Vector3 position,RoomType roomType,RoomSO roomSO)
    {
        this.radius = radius;
        transform.position = position;
        this.roomType = roomType;
        this.roomSO = roomSO;
    }

    public void SetPosition(Vector3 pos)
    {
        
        position = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
        transform.position = new Vector3Int(Mathf.RoundToInt(pos.x), 0,Mathf.RoundToInt(pos.z));
    }

    public VirtualRoomConnection GetConnectedRoom(VirtualRoom vRoom)
    {
        VirtualRoomConnection result = null;
        foreach (var item in connections)
        {
            if (item.connectedRoom == vRoom)
            {
                result = item;
            }
        }
        return result;
    }
}

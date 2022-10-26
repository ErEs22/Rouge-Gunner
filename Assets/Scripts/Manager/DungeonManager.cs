using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    UIManager uIManager;
    //描述当前地牢的构成
    public Room start;
    public Room end;

    public Room room_currentPlayerPosIn;//当前玩家所在房间

    protected override void Awake()
    {
        base.Awake();
        uIManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        EventManager.current.OnDungeonGenerated += GenerateCompelete;
    }

    void GenerateCompelete()
    {
        
        PlayerManager.Instance.SpawnCharacter(start.GetDestination());
        UpdateCurrentRoomPlayerPosIn(start);
        //AstarPath.active.Scan();
    }

    public void Record(Room room,RoomType roomType)
    {
        if (roomType == RoomType.Start)
        {
            start = room;
        }
        else if (roomType == RoomType.End)
        {
            end = room;
        }
    }

    /// <summary>
    /// 更新玩家所在所在房间
    /// </summary>
    public void UpdateCurrentRoomPlayerPosIn(Room room)
    {
        room_currentPlayerPosIn = room;
        uIManager.UpdatePlayerPositionInMinimap();
    }

    public void ActivePortal(int index)
    {
        room_currentPlayerPosIn.SetPortal(room_currentPlayerPosIn.connections[index].connectedRoom);
        
    }
}

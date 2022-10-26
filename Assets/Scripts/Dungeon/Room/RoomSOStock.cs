using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class used to store RoomStats ScriptableObject
/// You can imagine this as a storehouse
/// </summary>
[CreateAssetMenu(menuName ="Room/RoomSOStock" ,fileName = "RoomSOStock")]
public class RoomSOStock : ScriptableObject
{
    public List<RoomSO> roomSOList;

    [Header("特殊房间信息")]
    public RoomSO AngelRoom;
    public RoomSO DevilRoom;
}

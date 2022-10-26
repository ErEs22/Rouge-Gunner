using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VirtualRoomConnection 
{
    public VirtualRoom connectedRoom
    {
        get;
        private set;
    }

    public VirtualRoomConnection(VirtualRoom room)
    {
        connectedRoom = room;
    }

}

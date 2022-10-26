using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomConnection
{
    public Mark start;
    public Mark end;
    public Color color;
    [SerializeField]
    private LineRenderer line;
    public Room connectedRoom
    {
        get;
        private set;
    }

    public RoomConnection(Room room)
    {
        connectedRoom = room;
    }


    public void SetLine(Vector3 start,Vector3 end,Mark startMark,Mark endMark)
    {
        color = ColorManager.GetColor();
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        this.start = startMark;
        this.end = endMark;
        line.startColor = color;
        line.endColor = color;
        line.gameObject.SetActive(true);
        SetMarkPosition(start,end, color);
    }

    public void InitLine(LineRenderer line)
    {
        this.line = line;
        line.gameObject.SetActive(false);
    }

    private void SetMarkPosition(Vector3 startPosition, Vector3 endPosition,Color color)
    {

        start.EnableMark(startPosition, color);
        end.EnableMark(endPosition, color);
    }
}

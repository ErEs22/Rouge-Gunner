using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Room : MonoBehaviour
{
    public Portal reachPoint;
    public int roomId;
    public RoomType roomType;

    public bool noHint;//没有连接

    public bool isCleared;

    public bool Set;//标识房间是否初始化完毕

    public List<RoomConnection> connections = new List<RoomConnection>();

    public List<LineRenderer> lines = new List<LineRenderer>();

    public List<Mark> marks = new List<Mark>();

    [DisplayOnly] public List<GameObject> enemys = new List<GameObject>();

    [SerializeField] EnemyGenerator[] enemyGenerators;
    [SerializeField] Transporter transporter;

    [Header("标识半径")]
    public float radius;

    private void Awake()
    {

    }

    private void Start()
    {
        Set = true;
    }

    private void OnDisable()
    {
    }

    public void CreatePathFindingGraph()
    {
        AstarData data = AstarPath.active.data;
        GridGraph gg = data.AddGraph(typeof(GridGraph)) as GridGraph;
        gg.center = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        gg.name = gameObject.name;
        gg.erodeIterations = 2;
        float nodeSize = 0.5f;
        int width = (int)transform.localScale.x;
        int depth = (int)transform.localScale.z;
        gg.SetDimensions((int)(width / nodeSize), (int)(depth / nodeSize), nodeSize);
    }

    /// <summary>
    /// 房间开始激活，倒计时后开始刷敌人
    /// </summary>
    public void BeginActivateRoom()
    {
        StartCountDown();
        enemys.Clear();
        isCleared = false;

        Invoke(nameof(EndCountDown), 3f);
        Invoke(nameof(ActivateRoom), 3f);
        Invoke(nameof(StartCheckRoomIsCleared), 10f);
    }

    void StartCountDown()
    {
        EventManager.current.CountDownUIStarted();
    }

    void EndCountDown()
    {
        EventManager.current.CountDownUIEnded();
    }

    void StartCheckRoomIsCleared()
    {
        StartCoroutine(nameof(CheckRoomIsCleared));
    }

    void ActivateRoom()
    {
        foreach (var item in enemyGenerators)
        {
            item.StartGenerateEnemys();
        }
    }

    public void Init(int roomId, RoomType roomType, Vector3 position)
    {
        this.roomId = roomId;
        this.roomType = roomType;
        transform.position = position;
    }
    public void AddRoomConnection(RoomConnection connection)
    {

        if (!connections.Contains(connection))
        {
            foreach (var item in connections)
            {
                if (item.connectedRoom == connection.connectedRoom)
                {
                    return;
                }
            }
            connections.Add(connection);
        }
    }

    public void ConnectMark()
    {
        if (noHint)
        {
            return;
        }


        foreach (var connect in connections)
        {
            if (!DungenGenerator.Instance.IsRepetitive(roomId, connect.connectedRoom.roomId))
            {
                Vector3 dir = connect.connectedRoom.transform.position - transform.position;
                dir.Normalize();
                Vector3 start = dir * radius + transform.position;
                Vector3 end = -dir * radius + connect.connectedRoom.transform.position;
                Mark startMark = GetAvailableMark();
                Mark endMark = connect.connectedRoom.GetAvailableMark();
                if (startMark != null && endMark != null)
                    connect.SetLine(start, end, startMark, endMark);
                DungenGenerator.Instance.AddConnectIDs(roomId, connect.connectedRoom.roomId);
            }

        }
    }

    public void InitLines()
    {

        for (int i = 0; i < connections.Count; i++)
        {
            connections[i].InitLine(lines[i]);
        }

    }

    public Mark GetAvailableMark()
    {
        Mark result = null;
        foreach (var item in marks)
        {
            if (!item.connected)
            {
                result = item;
                break;
            }
        }
        return result;
    }

    IEnumerator CheckRoomIsCleared()
    {
        int enemyCount = 0;
        while (true)
        {
            enemyCount = 0;
            yield return null;
            foreach (var enemy in enemys)
            {
                if (enemy.activeSelf == true)
                {
                    enemyCount++;
                }
            }
            if (enemyCount == 0)
            {
                isCleared = true;
                yield break;
            }
        }
    }

    public Vector3 GetDestination()
    {
        return reachPoint.transform.position;
    }

    public void SetPortal(Room room)
    {
        transporter.portal.SetDestination(room);
    }
}

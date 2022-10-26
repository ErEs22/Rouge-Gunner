using Delaunay;
using Delaunay.Geo;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungenGenerator : Singleton<DungenGenerator>
{
    GameObject VirtualParent;
    GameObject EntityParent;
    public int roomCount;
    public RoomSOStock RoomStock;
    [SerializeField]
    [Range(1, 100)]
    float unitCircleradius;

    [SerializeField]
    List<VirtualRoom> vRooms = new List<VirtualRoom>();

    [Header("Separation Behaviour")]
    [SerializeField]
    bool separationCompelete;
    [Range(1f, 10f)]
    [SerializeField]
    float speedMultiplier;//虚房间移动速度倍率

    [Header("DelaunayTriangulation")]
    [SerializeField]
    float RoomConnectionFrequency = 0.15f;//连通返还率
    [SerializeField]
    bool delaunayied;//三角剖分状态

    List<Vector2> Points = new List<Vector2>();

    List<LineSegment> SpanningTree;//生成树
    List<LineSegment> DelaunayTriangulation;//三角剖分边关系

    Dictionary<Vector2, VirtualRoom> dic_vRoom = new Dictionary<Vector2, VirtualRoom>();
    Dictionary<VirtualRoom, int> ConnectionCounter = new Dictionary<VirtualRoom, int>();

    const float plotBounds = 500;//三角剖分的绘图边界

    [Header("Room Infomation")]
    [SerializeField]
    VirtualRoom start = null;
    [SerializeField]
    VirtualRoom end = null;
    public Dictionary<int, Room> dic_roomID = new Dictionary<int, Room>();
    [SerializeField]
    List<VirtualRoom> surplusVRoom = new List<VirtualRoom>();//剩余可被转化为特殊房的集合
    List<VirtualRoom> UnChangeableRooms = new List<VirtualRoom>();//不可转化的特殊房集合

    [SerializeField]
    public List<int> RoomConnectIDList = new List<int>();

    const int MaximumConnectionCount = 4;

    [SerializeField] bool needScan=true;//标识是否需要对graph进行扫描

    private void Start()
    {
        EventManager.current.OnEntityRoomGenerate += OnEntityRoomSpawn;
        GenerateVirtualRoom();
    }

    private void Update()
    {
        if (!separationCompelete)
        {
            SteeringSeparation();
            CheckSeparationState();
        }
        else
        {
            if (!delaunayied)
            {
                Delaunay();
                EventManager.current.DungeonGenerated();
                //EventManager.current.EntityRoomGenerate();
            }
            else
            {
                isGraphDrawn();
            }
        }
    }

    void isGraphDrawn()
    {
        if (!needScan)
        {
            return;
        }
        foreach (var kvp in dic_roomID)
        {
            if (!kvp.Value.Set)
                return;
        }
        AstarPath.active.Scan();
        needScan = false;
    }

    /// <summary>
    /// 生成虚房间
    /// </summary>
    private void GenerateVirtualRoom()
    {
        VirtualParent = new GameObject("Virtual Parent");
        List<RoomSO> roomSOs = RoomStock.roomSOList;
        for (int i = 0; i < roomCount; i++)
        {
            RoomSO roomSO = roomSOs[Random.Range(0, roomSOs.Count)];
            VirtualRoom vRoom = Instantiate(roomSO.virtualRoom, VirtualParent.transform).GetComponent<VirtualRoom>();
            Vector3 position = GetRandomPositionInCircle(unitCircleradius);
            vRoom.Init(roomSO.radius, position, RoomType.Normal,roomSO);
            vRoom.ID = i;
            vRooms.Add(vRoom);
            surplusVRoom.Add(vRoom);
        }
    }

    /// <summary>
    /// 在圆中符合正态分布生成房间
    /// </summary>
    /// <param name="radius">圆半径</param>
    /// <returns></returns>
    private Vector3 GetRandomPositionInCircle(float radius)
    {
        float angle = Random.Range(0f, 1f) * Mathf.PI * 2f;
        float rad = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;
        float x = rad * Mathf.Cos(angle);
        float z = rad * Mathf.Sin(angle);
        return new Vector3((int)x, 0, (int)z);
    }


    /// <summary>
    /// 分离行为
    /// 该方法主要用于调度作业
    /// </summary>
    private void SteeringSeparation()
    {
        NativeArray<float3> positionArray = new NativeArray<float3>(vRooms.Count, Allocator.TempJob);
        NativeArray<float> squaredRadiusArray = new NativeArray<float>(vRooms.Count, Allocator.TempJob);
        NativeArray<bool> statusArray = new NativeArray<bool>(vRooms.Count, Allocator.TempJob);
        //复制数据指向其数据内存
        for (int i = 0; i < vRooms.Count; i++)
        {
            positionArray[i] = vRooms[i].transform.position;
            squaredRadiusArray[i] = vRooms[i].squaredRadius;
            statusArray[i] = vRooms[i].separating;
        }

        FindPlaceJob findPlaceJob = new FindPlaceJob()//初始化作业
        {
            delta = Time.deltaTime,
            positionArray = positionArray,
            squaredRadiusArray = squaredRadiusArray,
            statusArray = statusArray,
            speedMultiplier = speedMultiplier
        };
        JobHandle jobHandle = findPlaceJob.Schedule(vRooms.Count, 1);
        jobHandle.Complete();

        for (int i = 0; i < vRooms.Count; i++)
        {
            vRooms[i].transform.position = positionArray[i];
            vRooms[i].separating = statusArray[i];
        }
        positionArray.Dispose();
        squaredRadiusArray.Dispose();
        statusArray.Dispose();
    }

    /// <summary>
    /// 检测当前分离是否完毕
    /// </summary>
    public void CheckSeparationState()
    {
        foreach (var vroom in vRooms)
        {
            if (vroom.separating)
            {
                return;
            }
        }
        separationCompelete = true;
        foreach (var vroom in vRooms)
        {
            vroom.SetPosition(vroom.transform.position);
        }
    }

    /// <summary>
    /// 分离行为作业
    /// 若VirtualRoom的separating属性为false时则说明该VRoom已停止分离
    /// </summary>
    [BurstCompile]
    public struct FindPlaceJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] public NativeArray<float3> positionArray;
        [NativeDisableParallelForRestriction] public NativeArray<float> squaredRadiusArray;
        [NativeDisableParallelForRestriction] public NativeArray<bool> statusArray;
        public float delta;
        public float speedMultiplier;
        public void Execute(int index)
        {
            float3 avoidanceMove = float3.zero;
            int nAvoid = 0;

            for (int i = 0; i < positionArray.Length; i++)
            {
                if (i == index)
                {
                    continue;
                }
                if (math.lengthsq(positionArray[index] - positionArray[i]) < (squaredRadiusArray[index] + squaredRadiusArray[i]))
                {
                    nAvoid += 1;
                    avoidanceMove += positionArray[index] - positionArray[i];
                    avoidanceMove.y = 0;
                }
            }
            if (nAvoid > 0)
            {
                avoidanceMove /= nAvoid;
                statusArray[index] = true;
            }
            else
            {
                statusArray[index] = false;
            }
            positionArray[index] += avoidanceMove * delta * speedMultiplier;
        }
    }

    /// <summary>
    /// 三角剖分，并确立房间
    /// </summary>
    private void Delaunay()
    {
        List<uint> colors = new List<uint>();
        dic_vRoom = new Dictionary<Vector2, VirtualRoom>();

        for (int i = 0; i < roomCount; i++)
        {
            Points.Add(vRooms[i].position);
            colors.Add(0);
            if (!dic_vRoom.ContainsKey(vRooms[i].position))
            {
                dic_vRoom.Add(vRooms[i].position, vRooms[i]);
            }
            SetNeighbours(i);
        }

        Voronoi v = new Voronoi(Points, colors, new Rect(0, 0, plotBounds, plotBounds));//绘制500X500的Voronoi图
        SpanningTree = v.SpanningTree(KruskalType.MINIMUM);
        DelaunayTriangulation = v.DelaunayTriangulation();

        SaveSpanningTreeLineInfo();
        List<VirtualRoom> roomsWithOneConnection = new List<VirtualRoom>();
        List<VirtualRoom> roomsWithTwoConnection = new List<VirtualRoom>();
        SetStartAndEndRooms(roomsWithOneConnection, roomsWithTwoConnection);
        SpawnTreasureRoom(roomsWithOneConnection, roomsWithTwoConnection);
        //for (int i = 0; i < vRooms.Count; i++)
        //{
        //    for (int n = 0; n < vRooms[i].connections.Count; n++)
        //    {
        //        Debug.Log(vRooms[i].ID + "Connection " + vRooms[i].connections[n].connectedRoom.ID);
        //    }
        //}
        delaunayied = true;
        EventManager.current.EntityRoomGenerate();

    }

    /// <summary>
    /// <para>确立开始房和结束房</para>
    /// 开始房与结束房优先从单连通房间选择再次从双连通中选择
    /// </summary>
    private void SetStartAndEndRooms(List<VirtualRoom> roomsWithOneConnection,List<VirtualRoom> roomsWithTwoConnection)
    {
        //check connection counters
        foreach (KeyValuePair<VirtualRoom, int> kvp in ConnectionCounter)
        {
            if (kvp.Value == 1)
            {
                roomsWithOneConnection.Add(kvp.Key);
            }
            if (kvp.Value == 2)
            {
                roomsWithTwoConnection.Add(kvp.Key);
            }
        }

        float distance = 0;

        //attempt to grab start room
        if (roomsWithOneConnection.Count >= 1)
        {
            start = roomsWithOneConnection[0];
            roomsWithOneConnection.RemoveAt(0);
        }
        else if (roomsWithTwoConnection.Count >= 1)
        {
            start = roomsWithTwoConnection[0];
            roomsWithTwoConnection.RemoveAt(0);
        }

        bool endIntwoRoom = false;

        //attempt to grab end room
        if (start != null)
        {
            for (int n = 0; n < roomsWithTwoConnection.Count; n++)
            {
                float d = (roomsWithTwoConnection[n].position - start.position).sqrMagnitude;
                if (d > distance)
                {
                    distance = d;
                    end = roomsWithTwoConnection[n];
                    //roomsWithTwoConnection.RemoveAt(n);
                    endIntwoRoom = true;
                }
            }
            for (int n = 0; n < roomsWithOneConnection.Count; n++)
            {
                float d = (roomsWithOneConnection[n].position - start.position).sqrMagnitude;
                if (d > distance)
                {
                    distance = d;
                    end = roomsWithOneConnection[n];
                    roomsWithOneConnection.RemoveAt(n);
                    endIntwoRoom = false;
                }
            }
            if (end != null && endIntwoRoom)
            {
                roomsWithTwoConnection.Remove(end);
                int indexToDelete = Random.Range(0, 2);
                var willDelete = end.connections[indexToDelete].connectedRoom;
                //因为终点房是双边房 所以随机删除它的一条边同时也需要将其从对方连接中删除
                var destination=end.connections[indexToDelete].connectedRoom.GetConnectedRoom(end);
                if (destination!=null)
                {
                    end.connections[indexToDelete].connectedRoom.connections.Remove(destination);
                    
                }
                end.connections.RemoveAt(indexToDelete);
                var lastConnectedRoom = end.connections[0].connectedRoom;
                SupplementSide(GetSupplementRoom(willDelete, lastConnectedRoom));
            }
        }
        start.roomType = RoomType.Start;
        end.roomType = RoomType.End;
        UnChangeableRooms.Add(start);
        UnChangeableRooms.Add(end);
        surplusVRoom.Remove(start);
        surplusVRoom.Remove(end);
    }

    /// <summary>
    /// 储存最小生成树的边信息
    /// </summary>
    private void SaveSpanningTreeLineInfo()
    {
        for (int i = 0; i < SpanningTree.Count; i++)
        {
            //Create an index so we can keep track of the actual connections count of each main room
            if (!ConnectionCounter.ContainsKey(dic_vRoom[SpanningTree[i].p0.Value]))
                ConnectionCounter.Add(dic_vRoom[SpanningTree[i].p0.Value], 0);
            if (!ConnectionCounter.ContainsKey(dic_vRoom[SpanningTree[i].p1.Value]))
                ConnectionCounter.Add(dic_vRoom[SpanningTree[i].p1.Value], 0);

            //increment the counter



            //Add the room connection to the Room object
            if (dic_vRoom[SpanningTree[i].p0.Value].AddRoomConnection(CreateRoomConnection(SpanningTree[i].p1.Value)))
            {
                ConnectionCounter[dic_vRoom[SpanningTree[i].p0.Value]]++;
            }
            if (dic_vRoom[SpanningTree[i].p1.Value].AddRoomConnection(CreateRoomConnection(SpanningTree[i].p0.Value)))
            {
                ConnectionCounter[dic_vRoom[SpanningTree[i].p1.Value]]++;
            }
            
        }
        AddExtraConnection();
        //foreach (var kvp in ConnectionCounter)
        //{
        //    Debug.Log("Room: " + kvp.Key.ID + " Connections: " + kvp.Value);
        //}
    }

    /// <summary>
    /// 从三角剖分中返还一部分边给最小生成树，基于返还率
    /// </summary>
    private void AddExtraConnection()
    {
        List<int> range = new List<int>();
        for (int i = 0; i < DelaunayTriangulation.Count; i++)
        {
            range.Add(i);
        }

        for (int i = 0; i < (int)(DelaunayTriangulation.Count * RoomConnectionFrequency); i++)
        {
            int idx = Random.Range(0, range.Count);
            int value = range[idx];
            range.RemoveAt(idx);

            ////Create an index so we can keep track of the actual connections count of each main room
            //if (!ConnectionCounter.ContainsKey(dic_vRoom[DelaunayTriangulation[value].p0.Value]))
            //    ConnectionCounter.Add(dic_vRoom[DelaunayTriangulation[value].p0.Value], 0);
            //if (!ConnectionCounter.ContainsKey(dic_vRoom[DelaunayTriangulation[value].p1.Value]))
            //    ConnectionCounter.Add(dic_vRoom[DelaunayTriangulation[value].p1.Value], 0);

            //increment the counter

            if (dic_vRoom[DelaunayTriangulation[value].p0.Value].connections.Count<=MaximumConnectionCount)
            {
                continue;
            }

            if (dic_vRoom[DelaunayTriangulation[value].p0.Value].AddRoomConnection(CreateRoomConnection(DelaunayTriangulation[value].p1.Value)))
            {
                ConnectionCounter[dic_vRoom[DelaunayTriangulation[value].p0.Value]]++;
            }
            if (dic_vRoom[DelaunayTriangulation[value].p1.Value].AddRoomConnection(CreateRoomConnection(DelaunayTriangulation[value].p0.Value)))
            {
                ConnectionCounter[dic_vRoom[DelaunayTriangulation[value].p1.Value]]++;
            }
            
        }
    }

    /// <summary>
    /// 根据边关系为该对房间封装边信息
    /// </summary>
    /// <param name="p1">该房间所连接的房间的坐标</param>
    /// <returns></returns>
    private VirtualRoomConnection CreateRoomConnection(Vector2 p1)
    {
        VirtualRoom vRoom = dic_vRoom[p1];
        return new VirtualRoomConnection(vRoom);
    }
    private VirtualRoomConnection CreateRoomConnection(VirtualRoom vRoom)
    {
        return new VirtualRoomConnection(vRoom);
    }

    /// <summary>
    /// 将周围的房间按距离添加到该房间的集合之中
    /// </summary>
    /// <param name="i"></param>
    private void SetNeighbours(int i)
    {
        List<float> distanceList = new List<float>();
        foreach (var item in vRooms)
        {
            if (item != vRooms[i])
            {
                float distance = (vRooms[i].position - item.position).sqrMagnitude;
                bool inserted=false;
                if (vRooms[i].neighbors.Count > 0)
                {

                    for (int n = 0; n < distanceList.Count; n++)
                    {
                        if (distance < distanceList[n])
                        {
                            distanceList.Insert(n, distance);
                            vRooms[i].neighbors.Insert(n, item);
                            inserted = true;
                            break;
                        }
                    }
                    if (!inserted)
                    {
                        distanceList.Add(distance);
                        vRooms[i].neighbors.Add(item);
                    }
                }
                else
                {
                    distanceList.Add(distance);
                    vRooms[i].neighbors.Add(item);
                }
            }
        }
    }

    private void SupplementSide(VirtualRoom vRoom)
    {
        foreach (var connection in vRoom.connections)
        {
            if (vRoom.neighbors.Contains(connection.connectedRoom))
            {
                vRoom.neighbors.Remove(connection.connectedRoom);
            }
        }
        foreach (var item in UnChangeableRooms)
        {
            vRoom.neighbors.Remove(item);
        }
        vRoom.AddRoomConnection(CreateRoomConnection(vRoom.neighbors[0]));
        vRoom.neighbors[0].AddRoomConnection(CreateRoomConnection(vRoom));
    }

    //用该方法生成实体房间
    public void OnEntityRoomSpawn()
    {
        EntityParent = new GameObject("Entity Parent");
        foreach (var kvp in dic_vRoom)
        {
            EntityRoomGenerator.GenerateEntity(kvp.Value.transform.position, kvp.Value,dic_roomID,EntityParent);               
        }

        foreach (var kvp in dic_vRoom)
        {
            int roomId = kvp.Value.ID;
            for (int i = 0; i < kvp.Value.connections.Count; i++)
            {
                int targetID = kvp.Value.connections[i].connectedRoom.ID;
                EntityRoomGenerator.ConnectRoom(roomId, targetID,dic_roomID);
         
            }
        }
        foreach (var kvp in dic_roomID)
        {
            kvp.Value.InitLines();
        }
        foreach (var kvp in dic_roomID)
        {
            kvp.Value.ConnectMark();
        }

        SpawnAngelAndDevil();
        //根据词典生成实体房间

    }

    public void SpawnTreasureRoom(List<VirtualRoom> roomsWithOneConnection, List<VirtualRoom> roomsWithTwoConnection)
    {
        if (roomsWithOneConnection.Count>0)
        {
            int index = Random.Range(0, roomsWithOneConnection.Count);
            roomsWithOneConnection[index].roomType = RoomType.Treasure;
            //某些房型可能不能生成宝箱房
            UnChangeableRooms.Add(roomsWithOneConnection[index]);
            surplusVRoom.Remove(roomsWithOneConnection[index]);
            roomsWithOneConnection.RemoveAt(index);

        }
        else if (roomsWithTwoConnection.Count>0)
        {
            //从双边房集合中获取一房改变类型。
            int index = Random.Range(0, roomsWithTwoConnection.Count);
            roomsWithTwoConnection[index].roomType = RoomType.Treasure;

            //双边房随机删除该房一条边，同时需要将对方房的该边删除
            int pointToDelete = Random.Range(0, roomsWithTwoConnection[index].connections.Count);
            var deletedRoom = roomsWithTwoConnection[index].connections[pointToDelete].connectedRoom;
            var desConnection= deletedRoom.GetConnectedRoom(roomsWithTwoConnection[index]);

            roomsWithTwoConnection[index].connections.Remove(desConnection);
            roomsWithTwoConnection[index].connections.RemoveAt(pointToDelete);
            //可能发生BUG
            //双边房添加错误
            VirtualRoom result = GetSupplementRoom(roomsWithTwoConnection[index].connections[0].connectedRoom, deletedRoom);
            UnChangeableRooms.Add(roomsWithTwoConnection[index]);
            surplusVRoom.Remove(roomsWithTwoConnection[index]);
            SupplementSide(result);
            Debug.Log("Spawn Two side Treasure Room");
            roomsWithTwoConnection.RemoveAt(index);
            Check1_2PointLegality(roomsWithOneConnection, roomsWithTwoConnection, result);
        }

    }
    /// <summary>
    /// 生成天使恶魔房
    /// </summary>
    public void SpawnAngelAndDevil()
    {
        var angel=Instantiate(RoomStock.AngelRoom.EntityRooms[0],EntityParent.transform).GetComponent<Room>();
        angel.Init(RoomStock.AngelRoom.FixedRoomID, RoomType.Angel, RoomStock.AngelRoom.FixedPosition);
        var devil=Instantiate(RoomStock.DevilRoom.EntityRooms[0], EntityParent.transform).GetComponent<Room>();
        devil.Init(RoomStock.DevilRoom.FixedRoomID, RoomType.Devil, RoomStock.DevilRoom.FixedPosition);
        dic_roomID.Add(angel.roomId, angel);
        dic_roomID.Add(devil.roomId, devil);
    }

    public VirtualRoom GetSupplementRoom(VirtualRoom room1,VirtualRoom room2)
    {
        if (room1.connections.Count < room2.connections.Count)
        {
            return room1;
        }
        else if (room1.connections.Count > room2.connections.Count)
        {
            return room2;
        }
        else if (room1.connections.Count == room2.connections.Count)
        {
            var suppleIndex = Random.Range(0, 2);
            if (suppleIndex == 0)
            {
                return room1;
            }
            else
            {
                return room2;
            }
        }
        return null;
    }

    private void GenerateShop()
    {
        //设定生成概率
        List<VirtualRoom> availableRoom = new List<VirtualRoom>(surplusVRoom);
        availableRoom.Remove(end.connections[0].connectedRoom);
        if (start.connections.Count==1)
        {
            availableRoom.Remove(start.connections[0].connectedRoom);
        }
        if (availableRoom!=null)
        {
            int index=Random.Range(0, availableRoom.Count);
            availableRoom[index].roomType = RoomType.Shop;
            surplusVRoom.Remove(availableRoom[index]);
        }
    }

    /// <summary>
    /// 检测单边和双边房是否因为删边或增边发生改变，并根据改变情况移出相应的集合
    /// </summary>
    /// <param name="roomsWithOneConnection"></param>
    /// <param name="roomsWithTwoConnection"></param>
    /// <param name="vRoom"></param>
    private void Check1_2PointLegality(List<VirtualRoom> roomsWithOneConnection, List<VirtualRoom> roomsWithTwoConnection, VirtualRoom vRoom)
    {
        if (vRoom == null)
        {
            return;
        }
        if (roomsWithOneConnection.Contains(vRoom))
        {
            roomsWithOneConnection.Remove(vRoom);
        }
        else if (roomsWithTwoConnection.Contains(vRoom))
        {
            roomsWithTwoConnection.Remove(vRoom);
        }
    }

    public  bool IsRepetitive(int i, int j)
    {
        if (RoomConnectIDList.Count == 0)
        {
            return false;
        }
        int result1 = int.Parse(string.Format("{0}" + "{1}", i, j));
        int result2 = int.Parse(string.Format("{0}" + "{1}", j, i));
        return (RoomConnectIDList.Contains(result1) || RoomConnectIDList.Contains(result2));
    }

    public void AddConnectIDs(int i, int j)
    {
        int result = int.Parse(string.Format("{0}" + "{1}", i, j));
        RoomConnectIDList.Add(result);
    }



    private void OnDisable()
    {
        EventManager.current.OnEntityRoomGenerate -= OnEntityRoomSpawn;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MapManager : MonoBehaviour
{
    [SerializeField] Camera minimapCam;
    public Camera mapCam;
    [SerializeField] float y_standard;//默认的高度位置
    [SerializeField] RectTransform mapRect;
    [SerializeField] GameObject mapParent;
    Vector2 boundTop;
    Vector2 boundBottom;
    [SerializeField] LayerMask roomLayer;

    private void Awake()
    {
        minimapCam = GameObject.FindGameObjectWithTag("MiniMapCamera").GetComponent<Camera>();
        mapCam = GameObject.FindGameObjectWithTag("MapCamera").GetComponent<Camera>();
    }
    private void Start()
    {

    }
    public void UpdatePlayerPosition()
    {
        Room room = DungeonManager.Instance.room_currentPlayerPosIn;
        minimapCam.transform.position = new Vector3(room.transform.position.x, y_standard, room.transform.position.z);
    }



    public bool isInRect(Vector2 startPos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(mapRect, startPos); 
    }

    /// <summary>
    /// 移动地图相机达成移动地图
    /// </summary>
    /// <param name="startPos">相机移动前的位置</param>
    /// <param name="dir"></param>
    public void MoveMapCamera(Vector3 startPos,Vector3 dir)
    {
        mapCam.transform.position= new Vector3(startPos.x-dir.x, mapCam.transform.position.y, startPos.z-dir.z);
    }

    public Vector3 ConvertToWorldPosition(Vector3 mousePosition)
    {
        return mapCam.ScreenToWorldPoint(mousePosition);
    }

    public void EnableMap()
    {
        mapParent.SetActive(true);
    }

    public void DisableMap()
    {
        mapParent.SetActive(false);
    }

    public void DetectRoom(Vector2 mousePosition,bool select)
    {
        float mapX = mousePosition.x - Screen.width / 2;
        float mapY = mousePosition.y - Screen.height / 2;
        //Debug.Log(mapX + "And" + mapY);
        float realX = mapX / (mapRect.rect.width / 2) * mapCam.orthographicSize;
        float realY= mapY / (mapRect.rect.height/2)* mapCam.orthographicSize;
        Vector3 targetPoint = new Vector3(realX+ mapCam.transform.position.x, 0, realY+ mapCam.transform.position.z);
        Ray ray = new Ray(mapCam.transform.position, targetPoint - mapCam.transform.position);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.GetPoint(1000f));
        if (Physics.Raycast(ray, out hit,1000f,roomLayer))
        {
            var room =hit.collider.GetComponent<Room>();
            Debug.Log(room);
            if (room!=null&&select)
            {
                Debug.Log("Select");
                DungeonManager.Instance.UpdateCurrentRoomPlayerPosIn(room);
                PlayerManager.Instance.PrepareTeleporting();
                PlayerManager.Instance.TraverseOtherRoom(room.GetDestination());
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    public Image scope;
    MapManager mapManager;
    [SerializeField] GameObject portalSelect;
    private void Awake()
    {
        mapManager = GetComponent<MapManager>();
        EventManager.current.OnTransporterInteract += EnablePortalSelectUI;
        EventManager.current.OnPortalSelected += ClosePortalSelectUI;
    }

    private void OnDisable()
    {
        EventManager.current.OnTransporterInteract -= EnablePortalSelectUI;
        EventManager.current.OnPortalSelected -= ClosePortalSelectUI;
    }

    /// <summary>
    /// 可能以后会使用的接口用于替换瞄具样式，但考虑到可能会有动效的关系应该会采用别的方法
    /// </summary>
    /// <param name="sprite"></param>
    public void ReplaceScopeStyle(Sprite sprite)
    {
      
    }

    public void EnableScope()
    {
        scope.gameObject.SetActive(true);
    }

    public void DisableScope()
    {
        scope.gameObject.SetActive(false);
    }

    public void UpdatePlayerPositionInMinimap()
    {
        mapManager.UpdatePlayerPosition();
    }

    public bool isInRect(Vector2 startPos)
    {
        return mapManager.isInRect(startPos);
    }

    public void EnablePortalSelectUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        portalSelect.SetActive(true);       
    }

    /// <summary>
    /// 关闭弹窗
    /// </summary>
    public void ClosePopUpUI()
    {
        if (portalSelect.activeSelf)
        {
            portalSelect.SetActive(false);
        }

    }

    void ClosePortalSelectUI()
    {
        portalSelect.SetActive(false);
    }
}

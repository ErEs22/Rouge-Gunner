using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSelectWindow : MonoBehaviour
{
    [SerializeField] List<PortalSelectButton> selectButtons;

    private void Awake()
    {
        EventManager.current.OnTransporterInteract += InitPortalSelect;
    }

    private void OnDisable()
    {
        EventManager.current.OnTransporterInteract -= InitPortalSelect;
    }

    void InitPortalSelect()
    {
        var connections = DungeonManager.Instance.room_currentPlayerPosIn.connections;
        for (int i = 0; i < connections.Count; i++)
        {
            selectButtons[i].UpdateColor(connections[i].color);
            selectButtons[i].gameObject.SetActive(true);
        }
        foreach (var button in selectButtons)
        {
            if (button == false)
            {
                button.gameObject.SetActive(false);
            }
        }
    
    }
}

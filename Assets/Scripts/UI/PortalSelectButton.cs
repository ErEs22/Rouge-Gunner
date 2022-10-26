using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PortalSelectButton : MonoBehaviour
{
    [SerializeField] Image selectIcon;
    public bool active;
    [SerializeField] int index;
    private void OnDisable()
    {
        active = false;
    }
    public void UpdateColor(Color color)
    {
        active = true;
        selectIcon.color = color;
    }

    public void SelectPortal()
    {
        EventManager.current.PortalSelected();
        DungeonManager.Instance.ActivePortal(index);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WeaponWheelSlot : MonoBehaviour
{
    [SerializeField]
    private Image weaponIcon;
    [SerializeField]
    private Image ammotypeIcon;
    [SerializeField]
    private TextMeshProUGUI currentAmmoCarriedText;
    [SerializeField]
    private TextMeshProUGUI currentAmmoMaxCarriedText;

    [SerializeField]
    private Image checkedIcon;



    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private Vector3 originPos = new Vector3(0, 162.6f, 0);

    private void Start()
    {
    }
    public void HighlightSelectedWeapon()
    {
        checkedIcon.gameObject.SetActive(true);
        transform.localPosition += offset*2;
    }

    public void SelectSolt()
    {
        transform.localPosition += offset;
    }

    public void cancelSelect()
    {
        checkedIcon.gameObject.SetActive(false);
        transform.localPosition = originPos;
    }

    /// <summary>
    /// 更换该槽位的图标和背景
    /// </summary>
    /// <param name="icon"></param>
    /// <param name="background"></param>
    public void SwitchWeaponIconAndBackground(Sprite icon,Sprite background)
    {
        weaponIcon.sprite = icon;
        ammotypeIcon.sprite = background;
    }

    public void ClearSlotContent()
    {
        weaponIcon.sprite = null;
        ammotypeIcon.sprite = null;
    }

    /// <summary>
    /// First Update
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="weaponSO"></param>
    /// <param name="typeIcon"></param>
    public void FirstUpdateSlotContent(Weapon weapon,WeaponSO weaponSO,Sprite typeIcon)
    {
        weaponIcon.sprite = weaponSO.weaponIcon;
        ammotypeIcon.sprite = typeIcon;
        currentAmmoCarriedText.text = weapon.currentAmmoCarried.ToString();
        currentAmmoMaxCarriedText.text = weapon.currentMaxAmmoCarried.ToString();
    }

    public void UpdateSlotContent(Weapon weapon)
    {
        currentAmmoCarriedText.text = weapon.currentAmmoCarried.ToString();
        currentAmmoMaxCarriedText.text = weapon.currentMaxAmmoCarried.ToString();
    }

}

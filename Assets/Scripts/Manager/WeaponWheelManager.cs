using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponWheelManager : MonoBehaviour
{
    [Header("槽位预设")]
    [SerializeField] WeaponSlotIconSO weaponSlotIconSO;
    [SerializeField] List<WeaponWheelSlot> Slots;
    [SerializeField] List<Sprite> ammotypes;

    public Transform initialPositition;
    [SerializeField]
    private GameObject weaponWheel;

    [Header("当前武器信息")]
    public WeaponWheelSlot selectSlot;
    public WeaponWheelSlot equipedSlot;

    WeaponManager weaponManager;
    [SerializeField]
    CurrentWeaponAbilityInfo currentWeaponInfo;
    private void Awake()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
    }

    public void HoverSlot(int index)
    {
        if (Slots[index] != equipedSlot&&weaponManager.weaponList[index]!=null)
        {
            if (selectSlot == null)
            {
                Slots[index].SelectSolt();
                selectSlot = Slots[index];
            }
            else
            {
                selectSlot.cancelSelect();
                Slots[index].SelectSolt();
                selectSlot = Slots[index];
            }
            currentWeaponInfo.UpdateWeaponInfo(weaponManager.ExposeWeaponInfoToUI(index));
        }
    }


    /// <summary>
    /// 轮盘关闭时选择武器
    /// </summary>
    public void SelectSlot()
    {
        if (selectSlot != equipedSlot && selectSlot != null)
        {
            selectSlot.HighlightSelectedWeapon();
            if (equipedSlot != null)
            {
                equipedSlot.cancelSelect();
            }
            equipedSlot = selectSlot;
            weaponManager.SwitchWeapon(Slots.IndexOf(equipedSlot));
        }
        selectSlot = null;
    }


    public void OpenWeaponWheelUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Mouse.current.WarpCursorPosition(initialPositition.position);
        weaponWheel.SetActive(true);
        Cursor.visible = true;
    }

    internal void UpdateWheelSlot(Weapon weapon,WeaponSO weaponSO,int availableSlot)
    {
        Slots[availableSlot].FirstUpdateSlotContent(weapon,weaponSO, ammotypes[(int)weaponSO.ammoType]);
    }

    public void CloseWeaponWheelUI()
    {
        SelectSlot();
        weaponWheel.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CancelSelect()
    {
        if (selectSlot != null)
        {
            selectSlot.cancelSelect();
            selectSlot = null;
        }
    }


    /// <summary>
    /// 通过按键直接选择武器
    /// </summary>
    /// <param name="index"></param>
    public void ChooseSlotDirectly(int index)
    {
        CancelSelect();
        if (equipedSlot != null)
        {
            equipedSlot.cancelSelect();
        }
        equipedSlot = Slots[index];
        equipedSlot.HighlightSelectedWeapon();
    }


    public void UpdateSlotInfo(int index,Weapon weapon)
    {
        Slots[index].UpdateSlotContent(weapon);
    }
}

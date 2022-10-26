using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CurrentWeaponInfo : MonoBehaviour
{
    [SerializeField] Image weaponIcon;
    [SerializeField] TextMeshProUGUI currentAmmoInMag;
    [SerializeField] TextMeshProUGUI currentMagSize;

    public void UpdateAmmoInfo(int ammoInMag, int magSize)
    {
        currentAmmoInMag.text = ammoInMag.ToString();
        currentMagSize.text = magSize.ToString();
    }
}

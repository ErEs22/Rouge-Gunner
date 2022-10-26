using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CurrentWeaponAbilityInfo : MonoBehaviour
{
    [SerializeField] Image cW_AblityIcon;
    [SerializeField] TextMeshProUGUI cW_AblityName;
    [SerializeField] TextMeshProUGUI cW_AblityDescription;


    public void UpdateWeaponInfo(WeaponSO SelectWeapon)
    {
        cW_AblityIcon.sprite = SelectWeapon.abilityIcon;
        cW_AblityName.text = SelectWeapon.abilityName;
        cW_AblityDescription.text = SelectWeapon.abilityDescription;
    }
}

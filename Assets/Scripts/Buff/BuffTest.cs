using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTest : MonoBehaviour
{
    PlayerStatsManager playerStatsManager;

    PermanentlyBuff defaultBuff;

    PermanentlyBuff secondBuff;

    PersistentlyBuff thirdBuff;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    private void Start()
    {
        defaultBuff = BuffManager.Instance.CreatePermanentlyBuff(playerStatsManager, eBuffType.direct, eAffectAttribute.health, 20, 5);
        // secondBuff = BuffManager.Instance.CreatePermanentlyBuff(playerStatsManager, eBuffType.percent, eAffectAttribute.health, 0.2f, 10);
        // defaultBuff = BuffManager.Instance.CreatePermanentlyBuff(playerStatsManager, eBuffType.direct, eAffectAttribute.health, 20, 5);
        // secondBuff = BuffManager.Instance.CreatePermanentlyBuff(playerStatsManager, eBuffType.percent, eAffectAttribute.health, 0.2f, 10);
        Invoke(nameof(RemoveBuff), 7);
    }

    void RemoveBuff()
    {
        // BuffManager.Instance.RemovePermanentlyBuff(defaultBuff.index);
    }
}

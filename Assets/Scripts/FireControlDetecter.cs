using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControlDetecter : Singleton<FireControlDetecter>
{
    WeaponManager weaponManager;
    float lockTime;
    Dictionary<EnemyManager, float> dic_DetectTarget = new Dictionary<EnemyManager, float>();
    public List<TargetInfo_FCS> lockedTargetList;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var target = FindTarget(other);
            if (target != null)
            {
                dic_DetectTarget.Add(target, 0);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var target = FindTarget(other);

            if (target != null && dic_DetectTarget.ContainsKey(target))
            {
                if (dic_DetectTarget[target] < lockTime)
                {
                    dic_DetectTarget[target] += Time.deltaTime;

                    if (dic_DetectTarget[target] >= lockTime)
                    {
                        lockedTargetList.Add(new TargetInfo_FCS(target, (target.transform.position - weaponManager.FpsCam.transform.position).sqrMagnitude));
                    }
                }
                else if (dic_DetectTarget[target] >= lockTime)
                {
                    var detectTagrt = GetTargetInfo(target);
                    detectTagrt.distance = (detectTagrt.enemy.transform.position - weaponManager.FpsCam.transform.position).sqrMagnitude;
                    //求距离集合
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var target = other.GetComponent<EnemyManager>();
            if (target != null && dic_DetectTarget.ContainsKey(target))
            {
                dic_DetectTarget.Remove(target);
                lockedTargetList.Remove(GetTargetInfo(target));
            }
        }
    }

    /// <summary>
    /// 获取mesh检测到的目标并检测是否被阻挡 被阻挡则会返回Null
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private EnemyManager FindTarget(Collider other)
    {
        RaycastHit hit;
        Physics.Raycast(weaponManager.FpsCam.transform.position, other.transform.position - weaponManager.FpsCam.transform.position, out hit, weaponManager.maxViewDistance, weaponManager.checkLayer);
        var target = hit.collider.GetComponent<EnemyManager>();
        return target;

    }

    /// <summary>
    /// 获取单个被锁定的敌人
    /// </summary>
    /// <returns></returns>
    public TargetInfo_FCS GetTarget()
    {
        return lockedTargetList[0];
    }

    /// <summary>
    /// 获取已经被锁定的多个敌人
    /// </summary>
    /// <param name="amount">获取的敌人数量</param>
    /// <returns></returns>
    public List<TargetInfo_FCS> GetTargets(int amount)
    {
        int count = amount;
        List<TargetInfo_FCS> result = new List<TargetInfo_FCS>();
        if (amount > lockedTargetList.Count)
        {
            count = lockedTargetList.Count;
        }
        for (int i = 0; i < count; i++)
        {
            result.Add(lockedTargetList[i]);
        }
        return result;
    }

    private TargetInfo_FCS GetTargetInfo(EnemyManager target)
    {
        foreach (var item in lockedTargetList)
        {
            if (item.enemy == target)
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// 快速排序由距离从近到远排序
    /// </summary>
    /// <param name="targetInfo_FCs"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void SortTargets(List<TargetInfo_FCS> targetInfo_FCs, int start, int end)
    {
        if (start < end)
        {
            int mid = Paritition(targetInfo_FCs, start, end);
            SortTargets(targetInfo_FCs, start, mid - 1);
            SortTargets(targetInfo_FCs, mid + 1, end);
        }
    }

    /// <summary>
    /// 分治方法，把数组放置在确定的位置
    /// </summary>
    /// <param name="targetInfo_FCs"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    int Paritition(List<TargetInfo_FCS> targetInfo_FCs, int start, int end)
    {
        TargetInfo_FCS pivot = targetInfo_FCs[end];
        int j = start;
        for (int i = start; i < end; i++)
        {
            if (targetInfo_FCs[i].distance < pivot.distance)
            {
                TargetInfo_FCS temp = targetInfo_FCs[i];
                targetInfo_FCs[i] = targetInfo_FCs[j];
                targetInfo_FCs[j] = temp;
                j++;
            }
        }
        targetInfo_FCs[end] = targetInfo_FCs[j];
        targetInfo_FCs[j] = pivot;
        return j;
    }

    public void ClearTargets()
    {
        dic_DetectTarget.Clear();
        lockedTargetList.Clear();
    }
}

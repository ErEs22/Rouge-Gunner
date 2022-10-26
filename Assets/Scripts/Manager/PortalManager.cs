using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : Singleton<PortalManager>
{
    public Dictionary<int, Portal> portalDic = new Dictionary<int, Portal>();
    /// <summary>
    /// 将传送目标传送至指定ID传送门的位置
    /// </summary>
    /// <param name="transportTarget">传送目标</param>
    /// <param name="destinationID">目标传送门ID</param>
    //public void Transport(GameObject transportTarget, int destinationID)
    //{
    //    Portal desPortal = null;
    //    portalDic.TryGetValue(destinationID, out desPortal);
    //    transportTarget.transform.position = desPortal.landingTrans.position;
    //}
}

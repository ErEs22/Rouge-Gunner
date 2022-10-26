using System.Collections.Generic;

/// <summary>
/// 该记录器用于记录根据
/// 未来修改到武器上，道具不适用
/// </summary>
public class EntityRecorder_Inventory 
{
    public List<Derivative_Inventory> entities;


    public void Add(int count, Derivative_Inventory entity)
    {
        if (!entities.Contains(entity))
        {
            entities.Add(entity);
        }
    }
    /// <summary>
    /// 从记录器中删除该道具并从集合中删除该记录器
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public void Remove()
    {
        if (entities!=null)
        {
            foreach (var item in entities)
            {
                item.DestroyUnit();
            }
        }
    }
}

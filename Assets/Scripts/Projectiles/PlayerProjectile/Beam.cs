using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : BaseProjectile
{
    public EnemyManager target;
    protected override void Move()
    {
        if (target == null)
        {
            Vector3 dir = target.transform.position - transform.position;
            transform.forward = Vector3.Slerp(transform.forward, dir,
                    15 * Time.deltaTime);
        }
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
    }

  
}

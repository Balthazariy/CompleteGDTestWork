using System.Collections;
using System.Collections.Generic;
using Test.Enemies.Base;
using UnityEngine;

namespace Test.Enemies
{
    public class DuplicatorEnemy : EnemyBase
    {
        protected override void Die()
        {
            base.Die();

            OnDuplicatorDeathEvent?.Invoke(transform.localPosition);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void TakeDamage(float damage);
    void GetAttackTarget(GameObject target);
    void ActivateStampAttack();
    void Stun(float stunDuration);
    void ModifySpeeds(float movementModifier, float moveDuration = 0, float attackSpeedModifier = 0, float attackDuration = 0);
    void SetLane(int laneNumber);
}

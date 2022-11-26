using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void TakeDamage(float damage);
    void GetAttackTarget(GameObject target);
    void ActivateStampAttack();
    void Stun(float stunDuration);
    //How much to slow (slow multiplier), how long the slow is (0 = infinite), how much to slow attack speed by (atk speed multiplier),
    //how long the attack speed slow is (0 = infinite) 
    void ModifySpeeds(float movementModifier, float moveDuration = 0, float attackSpeedModifier = 0, float attackDuration = 0);
    void SetLane(int laneNumber);
    int GetPlayerHealthDamage();
    void ForcedMove(Vector3 startPos, Vector3 endPos, float forcedMoveSpeed);
}

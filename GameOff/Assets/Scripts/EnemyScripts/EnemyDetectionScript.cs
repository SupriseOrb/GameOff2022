using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.TryGetComponent(out IItemStamp item))
        {
            Debug.Log("Enemy attacking item stamp!");
            GetComponentInParent<IEnemy>().GetAttackTarget(other.gameObject);
        }
    }
}

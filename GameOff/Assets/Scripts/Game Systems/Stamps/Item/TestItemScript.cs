using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemScript : MonoBehaviour , IItemStamp
{
    [SerializeField] private float _itemHealth = 3;
    [SerializeField] private Transform _itemTransform;
    // Start is called before the first frame update
    void Start()
    {
        _itemTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        _itemHealth -= damage;
        _itemTransform.localScale = new Vector3(_itemTransform.localScale.x, _itemTransform.localScale.y + 2, _itemTransform.localScale.z);
        if(_itemHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void HealHealth(float health)
    {

    }

    public void ActivateStampAbility()
    {
    
    }

    public void SetLane(int laneNumber)
    {
        
    }
    
    public string GetStampName()
    {
        return "";
    }

    public bool IsDead()
    {
        return false;
    }

    public void EnableStamp()
    {

    }

    public void DisableStamp()
    {

    }
}

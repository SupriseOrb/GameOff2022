using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkReservoirLand : MonoBehaviour, ILandStamp
{
    [SerializeField] private LandStampScriptableObject _reservoirSO;
    
    [Header("Debug Values")]
    [SerializeField] private int _laneNumber;
    [SerializeField] private float _reservoirHeal;
    [SerializeField] private float _baseHealCooldown;
    [SerializeField] private float _currentHealCooldown;
    [SerializeField] private List<GameObject> _laneItems;

    // Start is called before the first frame update
    void Start()
    {
        LoadBaseStats();
        AkSoundEngine.PostEvent("Play_InkReservoir", gameObject);
    }

    private void LoadBaseStats()
    {
        _reservoirHeal = _reservoirSO.LandAbilityValue;
        _baseHealCooldown = _reservoirSO.LandAbilityCooldown;
        _currentHealCooldown = _baseHealCooldown;
    }

    private void FixedUpdate() 
    {
        _currentHealCooldown -= Time.deltaTime;
        if(_currentHealCooldown < 0)
        {
            ActivateStampAbility();
            _currentHealCooldown = _baseHealCooldown;
        }
    }
    public void ActivateStampAbility()
    {
        _laneItems = BoardManager.Instance.GetLane(_laneNumber).GetLaneItems();
        foreach(GameObject item in _laneItems)
        {
            if(item != null)
            {
                item.GetComponent<IItemStamp>().HealHealth(_reservoirHeal);
            }
        }
        //AkSoundEngine.PostEvent("Play_StampGeneral", gameObject);
    }

    public void SetLane(int laneNumber)
    {
        _laneNumber = laneNumber;
    }

    public string GetStampName()
    {
        return _reservoirSO.StampName;
    }

    public void EnableStamp()
    {

    }

    public void DisableStamp()
    {

    }
}

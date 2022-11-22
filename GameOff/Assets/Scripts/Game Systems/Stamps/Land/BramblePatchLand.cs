using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BramblePatchLand : MonoBehaviour, ILandStamp
{
    [SerializeField] private LandStampScriptableObject _brambleSO;
    
    [Header("Debug Values")]
    [SerializeField] private int _laneNumber;
    [SerializeField] private float _brambleDamage;
    [SerializeField] private float _baseDamageCooldown;
    [SerializeField] private float _currentDamageCooldown;
    [SerializeField] private List<GameObject> _laneEnemies;

    // Start is called before the first frame update
    void Start()
    {
        LoadBaseStats();   
    }

    private void LoadBaseStats()
    {
        _brambleDamage = _brambleSO.StampAbilityValue;
        _baseDamageCooldown = _brambleSO.StampAbilityCooldown;
        _currentDamageCooldown = _baseDamageCooldown;
    }

    private void FixedUpdate() 
    {
        _currentDamageCooldown -= Time.deltaTime;
        if(_currentDamageCooldown < 0)
        {
            ActivateStampAbility();
            _currentDamageCooldown = _baseDamageCooldown;
        }
    }
    public void ActivateStampAbility()
    {
        _laneEnemies = BoardManager.Instance.GetLane(_laneNumber).GetLaneEnemies();
        for(int i = _laneEnemies.Count - 1; i >= 0; i--)
        {
            _laneEnemies[i].GetComponent<IEnemy>().TakeDamage(_brambleDamage);
        }
                    AkSoundEngine.PostEvent("Play_StampGeneral", gameObject);
    }

    public void SetLane(int laneNumber)
    {
        _laneNumber = laneNumber;
    }

    public string GetStampName()
    {
        return _brambleSO.StampName;
    }

    public void EnableStamp()
    {

    }

    public void DisableStamp()
    {

    }
}

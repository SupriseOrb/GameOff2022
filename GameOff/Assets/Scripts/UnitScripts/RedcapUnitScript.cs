using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedcapUnitScript : MonoBehaviour, IUnitStamp
{
    [SerializeField] private UnitStampScriptableObject _redcapSO;
    [SerializeField] private bool _isActive = false;
    [SerializeField] private float _redcapAttackCooldown;

#region UnitStats
    [SerializeField] private string _redcapName;
    [SerializeField] private string _unitType;
    [SerializeField] private float _redcapDamage;
    [SerializeField] private float _redcapAttackSpeed;
    [SerializeField] private float _unitSlowAmount;
    [SerializeField] private int _redcapPierceAmount;
    [SerializeField] private float _redcapStunDuration;
#endregion

#region UnitVariables
    [SerializeField] private GameObject _inkProjectile;
    [SerializeField] private Transform _inkProjectileSpawnLocation;
    [SerializeField] private Animator _redcapAnimator;
    [SerializeField] private string _redcapAttackAnimationName = "Redcap_Attack";
    [SerializeField] private string _redcapAppearAnimationName = "Redcap_Appear";
    //Just in case we need this value (unlikely for redcap but necessary for ink demon)
    [SerializeField] private int _redcapLane;
#endregion

    public enum TestUnitUpgradePaths
    {
        upgradeBase = 0,
        upgradeOne = 1,
        upgradeTwo = 2,
        upgradeThree = 3
    }
    public TestUnitUpgradePaths _currentUpgradePath = TestUnitUpgradePaths.upgradeBase;

    // Start is called before the first frame update
    void Start()
    {
        LoadBaseStats();
        _redcapAttackCooldown = 1 / _redcapAttackSpeed;
        _redcapAnimator.Play(_redcapAppearAnimationName);
    }

    public void LoadBaseStats()
    {
        //Test variables:
        if (_currentUpgradePath == TestUnitUpgradePaths.upgradeThree)
        {
            _redcapStunDuration = 3;
        }
        
        _redcapName = _redcapSO.StampName;
        _unitType = _redcapSO.UnitType;
        _redcapDamage = _redcapSO.UnitDamage;
        _redcapAttackSpeed = _redcapSO.UnitAttackSpeed;
        _unitSlowAmount = _redcapSO.UnitSlowAmount;
    }

    public void SetLane(int lane)
    {
        _redcapLane = lane;
    }

    private void FixedUpdate() 
    {
        if(_isActive)
        {
            if(_redcapAttackCooldown <= 0)
            {
                ActivateStampAttack();
                _redcapAttackCooldown = 1 / _redcapAttackSpeed;
            }
            _redcapAttackCooldown -= Time.deltaTime;
        }
    }

    public void UpgradeUnit()
    {
        //bring up the upgrade menu I think

        //Note: Cap Atk Speed at 1.0 (matches animation length)
        //Random Upgradable Stats: Attack, Attack Speed, Pierce Amt/Stun Duration (depending on Upgrade path)
    }

#region Ability Functions
    public void ActivateStampAbility()
    {
        switch(_currentUpgradePath)
        {
            case TestUnitUpgradePaths.upgradeOne:
                UpgradeOneAbilityHelper();
                break;
            case TestUnitUpgradePaths.upgradeTwo:
                UpgradeTwoAbilityHelper();
                break;
            case TestUnitUpgradePaths.upgradeThree:
                UpgradeThreeAbilityHelper();
                break;
            default:
                UpgradeBaseAbilityHelper();
                break;
        }
    }

    private void UpgradeBaseAbilityHelper()
    {

    }

    private void UpgradeOneAbilityHelper()
    {
        
    }

    private void UpgradeTwoAbilityHelper()
    {
        
    }

    private void UpgradeThreeAbilityHelper()
    {
        
    }
#endregion
    
    public void ActivateStampAttack()
    {
        // Do the attack animation
        _redcapAnimator.Play(_redcapAttackAnimationName);
        // Do the attack
        GameObject inkball = Instantiate(_inkProjectile, _inkProjectileSpawnLocation.position, Quaternion.identity);
        //Consider just declaring the inkballScript
        RedcapInkBallProjectile inkballScript = inkball.GetComponent<RedcapInkBallProjectile>();
        inkballScript.SetDamage(_redcapDamage);
        switch(_currentUpgradePath)
        {
            /*
            For pierce, maybe have a raycast shoot out when the inkball hits the 1st enemy and check for other enemies
            If they exist, deal x% of the initial hit to them?
            */
            case TestUnitUpgradePaths.upgradeTwo:
                inkballScript.SetPiercing(_redcapPierceAmount);
                break;
            /*
            For stun, have the enemy hit have their velocity set to 0 for x amount of time
            */
            case TestUnitUpgradePaths.upgradeThree:
                inkballScript.SetStunValues(true, _redcapStunDuration);
                break;
            default:
                break;
        }
    }

    public void DisableStamp()
    {
        _isActive = false;
    }

    public void EnableStamp()
    {
        _isActive = true;
    }

    public string GetUnitName()
    {
        return _redcapName;
    }
}

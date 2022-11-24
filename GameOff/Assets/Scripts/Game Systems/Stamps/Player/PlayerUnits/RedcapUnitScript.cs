using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedcapUnitScript : MonoBehaviour, IUnitStamp
{
    [SerializeField] private UnitStampScriptableObject _redcapSO;
    [SerializeField] private bool _isActive = false;
    [SerializeField] private float _redcapAttackCooldown;

#region UnitStats
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
    [SerializeField] private string _redcapAppearAnimationName = "Redcap_Red_Appear";
    [SerializeField] private string _bluecapAppearAnimationName = "Redcap_Blue_Appear";
    [SerializeField] private string _purplecapAppearAnimationName = "Redcap_Purple_Appear";
    [SerializeField] private string _redcapAttackAnimationName = "Redcap_Red_Attack";
    [SerializeField] private string _bluecapAttackAnimationName = "Redcap_Blue_Attack";
    [SerializeField] private string _purplecapAttackAnimationName = "Redcap_Purple_Attack";
    
    //Just in case we need this value (unlikely for redcap but necessary for ink demon)
    [SerializeField] private int _redcapLaneNumber;
#endregion

    public enum RedcapUpgradePaths
    {
        //Red
        upgradeBase = 0,
        //Blue
        upgradeOne = 1,
        //Purple
        upgradeTwo = 2,
    }
    public RedcapUpgradePaths _currentUpgradePath = RedcapUpgradePaths.upgradeBase;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_StampGeneral", gameObject);
        LoadBaseStats();
        _redcapAttackCooldown = 1 / _redcapAttackSpeed;
        _redcapAnimator.Play(_redcapAppearAnimationName);
    }

    public void LoadBaseStats()
    {
        //Test variables:
        _redcapStunDuration = 3;
        _redcapPierceAmount = 1;

        _unitType = _redcapSO.UnitType;
        _redcapDamage = _redcapSO.UnitDamage;
        _redcapAttackSpeed = _redcapSO.UnitAttackSpeed;
        _unitSlowAmount = _redcapSO.UnitSlowAmount;
    }

    public void SetLane(int lane)
    {
        _redcapLaneNumber = lane;
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

    public void OpenUnitUpgrade()
    {
        //Opens the upgrade UI
    }

    public void UpgradeUnit(int upgradePath)
    {
        //bring up the upgrade menu I think
        if(upgradePath == (int)_currentUpgradePath)
        {
            //Note: Cap Atk Speed at 1.0 (matches animation length) or increase anim speed
            //Random Upgradable Stats: Attack, Attack Speed, Pierce Amt/Stun Duration (depending on Upgrade path)
        }
        else
        {
            if(upgradePath == (int)RedcapUpgradePaths.upgradeOne)
            {
                _currentUpgradePath = RedcapUpgradePaths.upgradeOne;
                _redcapAnimator.Play(_bluecapAppearAnimationName);
            }
            else
            {
                _currentUpgradePath = RedcapUpgradePaths.upgradeTwo;
                _redcapAnimator.Play(_purplecapAppearAnimationName);
            }
        }
        
    }

    public void ReduceCooldown(float reductionAmount)
    {
        _redcapAttackCooldown -= reductionAmount;
    }

#region Ability Functions
    public void ActivateStampAbility()
    {
        switch(_currentUpgradePath)
        {
            case RedcapUpgradePaths.upgradeOne:
                break;
            case RedcapUpgradePaths.upgradeTwo:
                break;
            default:
                break;
        }
    }
#endregion
    
    public void ActivateStampAttack()
    {
        if(_redcapAttackSpeed > 1)
        {
            _redcapAnimator.speed = _redcapAttackSpeed;
        }
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
            case RedcapUpgradePaths.upgradeOne:
                // Do the attack animation
                _redcapAnimator.Play(_bluecapAttackAnimationName);
                inkballScript.SetSprite((int)RedcapUpgradePaths.upgradeOne);
                inkballScript.SetPiercing(_redcapPierceAmount);
                break;
            /*
            For stun, have the enemy hit have their velocity set to 0 for x amount of time
            */
            case RedcapUpgradePaths.upgradeTwo:
                // Do the attack animation
                _redcapAnimator.Play(_purplecapAttackAnimationName);
                inkballScript.SetSprite((int)RedcapUpgradePaths.upgradeTwo);
                inkballScript.SetStunValues(true, _redcapStunDuration);
                break;
            default:
                // Do the attack animation
                _redcapAnimator.Play(_redcapAttackAnimationName);
                inkballScript.SetSprite((int)RedcapUpgradePaths.upgradeBase);
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

    public string GetStampName()
    {
        return _redcapSO.StampName;
    }
}

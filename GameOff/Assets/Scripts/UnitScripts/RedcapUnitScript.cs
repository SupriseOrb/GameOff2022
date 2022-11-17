using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedcapUnitScript : MonoBehaviour, IUnitStamp
{
    [SerializeField] private UnitStampScriptableObject _redcapSO;
    [SerializeField] private bool _isActive = false;
    [SerializeField] private float _redcapAttackCooldown;

#region UnitStats
    [SerializeField] private string _unitName;
    [SerializeField] private string _unitType;
    [SerializeField] private float _unitDamage;
    [SerializeField] private float _redcapAttackSpeed;
    [SerializeField] private float _unitSlowAmount;
#endregion

#region UnitVariables
    [SerializeField] private GameObject _inkProjectile;
    [SerializeField] private Transform _inkProjectileSpawnLocation;
    [SerializeField] private Animator _redcapAnimator;
    [SerializeField] private string _redcapAttackAnimationName = "Redcap_Attack";
    [SerializeField] private string _redcapAppearAnimationName = "Redcap_Appear";
#endregion

    public enum TestUnitUpgradePaths
    {
        upgradeBase = 0,
        upgradeOne = 1,
        upgradeTwo = 2,
        upgradeThree = 3
    }
    public TestUnitUpgradePaths currentUpgradePath = TestUnitUpgradePaths.upgradeBase;

    // Start is called before the first frame update
    void Start()
    {
        LoadBaseStats();
        _redcapAttackCooldown = 1 / _redcapAttackSpeed;
        _redcapAnimator.Play(_redcapAppearAnimationName);
    }

    public void LoadBaseStats()
    {
        _unitName = _redcapSO.StampName;
        _unitType = _redcapSO.UnitType;
        _unitDamage = _redcapSO.UnitDamage;
        _redcapAttackSpeed = _redcapSO.UnitAttackSpeed;
        _unitSlowAmount = _redcapSO.UnitSlowAmount;
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
    }

#region Ability Functions
    public void ActivateStampAbility()
    {
        switch(currentUpgradePath)
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
        RedcapInkBallProjectile inkballScript = inkball.GetComponent<RedcapInkBallProjectile>();
        inkballScript.SetDamage(_unitDamage);
        switch(currentUpgradePath)
        {
            case TestUnitUpgradePaths.upgradeTwo:
                inkballScript.SetPiercing(1);
                break;
            case TestUnitUpgradePaths.upgradeThree:
                inkballScript.SetStun(true);
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
        return _unitName;
    }
}

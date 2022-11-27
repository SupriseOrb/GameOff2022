using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDemonUnitScript : MonoBehaviour, IUnitStamp
{
    [SerializeField] private UnitStampScriptableObject _inkDemonSO;
    [SerializeField] private CardScriptableObject _inkDemonCardSO;
    [SerializeField] private bool _isActive = false;

#region UnitStats
    [SerializeField] private string _unitType;
    [SerializeField] private float _inkDemonAbilityCooldown;
    [SerializeField] private float _inkDemonCooldownReduction;

    [Header("Ink Minion Stats")]
    [SerializeField] private  GameObject _inkMinionPrefab;
    [SerializeField] private int _inkMinionHealth;
    [SerializeField] private float _inkMinionDamage;
    [SerializeField] private float _inkMinionAttackSpeed;
    [SerializeField] private int _inkMinionDeathDamageMultiplier;
    [SerializeField] private float _inkMinionSlowAmount;
    [SerializeField] private float _inkMinionSlowDuration;
    
#endregion

#region UnitVariables
    [Header("Unit Variables")]
    [SerializeField] private BoardTile _summonTile;
    [SerializeField] private float _currentInkDemonAbilityCooldown;
    //[SerializeField] private Transform _inkProjectileSpawnLocation;
    [SerializeField] private Animator _inkDemonAnimator;
    [SerializeField] private string _inkDemonGreenAttackAnimationName = "InkDemon_Green_Attack";
    [SerializeField] private string _inkDemonGreenAppearAnimationName = "InkDemon_Green_Appear";
    [SerializeField] private string _inkDemonBlueAttackAnimationName = "InkDemon_Blue_Attack";
    [SerializeField] private string _inkDemonBlueAppearAnimationName = "InkDemon_Blue_Appear";
    [SerializeField] private string _inkDemonRedAttackAnimationName = "InkDemon_Red_Attack";
    [SerializeField] private string _inkDemonRedAppearAnimationName = "InkDemon_Red_Appear";
    [SerializeField] private float _inkMinionAttackAnimationLength;
    //Just in case we need this value (unlikely for redcap but necessary for ink demon)
    [SerializeField] private int _inkDemonLaneNumber;
    [SerializeField] private List<InkDemonMinion> _activeMinions;
#endregion

#region Unit Upgrade Values
    [SerializeField] private float _attackDamageUpgradeIncrease;
    [SerializeField] private float _attackSpeedUpgradeIncrease;
    [SerializeField] private int _maxHealthUpgradeIncrease;
    [SerializeField] private int _cooldownReductionUpgrade;
    [SerializeField] private int _cooldownIncreaseUpgrade;
#endregion

    public enum InkDemonUpgradePaths
    {
        upgradeBase = 0,
        upgradeVolatileSummons = 1,
        upgradeMassProducedSummons = 2,
    }
    public InkDemonUpgradePaths _currentUpgradePath = InkDemonUpgradePaths.upgradeBase;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_StampGeneral", gameObject);
        LoadBaseStats();
        LoadUpgradeStats();
        _inkDemonAnimator.Play(_inkDemonGreenAppearAnimationName);
    }

    public void LoadBaseStats()
    {        
        _unitType = _inkDemonSO.UnitType;
        _inkDemonAbilityCooldown = _inkDemonSO.UnitAbilityCooldown;
        _inkDemonCooldownReduction = _inkDemonSO.UnitCooldownReduction;
        
        _inkMinionPrefab = _inkDemonSO.SpawnedUnit;
        _inkMinionHealth = _inkDemonSO.UnitHealth;
        _inkMinionAttackSpeed = _inkDemonSO.UnitAttackSpeed;
        _inkMinionDamage = _inkDemonSO.UnitDamage;
    }

    public void LoadUpgradeStats()
    {
        _attackDamageUpgradeIncrease = _inkDemonSO.AttackDamageIncreaseAmount;
        _attackSpeedUpgradeIncrease = _inkDemonSO.AttackSpeedIncreaseAmount;
        _maxHealthUpgradeIncrease = (int)_inkDemonSO.UniqueUpgradeOneIncreaseAmount;
        _cooldownIncreaseUpgrade = (int)_inkDemonSO.UniqueUpgradeTwoIncreaseAmount;
        _cooldownReductionUpgrade = (int)_inkDemonSO.UniqueUpgradeThreeIncreaseAmount;
    }

    public void SetLane(int lane)
    {
        _inkDemonLaneNumber = lane;
    }

    private void FixedUpdate() 
    {
        if(_isActive)
        {
            if(_currentInkDemonAbilityCooldown <= 0)
            {
                ActivateStampAbility();
                //calculate cooldown based on cdr
                _currentInkDemonAbilityCooldown = _inkDemonAbilityCooldown * (1 - (_inkDemonCooldownReduction/100));
            }
            _currentInkDemonAbilityCooldown -= Time.deltaTime;
        }
    }

    public void OpenUnitUpgrade()
    {
        UpgradeMenu.Instance.Open(gameObject, _inkDemonCardSO.CardIcon, _inkDemonSO.Upgrades, (int)_currentUpgradePath);
    }

    public void UpgradeUnit(int upgradePath)
    {
        if(upgradePath == (int)_currentUpgradePath)
        {
            Debug.Log("Upgrading Random Stat");
            int upgradedStat = Random.Range(0, 3);
            switch (upgradedStat)
            {
                case 0:
                    _inkMinionDamage += _attackDamageUpgradeIncrease;
                    break;
                case 1:
                    _inkMinionAttackSpeed += _attackSpeedUpgradeIncrease;
                    break;
                default:
                    _inkMinionHealth += _maxHealthUpgradeIncrease;
                    break;
            }
            //Note: Cap Atk Speed at 1.0 (matches animation length) or increase anim speed
            //Random Upgradable Stats: Attack, Attack Speed, Pierce Amt/Stun Duration (depending on Upgrade path)
        }
        else
        {
            if(upgradePath == (int)InkDemonUpgradePaths.upgradeVolatileSummons)
            {
                LoadBaseStats();
                _inkMinionDeathDamageMultiplier = 4;
                _inkMinionSlowAmount = .5f;
                _inkMinionSlowDuration = 1f;
                _inkDemonCooldownReduction = _cooldownIncreaseUpgrade;

                _currentUpgradePath = InkDemonUpgradePaths.upgradeVolatileSummons;
                _inkDemonAnimator.Play(_inkDemonBlueAppearAnimationName);
            }
            else
            {
                LoadBaseStats();
                _inkMinionDeathDamageMultiplier = 0;
                _inkMinionSlowAmount = 0;
                _inkMinionSlowDuration = 0;
                _inkDemonCooldownReduction = _cooldownReductionUpgrade;
                
                _currentUpgradePath = InkDemonUpgradePaths.upgradeMassProducedSummons;
                _inkDemonAnimator.Play(_inkDemonRedAppearAnimationName);
            }
            //Random Upgradable Stats: Minion Attack, Minion Attack Speed, Ink Minion Health, Slow Duration(depending on upgrade path)
        }

        foreach(InkDemonMinion inkMinion in _activeMinions)
        {
            inkMinion.UpdateMinionStats(_currentUpgradePath, _inkMinionHealth, _inkMinionAttackSpeed, _inkMinionDamage, _inkMinionDeathDamageMultiplier, _inkMinionSlowAmount, _inkMinionSlowDuration, _inkDemonLaneNumber);
        }
    }

    public void ReduceCooldown(float reductionAmount)
    {
        _currentInkDemonAbilityCooldown -= reductionAmount;
    }

#region Ability Functions
    public void ActivateStampAbility()
    {
        for(int i = _activeMinions.Count - 1; i >= 0; i--)
        {
            if(_activeMinions[i] == null)
            {
                _activeMinions.RemoveAt(i);
            }
        }
        switch(_currentUpgradePath)
        {
            case InkDemonUpgradePaths.upgradeVolatileSummons:
                UpgradeOneAbilityHelper();
                break;
            case InkDemonUpgradePaths.upgradeMassProducedSummons:
                UpgradeTwoAbilityHelper();
                break;
            default:
                UpgradeBaseAbilityHelper();
                break;
        }
    }

    private void UpgradeBaseAbilityHelper()
    {
        _summonTile = BoardManager.Instance.GetLane(_inkDemonLaneNumber).GetNearestFreeTile();
        if(_summonTile != null)
        {
            _summonTile.SetHeldStamp(_inkMinionPrefab);
            _activeMinions.Insert(0, _summonTile.GetHeldStamp().GetComponent<InkDemonMinion>());
            _activeMinions[0].UpdateMinionStats(_currentUpgradePath, _inkMinionHealth, _inkMinionAttackSpeed, _inkMinionDamage, laneNumber: _inkDemonLaneNumber); //Set its stats based on its upgrades
            _inkDemonAnimator.Play(_inkDemonGreenAttackAnimationName);
        }
    }

    private void UpgradeOneAbilityHelper()
    {
        _summonTile = BoardManager.Instance.GetLane(_inkDemonLaneNumber).GetNearestFreeTile();
        if(_summonTile != null)
        {
            _summonTile.SetHeldStamp(_inkMinionPrefab);
            _activeMinions.Insert(0, _summonTile.GetHeldStamp().GetComponent<InkDemonMinion>());
            _activeMinions[0].UpdateMinionStats(_currentUpgradePath, _inkMinionHealth, _inkMinionAttackSpeed, _inkMinionDamage, 4, 5, _inkDemonLaneNumber); //Set its stats based on its upgrades
            _inkDemonAnimator.Play(_inkDemonBlueAttackAnimationName);
        }
    }

    private void UpgradeTwoAbilityHelper()
    {
        _summonTile = BoardManager.Instance.GetLane(_inkDemonLaneNumber).GetNearestFreeTile();
        if(_summonTile != null)
        {
            _summonTile.SetHeldStamp(_inkMinionPrefab);
            _activeMinions.Insert(0, _summonTile.GetHeldStamp().GetComponent<InkDemonMinion>());
            _activeMinions[0].UpdateMinionStats(_currentUpgradePath, _inkMinionHealth, _inkMinionAttackSpeed, _inkMinionDamage, laneNumber: _inkDemonLaneNumber); //Set its stats based on its upgrades
            _inkDemonAnimator.Play(_inkDemonRedAttackAnimationName);
        }
    }

#endregion
    public void ActivateStampAttack()
    {
    }
    /*
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
            //For pierce, maybe have a raycast shoot out when the inkball hits the 1st enemy and check for other enemies
            //If they exist, deal x% of the initial hit to them?
            case TestUnitUpgradePaths.upgradeTwo:
                inkballScript.SetPiercing(_redcapPierceAmount);
                break;
            //For stun, have the enemy hit have their velocity set to 0 for x amount of time
            case TestUnitUpgradePaths.upgradeThree:
                inkballScript.SetStunValues(true, _redcapStunDuration);
                break;
            default:
                break;
        }
    }
*/
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
        return _inkDemonSO.StampName;
    }

    public string GetTileDescription()
    {
        string name = _inkDemonCardSO.CardName + " | Unit | " + _inkDemonCardSO.InkCost + " Ink";
        string description = _inkDemonCardSO.CardDescription;
        string stats = _inkDemonAbilityCooldown + " Cooldown | " + _inkMinionHealth + " Minion Health | " + _inkMinionDamage + " Minion Damage | " + _inkMinionAttackSpeed + " Minion Attacks/Sec";

        return name + "\n" + description + "\n" + stats;
    }
}

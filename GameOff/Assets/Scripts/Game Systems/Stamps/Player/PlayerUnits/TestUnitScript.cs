using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnitScript : MonoBehaviour, IUnitStamp
{
    [SerializeField] private UnitStampScriptableObject _unitSO;
    [SerializeField] private CardScriptableObject _cardSO;

    [SerializeField] private bool _isActive;
    [SerializeField] private float _currentUnitAbilityCooldown;
    [SerializeField] private float _currentUnitAttackCooldown;

#region UnitStats
    [SerializeField] private string _unitName;
    [SerializeField] private  GameObject _spawnedUnit;
    [SerializeField] private string _unitType;
    [SerializeField] private float _unitAbilityCooldown;
    [SerializeField] private float _unitCooldownReduction;
    [SerializeField] private float _unitDamage;
    [SerializeField] private float _unitAttackSpeed;
    [SerializeField] private float _unitSlowAmount;
#endregion

    [SerializeField] private int _unitLane;

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

        _currentUnitAbilityCooldown = _unitAbilityCooldown;
        _currentUnitAttackCooldown = 1 / _unitAttackSpeed;
    }

    public void LoadBaseStats()
    {
        _unitName = _unitSO.StampName;
        _spawnedUnit = _unitSO.SpawnedUnit;
        _unitType = _unitSO.UnitType;
        _unitAbilityCooldown = _unitSO.UnitAbilityCooldown;
        _unitCooldownReduction = _unitSO.UnitCooldownReduction;
        _unitDamage = _unitSO.UnitDamage;
        _unitAttackSpeed = _unitSO.UnitAttackSpeed;
        _unitSlowAmount = _unitSO.UnitSlowAmount;
    }

    public void SetLane(int lane)
    {
        _unitLane = lane;
    }

    private void FixedUpdate() 
    {
        if(_isActive)
        {
            //Ability CD Timer
            if(_currentUnitAbilityCooldown <= 0)
            {
                ActivateStampAbility();
                //calculate cooldown based on cdr
                _currentUnitAbilityCooldown = _unitAbilityCooldown * (1 - (_unitCooldownReduction/100));
            }
            _currentUnitAbilityCooldown -= Time.deltaTime;

            if(_currentUnitAttackCooldown <= 0)
            {
                ActivateStampAttack();
                _currentUnitAttackCooldown = 1 / _unitAttackSpeed;
            }
            _currentUnitAttackCooldown -= Time.deltaTime;
        }
    }

    public void OpenUnitUpgrade()
    {
        //Opens the upgrade UI
    }

    public void UpgradeUnit(int upgradePath)
    {
        //bring up the upgrade menu I think
    }

    public void ReduceCooldown(float reductionAmount)
    {

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

    public void UpgradeBaseAbilityHelper()
    {

    }

    public void UpgradeOneAbilityHelper()
    {
        
    }

    public void UpgradeTwoAbilityHelper()
    {
        
    }

    public void UpgradeThreeAbilityHelper()
    {
        
    }
#endregion
    
    public void ActivateStampAttack()
    {
        //do the attack
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
        return _unitName;
    }

    public string GetTileDescription()
    {
        string name = Vocab.SEPARATE(new string[] {_cardSO.CardName, Vocab.PLAYER_UNIT, Vocab.INKCOST(_cardSO.InkCost)});
        string description = _cardSO.CardDescription;
        return name + "\n" + description;
    }
}

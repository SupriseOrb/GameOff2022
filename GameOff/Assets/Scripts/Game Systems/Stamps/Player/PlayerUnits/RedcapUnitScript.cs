using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedcapUnitScript : MonoBehaviour, IUnitStamp
{
    [SerializeField] private UnitStampScriptableObject _redcapSO;
    [SerializeField] private CardScriptableObject _cardSO;
    [SerializeField] private float _redcapAttackCooldown;

#region UnitStats
    [SerializeField] private string _unitType;
    [SerializeField] private float _redcapDamage;
    [SerializeField] private float _redcapAttackSpeed;
    [SerializeField] private float _unitSlowAmount;
    [SerializeField] private int _redcapPierceAmount;
    [SerializeField] private float _redcapStunDuration;
#endregion

#region UnitStatGetters
    public float Damage
    {
        get {return _redcapDamage;}
    }

    public int PierceAmount
    {
        get {return _redcapPierceAmount;}
    }

    public float StunDuration
    {
        get {return _redcapStunDuration;}
    }
    public bool CanStun
    {
        get {return _canStun;}
    }
#endregion

#region UnitVariables
    [SerializeField] private GameObject _inkProjectile;
    [SerializeField] private Transform _inkProjectileSpawnLocation;
    [SerializeField] private bool _canStun = false;
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

#region Unit Upgrade Values
    [SerializeField] private float _attackDamageUpgradeIncrease;
    [SerializeField] private float _attackSpeedUpgradeIncrease;
    [SerializeField] private int _pierceUpgradeIncrease;
    [SerializeField] private float _stunDurationUpgradeIncrease;
    [SerializeField] private float _stunDurationBaseUpgrade;
#endregion
    [SerializeField] BoolVariable _isInWave;

    public enum RedcapUpgradePaths
    {
        //Red
        upgradeBase = 0,
        //Blue
        upgradeLeadInk = 1,
        //Purple
        upgradeStickyInk = 2,
    }
    public RedcapUpgradePaths _currentUpgradePath = RedcapUpgradePaths.upgradeBase;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_StampRedcap", gameObject);
        LoadBaseStats();
        LoadUpgradeStats();
        _redcapAttackCooldown = 1 / _redcapAttackSpeed;
        _redcapAnimator.Play(_redcapAppearAnimationName);
        _currentUpgradePath = RedcapUpgradePaths.upgradeBase;
    }

    public void LoadBaseStats()
    {
        //Test variables:
        _redcapStunDuration = 0;
        _redcapPierceAmount = 0;

        _unitType = _redcapSO.UnitType;
        _redcapDamage = _redcapSO.UnitDamage;
        _redcapAttackSpeed = _redcapSO.UnitAttackSpeed;
        _unitSlowAmount = _redcapSO.UnitSlowAmount;
    }

    public void LoadUpgradeStats()
    {
        _attackDamageUpgradeIncrease = _redcapSO.AttackDamageIncreaseAmount;
        _attackSpeedUpgradeIncrease = _redcapSO.AttackSpeedIncreaseAmount;
        _pierceUpgradeIncrease = (int)_redcapSO.UniqueUpgradeOneIncreaseAmount;
        _stunDurationUpgradeIncrease = _redcapSO.UniqueUpgradeTwoIncreaseAmount;
        _stunDurationBaseUpgrade = _redcapSO.UniqueUpgradeThreeIncreaseAmount;
    }


    public void SetLane(int lane)
    {
        _redcapLaneNumber = lane;
    }

    private void FixedUpdate() 
    {
        if(_isInWave.Value)
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
        UpgradeMenu.Instance.Open(gameObject, _cardSO.CardIcon, _redcapSO.Upgrades, (int)_currentUpgradePath);
    }

    public void UpgradeUnit(int upgradePath)
    {
        //bring up the upgrade menu I think
        if(upgradePath == (int)_currentUpgradePath)
        {
            Debug.Log("Upgrading Random Stat");
            int upgradedStat = Random.Range(0, 3);
            switch (upgradedStat)
            {
                case 0:
                    _redcapDamage += _attackDamageUpgradeIncrease;
                    break;
                case 1:
                    _redcapAttackSpeed += _attackSpeedUpgradeIncrease;
                    break;
                default:
                    if(_currentUpgradePath == RedcapUpgradePaths.upgradeLeadInk)
                    {
                        _redcapPierceAmount += _pierceUpgradeIncrease;
                    }
                    else
                    {
                        _redcapStunDuration += _stunDurationUpgradeIncrease;
                    }
                    break;
            }
            //Note: Cap Atk Speed at 1.0 (matches animation length) or increase anim speed
            //Random Upgradable Stats: Attack, Attack Speed, Pierce Amt/Stun Duration (depending on Upgrade path)
        }
        else
        {
            if(upgradePath == (int)RedcapUpgradePaths.upgradeLeadInk)
            {
                LoadBaseStats();
                _redcapPierceAmount = 1;
                _currentUpgradePath = RedcapUpgradePaths.upgradeLeadInk;
                _redcapAnimator.Play(_bluecapAppearAnimationName);
            }
            else //if upgradePath == (int)RedcapUpgradePaths.upgradeTwo
            {
                LoadBaseStats();
                _redcapStunDuration = 2;
                _canStun = true;
                _currentUpgradePath = RedcapUpgradePaths.upgradeStickyInk;
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
            case RedcapUpgradePaths.upgradeLeadInk:
                break;
            case RedcapUpgradePaths.upgradeStickyInk:
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
            _redcapAnimator.SetFloat("AttackSpeedModifier", _redcapAttackSpeed);
        }
        // Do the attack
        GameObject inkball = Instantiate(_inkProjectile, _inkProjectileSpawnLocation.position, Quaternion.identity, gameObject.transform);
        switch(_currentUpgradePath)
        {
            /*
            For pierce, maybe have a raycast shoot out when the inkball hits the 1st enemy and check for other enemies
            If they exist, deal x% of the initial hit to them?
            */
            case RedcapUpgradePaths.upgradeLeadInk:
                // Do the attack animation
                _redcapAnimator.Play(_bluecapAttackAnimationName);
                break;
            /*
            For stun, have the enemy hit have their velocity set to 0 for x amount of time
            */
            case RedcapUpgradePaths.upgradeStickyInk:
                // Do the attack animation
                _redcapAnimator.Play(_purplecapAttackAnimationName);
                break;
            default:
                // Do the attack animation
                _redcapAnimator.Play(_redcapAttackAnimationName);
                break;
        }
    }

    public string GetStampName()
    {
        return _redcapSO.StampName;
    }

    public string GetTileDescription()
    {
        string name = Vocab.SEPARATE(new string[] {_cardSO.CardName, Vocab.PLAYER_UNIT, Vocab.INKCOST(_cardSO.InkCost)});
        string description = _cardSO.CardDescriptionGivenInt((int)_currentUpgradePath);
        string stats = Vocab.SEPARATE(new string[] {Vocab.STUN_DURATION(StunDuration), Vocab.PIERCE_AMOUNT(PierceAmount), Vocab.DAMAGE(Damage), Vocab.ATKSPD(_redcapAttackSpeed)});
        return name + "\n" + description + "\n" + stats;
    }
}

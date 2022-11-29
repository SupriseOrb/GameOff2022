using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpyUnitScript : MonoBehaviour, IUnitStamp
{
    [SerializeField] private UnitStampScriptableObject _harpySO;
    [SerializeField] private CardScriptableObject _cardSO;
    [SerializeField] private float _harpyAttackCooldown;
    [SerializeField] private float _harpyForcedMoveSpeed;

#region UnitStats
    [SerializeField] private string _unitType;
    [SerializeField] private float _harpyDamage;
    [SerializeField] private float _harpyAttackSpeed;
    [SerializeField] private float _harpySlowIntensity;
    [SerializeField] private float _harpySlowDuration;
    [SerializeField] private float _harpyPushDistance;
#endregion

#region UnitStatGetters
    public float Damage
    {
        get {return _harpyDamage;}
    }
    public float PushDistance
    {
        get {return _harpyPushDistance;}
    }
    public float SlowIntensity
    {
        get {return _harpySlowIntensity;}
    }
    public float SlowDuration
    {
        get {return _harpySlowDuration;}
    }
    public float ForcedMoveSpeed
    {
        get {return _harpyForcedMoveSpeed;}
    }
#endregion

#region FeatherVariables
    [SerializeField] private GameObject _featherProjectile;
    [SerializeField] private Transform _featherProjectileSpawnLocation;
    [SerializeField] private float _featherDestroyTime;
    public float FeatherDestroyTime
    {
        get {return _featherDestroyTime;}
    }
#endregion

#region UnitVariables
    [SerializeField] private Animator _harpyAnimator;
    [SerializeField] private string _harpyAquaAppearAnimationName = "Harpy_Aqua_Appear";
    [SerializeField] private string _harpyBlueAppearAnimationName = "Harpy_Blue_Appear";
    [SerializeField] private string _harpyGreenAppearAnimationName = "Harpy_Green_Appear";
    [SerializeField] private string _harpyAquaAttackAnimationName = "Harpy_Aqua_Attack";
    [SerializeField] private string _harpyBlueAttackAnimationName = "Harpy_Blue_Attack";
    [SerializeField] private string _harpyGreenAttackAnimationName = "Harpy_Green_Attack";
    
    [SerializeField] private int _harpyLaneNumber;
#endregion

#region Unit Upgrade Values
    [SerializeField] private float _attackDamageUpgradeIncrease;
    [SerializeField] private float _attackSpeedUpgradeIncrease;
    [SerializeField] private float _slowIntensityUpgradeIncrease;
    [SerializeField] private float _pushDistanceUpgradeIncrease;
    [SerializeField] private float _attackSpeedUpgradeDecrease;
#endregion
    [SerializeField] BoolVariable _isInWave;

    public enum HarpyUpgradePaths
    {
        //Aqua
        upgradeBase = 0,
        //Blue
        upgradeDisorientingSong = 1,
        //Green
        upgradeBoomingSong = 2,
    }
    public HarpyUpgradePaths _currentUpgradePath = HarpyUpgradePaths.upgradeBase;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_StampHarpy", gameObject);
        LoadBaseStats();
        LoadUpgradeStats();
        _harpyAttackCooldown = 1 / _harpyAttackSpeed;
        _harpyAnimator.Play(_harpyAquaAppearAnimationName);
    }

    public void LoadBaseStats()
    {
        _unitType = _harpySO.UnitType;
        _harpyDamage = _harpySO.UnitDamage;
        _harpyAttackSpeed = _harpySO.UnitAttackSpeed;
        _harpySlowIntensity = _harpySO.UnitSlowAmount;
        _harpySlowDuration = _harpySO.UnitSlowDuration;

        //If left tile has a unit and it's this unit
        if (BoardManager.Instance.GetLane(_harpyLaneNumber).GetLaneUnits()[0] != null &&
            BoardManager.Instance.GetLane(_harpyLaneNumber).GetLaneUnits()[0].transform.position == this.gameObject.transform.position)
        {
            _featherDestroyTime = 0.315f;
        }
        //If right tile has a unit and it's this unit
        else if (BoardManager.Instance.GetLane(_harpyLaneNumber).GetLaneUnits()[1] != null &&
            BoardManager.Instance.GetLane(_harpyLaneNumber).GetLaneUnits()[1].transform.position == this.gameObject.transform.position)
        {
            _featherDestroyTime = 0.277f;
        }
        
        //Set once testing is done
        //_harpyForcedMoveSpeed = 0f;
    }

    public void LoadUpgradeStats()
    {
        _attackDamageUpgradeIncrease = _harpySO.AttackDamageIncreaseAmount;
        _attackSpeedUpgradeIncrease = _harpySO.AttackSpeedIncreaseAmount;
        _slowIntensityUpgradeIncrease = _harpySO.UniqueUpgradeOneIncreaseAmount;
        _pushDistanceUpgradeIncrease = _harpySO.UniqueUpgradeTwoIncreaseAmount;
        _attackSpeedUpgradeDecrease = _harpySO.UniqueUpgradeThreeIncreaseAmount;
    }


    public void SetLane(int lane)
    {
        _harpyLaneNumber = lane;
    }

    private void FixedUpdate() 
    {
        if(_isInWave.Value)
        {
            if(_harpyAttackCooldown <= 0)
            {
                ActivateStampAttack();
                _harpyAttackCooldown = 1 / _harpyAttackSpeed;
            }
            _harpyAttackCooldown -= Time.deltaTime;
        }
    }

    public void OpenUnitUpgrade()
    {
        UpgradeMenu.Instance.Open(gameObject, _cardSO.CardIcon, _harpySO.Upgrades, (int)_currentUpgradePath);
    }

    public void UpgradeUnit(int upgradePath)
    {
        //bring up the upgrade menu I think
        if(upgradePath == (int)_currentUpgradePath)
        {
            //Random Upgradable Stats: Attack, Attack Speed, Slow Intensity / Push Distance (depending on upgrade path)
            Debug.Log("Upgrading Random Stat");
            int upgradedStat;
            if (_currentUpgradePath == HarpyUpgradePaths.upgradeDisorientingSong)
            {
                upgradedStat = Random.Range(0, 4);
            }
            else //if upgrade 2
            {
                upgradedStat = Random.Range(0, 2);
            }

            switch(upgradedStat)
            {
                case 0:
                    _harpyDamage += _attackDamageUpgradeIncrease;
                    break;
                case 1:
                    _harpyAttackSpeed += _attackSpeedUpgradeIncrease;
                    break;
                case 2:
                    _harpyPushDistance += _pushDistanceUpgradeIncrease; 
                    break;
                default:
                    _harpySlowIntensity += _slowIntensityUpgradeIncrease;
                    break;
            }
        }
        else
        {
            if(upgradePath == (int)HarpyUpgradePaths.upgradeDisorientingSong)
            {
                LoadBaseStats();
                _harpySlowIntensity = 0f;
                _harpyForcedMoveSpeed = 25f;
                _harpyAttackSpeed = _attackSpeedUpgradeDecrease;
                _currentUpgradePath = HarpyUpgradePaths.upgradeDisorientingSong;
                _harpyAnimator.Play(_harpyBlueAppearAnimationName);
            }
            else //if upgradePath == (int)HarpyUpgradePaths.upgradeTwo
            {
                LoadBaseStats();
                _harpyPushDistance = 3f;   
                _harpyForcedMoveSpeed = 10 * _harpyPushDistance;
                _currentUpgradePath = HarpyUpgradePaths.upgradeBoomingSong;
                _harpyAnimator.Play(_harpyGreenAppearAnimationName);
            }
        }
        
    }

    public void ReduceCooldown(float reductionAmount)
    {
        _harpyAttackCooldown -= reductionAmount;
    }

#region Ability Functions
    public void ActivateStampAbility()
    {
        switch(_currentUpgradePath)
        {
            case HarpyUpgradePaths.upgradeDisorientingSong:
                break;
            case HarpyUpgradePaths.upgradeBoomingSong:
                break;
            default:
                break;
        }
    }
#endregion
    
    public void ActivateStampAttack()
    {
        //AkSoundEngine.PostEvent("Play_HarpyAbility1", gameObject);
        if (_harpyAttackSpeed > 1)
        {
            _harpyAnimator.SetFloat("AttackSpeedModifier", _harpyAttackSpeed);
        }
        // Do the attack
        GameObject feather = Instantiate(_featherProjectile, _featherProjectileSpawnLocation.position, Quaternion.identity, gameObject.transform);
        switch(_currentUpgradePath)
        {
            case HarpyUpgradePaths.upgradeDisorientingSong:
                // Do the attack animation
                _harpyAnimator.Play(_harpyBlueAttackAnimationName);
                break;
            case HarpyUpgradePaths.upgradeBoomingSong:
                // Do the attack animation
                _harpyAnimator.Play(_harpyGreenAttackAnimationName);
                break;
            default:
                // Do the attack animation
                _harpyAnimator.Play(_harpyAquaAttackAnimationName);
                break;
        }
    }

    public string GetStampName()
    {
        return _harpySO.StampName;
    }

    public string GetTileDescription()
    {
        string name = Vocab.SEPARATE(new string[] {_cardSO.CardName, Vocab.PLAYER_UNIT, Vocab.INKCOST(_cardSO.InkCost)});
        string description = _cardSO.CardDescriptionGivenInt((int)_currentUpgradePath);
        string stats = Vocab.SEPARATE(new string[] {Vocab.PUSH_DISTANCE(PushDistance), Vocab.DAMAGE(_harpyDamage), Vocab.DAMAGE(Damage), Vocab.ATKSPD(_harpyAttackSpeed)});
        return name + "\n" + description + "\n" + stats;
    }
}

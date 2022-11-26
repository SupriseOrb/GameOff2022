using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpyUnitScript : MonoBehaviour, IUnitStamp
{
    [SerializeField] private UnitStampScriptableObject _harpySO;
    [SerializeField] private CardScriptableObject _harpyCardSO;
    [SerializeField] private bool _isActive = false;
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

#region UnitVariables
    [SerializeField] private GameObject _featherProjectile;
    [SerializeField] private Transform _featherProjectileSpawnLocation;
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
#endregion

    public enum HarpyUpgradePaths
    {
        //Red
        upgradeBase = 0,
        //Blue
        upgradeDisorientingSong = 1,
        //Purple
        upgradeBoomingSong = 2,
    }
    public HarpyUpgradePaths _currentUpgradePath = HarpyUpgradePaths.upgradeBase;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_StampGeneral", gameObject);
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
        //_harpySlowDuration = 1f;
        //_harpyPushDistance = 0f;
    }

    public void LoadUpgradeStats()
    {
        _attackDamageUpgradeIncrease = _harpySO.AttackDamageIncreaseAmount;
        _attackSpeedUpgradeIncrease = _harpySO.AttackSpeedIncreaseAmount;
        _slowIntensityUpgradeIncrease = _harpySO.UniqueUpgradeOneIncreaseAmount;
        _pushDistanceUpgradeIncrease = _harpySO.UniqueUpgradeTwoIncreaseAmount;
    }


    public void SetLane(int lane)
    {
        _harpyLaneNumber = lane;
    }

    private void FixedUpdate() 
    {
        if(_isActive)
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
        UpgradeMenu.Instance.Open(gameObject, _harpyCardSO.CardIcon, _harpySO.Upgrades, (int)_currentUpgradePath);
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
                _currentUpgradePath = HarpyUpgradePaths.upgradeDisorientingSong;
                _harpyAnimator.Play(_harpyBlueAppearAnimationName);
            }
            else //if upgradePath == (int)HarpyUpgradePaths.upgradeTwo
            {
                _harpyPushDistance = 3f;   
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
        if(_harpyAttackSpeed > 1)
        {
            _harpyAnimator.speed = _harpyAttackSpeed;
        }
        // Do the attack
        GameObject feather = Instantiate(_featherProjectile, _featherProjectileSpawnLocation.position, Quaternion.identity, gameObject.transform);
        //Consider just declaring the inkballScript
        //FeatherProjectile featherScript = feather.GetComponent<FeatherProjectile>();
        //featherScript.SetDamage(_harpyDamage);
        switch(_currentUpgradePath)
        {
            /*
            For pierce, maybe have a raycast shoot out when the inkball hits the 1st enemy and check for other enemies
            If they exist, deal x% of the initial hit to them?
            */
            case HarpyUpgradePaths.upgradeDisorientingSong:
                // Do the attack animation
                _harpyAnimator.Play(_harpyBlueAttackAnimationName);
                //featherScript.SetSprite((int)HarpyUpgradePaths.upgradeOne);
                //Upgrade: Move enemy to a different lane (top, bottom => middle); longer CD
                //This probably holds 3 positions for the y to switch between (?) and lerps (?)
                break;
            /*
            For stun, have the enemy hit have their velocity set to 0 for x amount of time
            */
            case HarpyUpgradePaths.upgradeBoomingSong:
                // Do the attack animation
                _harpyAnimator.Play(_harpyGreenAttackAnimationName);
                //featherScript.SetSprite((int)HarpyUpgradePaths.upgradeTwo);
                //Upgrade: Push enemy back and slow
                
                break;
            default:
                // Do the attack animation
                _harpyAnimator.Play(_harpyAquaAttackAnimationName);
                //featherScript.SetSprite((int)HarpyUpgradePaths.upgradeBase);
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
        return _harpySO.StampName;
    }
}

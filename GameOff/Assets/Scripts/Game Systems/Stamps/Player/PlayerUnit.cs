using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    #region General Unit Info
    // TODO : CardData should be separated into multiple categories, one of which should be PlayerCardData. Use that instead.
    [SerializeField] private CardData _data;
    [SerializeField] private BoolVariable _isInWave;
    private string _tileDescription;
    private int _laneNumber;
    private int _tileNumber;
    private int _currentUpgradePath = 0;
    #endregion

    #region Animation
    [SerializeField] private Animator _animator;
    private string _appearBaseAnimationName;
    private string _appear1AnimationName;
    private string _appear2AnimationName;
    private string _attackBaseAnimationName;
    private string _attack1AnimationName;
    private string _attack2AnimationName;
    #endregion

    #region Unit RNG Stats
    private float _attackDamageUpgradeValue;
    private float _cooldownReductionUpgradeValue;
    #endregion

    private string _appearSFXName;

    #region Unit Functions
    protected void Start()
    {
        LoadBaseStats();
        LoadUpgradeStats();
        _animator.Play(_appearBaseAnimationName);
        AkSoundEngine.PostEvent(_appearSFXName, gameObject);
    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void OpenUpgradeMenu()
    {
        //UpgradeMenu.Instance.Open(gameObject, _data.CardIcon, _data.Upgrades, (int)_currentUpgradePath);
    }

    protected virtual void ReduceCooldown()
    {
        // TODO : Can't this be taken care of through a setter on the CD Value of the unit (which is in place currently)?
    }

    protected virtual void LoadBaseStats()
    {

    }

    protected virtual void LoadUpgradeStats()
    {

    }

    protected virtual void UpgradeUnit()
    {

    }
    #endregion
    #region Properties
    /*
    =================
    General Unit Info
    =================
    */
    protected CardData Data {get {return _data;}}
    protected BoolVariable IsInWave {get {return _isInWave;}}
    protected string TileDescription {get {return _tileDescription;} set {_tileDescription = value;}}
    protected int LaneNumber {get {return _laneNumber;} set {_laneNumber = value;}}
    protected int TileNumber {get {return _tileNumber;} set {_tileNumber = value;}}
    protected int CurrentUpgradePath {get {return _currentUpgradePath;} set {_currentUpgradePath = value;}}
    /*
    =================
    Animation
    =================
    */
    protected Animator Animator {get {return _animator;}}
    protected string AppearBaseAnimationName {get {return _appearBaseAnimationName;} set {_appearBaseAnimationName = value;}}
    protected string Appear1AnimationName {get {return _appear1AnimationName;} set {_appear1AnimationName = value;}}
    protected string Appear2AnimationName {get {return _appear2AnimationName;} set {_appear2AnimationName = value;}}
    protected string AttackBaseAnimationName {get {return _attackBaseAnimationName;} set {_attackBaseAnimationName = value;}}
    protected string Attack1AnimationName {get {return _attack1AnimationName;} set {_attack1AnimationName = value;}}
    protected string Attack2AnimationName {get {return _attack2AnimationName;} set {_attack2AnimationName = value;}}
    /*
    =================
    Unit RNG Stats
    =================
    */
    protected float AttackDamageUpgradeValue {get {return _attackDamageUpgradeValue;} set {_attackDamageUpgradeValue = value;}}
    protected float CooldownReductionUpgradeValue {get {return _cooldownReductionUpgradeValue;} set {_cooldownReductionUpgradeValue = value;}}
    protected string AppearSFXName {get {return _appearSFXName;} set {_appearSFXName = value;}}
    /*
    =================
    Card Data
    =================
    */
    #endregion

}

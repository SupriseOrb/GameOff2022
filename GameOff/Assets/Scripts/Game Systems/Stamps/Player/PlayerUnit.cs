using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    #region General Unit Info
    private CardData _data;
    private string _tileDescription;
    private int _laneNumber;
    private int _tileNumber;
    private int _currentUpgradePath = 0;
    #endregion

    #region Animation
    private Animator _animator;
    private string _appearBaseAnimationName;
    private string _appear1AnimationName;
    private string _appear2AnimationName;
    private string _attackBaseAnimationName;
    private string _attack1AnimationName;
    private string _attack2AnimationName;
    #endregion

    #region Unit RNG Upgrades
    private float _attackDamageUpgradeValue;
    private float _attackSpeedUpgradeValue;
    #endregion

    private string _appearSFXName;

    #region Unit Getters
    /*
    =================
    General Unit Info
    =================
    */
    protected virtual CardData Data {get {return _data;}}
    protected virtual string TileDescription {get {return _tileDescription;}}
    protected virtual int LaneNumber {get {return _laneNumber;}}
    protected virtual int TileNumber {get {return _tileNumber;}}
    protected virtual int CurrentUpgradePath {get {return _currentUpgradePath;}}
    /*
    =================
    Animation
    =================
    */
    protected virtual Animator Animator {get {return _animator;}}
    protected virtual string AppearBaseAnimationName {get {return _appearBaseAnimationName;}}
    protected virtual string Appear1AnimationName {get {return _appear1AnimationName;}}
    protected virtual string Appear2AnimationName {get {return _appear2AnimationName;}}
    protected virtual string AttackBaseAnimationName {get {return _attackBaseAnimationName;}}
    protected virtual string Attack1AnimationName {get {return _attack1AnimationName;}}
    protected virtual string Attack2AnimationName {get {return _attack2AnimationName;}}
    /*
    =================
    Unit RNG Upgrades
    =================
    */
    protected virtual float AttackDamageUpgradeValue {get {return _attackDamageUpgradeValue;}}
    protected virtual float AttackSpeedUpgradeValue {get {return _attackSpeedUpgradeValue;}}

    protected virtual string AppearSFXName {get {return _appearSFXName;}}
    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    private static UpgradeMenu _instance;
    public static UpgradeMenu Instance
    {
        get{return _instance;}
    }

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animationOpenString = "Panel_Open";
    [SerializeField] private string _animationCloseString = "Panel_Close";

    [Header("Upgrades")]
    [SerializeField] private Upgrade[] _cardUpgrades;

    [SerializeField] private IUnitStamp _unitToBeUpgraded;
    [System.Serializable]
    public struct Upgrade
    {
        [SerializeField] public Image icon;
        [SerializeField] public TextMeshProUGUI name;
        [SerializeField] public TextMeshProUGUI description;
    } 

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Open(IUnitStamp unit, Sprite unitIcon, UnitStampScriptableObject.UpgradeInfo[] upgrades)
    {
        for (int i = 0; i < _cardUpgrades.Length; i++)
        {
            _cardUpgrades[i].icon.sprite = unitIcon;
            _cardUpgrades[i].name.text = upgrades[i].name;
            _cardUpgrades[i].description.text = upgrades[i].description;
        }
        _unitToBeUpgraded = unit;

        _animator.Play(_animationOpenString);    
    }

    public void ChooseUpgrade(int path)
    {
        _animator.Play(_animationCloseString);
        _unitToBeUpgraded.UpgradeUnit(path);
    }
}

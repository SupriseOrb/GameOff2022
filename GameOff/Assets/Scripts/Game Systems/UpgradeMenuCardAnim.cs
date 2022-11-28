using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeMenuCardAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animationHighlight = "Card_Highlighted";
    [SerializeField] private string _animationIdle = "Card_Idle";
    [SerializeField] private BoolVariable _isHoveringUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.Play(_animationHighlight);
        AkSoundEngine.PostEvent("Play_ClicheHover", gameObject);
        _isHoveringUI.Value = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.Play(_animationIdle);
        _isHoveringUI.Value = false;
    }
}

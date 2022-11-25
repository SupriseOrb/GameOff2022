using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeMenuCardAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animationHighlight = "Card_Highlighted";
    [SerializeField] private string _animationIdle = "Card_Idle";

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.Play(_animationHighlight);
        AkSoundEngine.PostEvent("Play_ClicheHover", gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.Play(_animationIdle);
    }
}

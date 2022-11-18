using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] string[] _animationsToCycle;
    int _index = 0;

    public void PlayNextAnimation()
    {
        if (_animationsToCycle.Length == 0)
        {
            Debug.Log("No animations to cycle through");
            return;
        }
        if (_index >= _animationsToCycle.Length)
        {
            _index = 0;
        }
        _animator.Play(_animationsToCycle[_index]);
        _index++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDrag : MonoBehaviour
{
    [SerializeField] private bool _isDragging;
    [SerializeField] private SpriteRenderer _iconSR;
    [SerializeField] private GameObject _gameObject;

    public bool IsDragging
    {
        get {return _isDragging;}
    }

    private void Awake()
    {
        _isDragging = false;
    }

    public void ToggleIsDragging(bool isDragging, CardScriptLoader card = null)
    {
        _isDragging = isDragging;
        
        if (_isDragging && card !=null)
        {
            _iconSR.sprite = card.CardSO.CardIcon;
        }

        _gameObject.SetActive(_isDragging);
    }

    public void Move(Vector2 position)
    {
        if (_isDragging)
        {
            transform.position = position;
        }
    }
}

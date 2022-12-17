using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Rename to something else
// It implies we are dragging it, but in reality we're trying to convey that we are holding a card
public class SpriteDrag : MonoBehaviour
{
    [SerializeField] private bool _isDragging;
    [SerializeField] private SpriteRenderer _iconSR;
    [SerializeField] private GameObject _gameObject;

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
            transform.position = new Vector3(0f, 0f, -10);
        }

        _gameObject.SetActive(_isDragging);
    }

    public void Move(Vector3 position)
    {
        if (_isDragging)
        {
            transform.position = position;
        }
    }
}

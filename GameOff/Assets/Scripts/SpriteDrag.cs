using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDrag : MonoBehaviour
{
    [SerializeField] private Sprite _spriteRef = null;
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private bool _isDragging = false;

    //Consider making _mousePositionWorld a Vector3 SO instead of having to make this many connections to get the info
    [SerializeField] private InputManager _input;

    public Sprite SpriteReference
    {
        get {return _spriteRef;}
        set {_spriteRef = value;}
    }

    public bool IsDragging
    {
        get {return _isDragging;}
        set {_isDragging = value;}
    }

    private void Update()
    {
        if (_isDragging == true)
        {
            _spriteRender.sprite = _spriteRef;
            gameObject.transform.position = new Vector3(_input.MousePositionWorld.x, _input.MousePositionWorld.y, 0);
        }
        if (_isDragging == false)
        {
            _spriteRender.sprite = null;
        }
    }
}

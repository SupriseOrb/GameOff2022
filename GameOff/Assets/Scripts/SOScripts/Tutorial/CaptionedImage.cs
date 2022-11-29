using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CaptionedImage", menuName = "GameOff2022/Tutorial/ImageWithCaption", order = 0)]
public class CaptionedImage : ScriptableObject
{
    [SerializeField] private Sprite _image;
    [SerializeField] [TextArea] string _description;

    public Sprite Image
    {
        get {return _image;}
    }

    public string Description
    {
        get {return _description;}
    }

}

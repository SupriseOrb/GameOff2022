using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellStamp : IStamp
{
    bool IsDead();
    void SetTile(BoardTile item);
}

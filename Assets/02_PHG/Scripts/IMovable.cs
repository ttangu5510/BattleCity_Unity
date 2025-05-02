using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    public MoveType moveType { get; set; }
}

public enum MoveType
{
    normal, slide, slow, dotDamaged
}

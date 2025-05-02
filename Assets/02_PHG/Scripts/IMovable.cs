using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    public MoveType moveType { get; set; }
    public void MoveTypeUpate();
}

public enum MoveType
{
    normal, iceSlide, bushSlow, lavarDotDamaged, Stun, sandSlow
}

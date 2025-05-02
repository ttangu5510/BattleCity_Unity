using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    public MoveType moveType { get; set; }
    public void MoveTypeUpdate();
}

public enum MoveType
{
    none,normal, iceSlide, bushSlow, lavarDotDamaged, Stun, sandSlow
}

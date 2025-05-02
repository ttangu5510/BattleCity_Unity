using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData02
{
    public int id; //아이템 id
    public string name; //아이템이름
    public Action<GameObject> effect; //이팩트
    public float duration; //지속시간 (0초면 즉발)
    public GameObject itemprefap; //아이콘 추가

    public ItemData02(int id, string name, Action<GameObject> effect, GameObject itemprefap, float duration = 0f)
    {
        this.id = id;
        this.name = name;
        this.effect = effect;
        this.duration = duration;
        this.itemprefap = itemprefap;
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData02
{
    public int id; //������ id
    public string name; //�������̸�
    public Action<GameObject> effect; //����Ʈ
    public float duration; //���ӽð� (0�ʸ� ���)
    public GameObject itemprefap; //������ �߰�

    public ItemData02(int id, string name, Action<GameObject> effect, GameObject itemprefap, float duration = 0f)
    {
        this.id = id;
        this.name = name;
        this.effect = effect;
        this.duration = duration;
        this.itemprefap = itemprefap;
    }
}


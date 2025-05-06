using System;
using UnityEngine;
namespace KMS
{
    public class ItemData
    {
        public int id; //아이템 id
        public string name; //아이템이름
        public Action<GameObject> effect; //이팩트
        public float duration; //지속시간 (0초면 즉발)
        public Sprite icon; //아이콘 추가

        public ItemData(int id, string name, Action<GameObject> effect, float duration = 0f, Sprite icon = null)
        {
            this.id = id;
            this.name = name;
            this.effect = effect;
            this.duration = duration;
            this.icon = icon;
        }
    }
}


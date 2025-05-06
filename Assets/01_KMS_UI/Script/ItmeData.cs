using System;
using UnityEngine;
namespace KMS
{
    public class ItemData
    {
        public int id; //������ id
        public string name; //�������̸�
        public Action<GameObject> effect; //����Ʈ
        public float duration; //���ӽð� (0�ʸ� ���)
        public Sprite icon; //������ �߰�

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


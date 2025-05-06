using System;
using UnityEngine;


namespace CJM
{
    public abstract class Item : MonoBehaviour
    {
        // �̸� ��ü�� ������ �������� �̸��� ���
        public Action<Player> effect; //����Ʈ(����ȿ��)

        public abstract void SetEffect();

        private void Awake()
        {
            // �ڽĵ鿡�� ��ü���� ȿ�� ����
            SetEffect();
        }

        private void OnTriggerEnter(Collider other) //OnCollision�� ����Ϸ��������� ���۰��ӿ��� �������� �������� �����ϱ⿡ OnTrigger�� ����
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Player player = other.gameObject.GetComponentInParent<Player>();
                // ������ ȿ�� ����
                effect(player);

                // �������� ������ �������� �Ҹ�
                Destroy(gameObject);
            }
        }


        public void DropItem(Vector3 dropPos)
        {
            GameObject itemInstance = Instantiate(this.gameObject); // ����Ǿ��ִ� ������ ���
            itemInstance.transform.position = dropPos;
        }
    }
}
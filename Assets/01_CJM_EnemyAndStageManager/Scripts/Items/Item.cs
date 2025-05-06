using System;
using UnityEngine;


namespace CJM
{
    public abstract class Item : MonoBehaviour
    {
        // 이름 자체는 아이템 프리펩의 이름을 사용
        public Action<Player> effect; //이팩트(실행효과)

        public abstract void SetEffect();

        private void Awake()
        {
            // 자식들에서 구체적인 효과 설정
            SetEffect();
        }

        private void OnTriggerEnter(Collider other) //OnCollision을 사용하려고했으나 원작게임에선 벽위에도 아이템이 등장하기에 OnTrigger로 변경
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Player player = other.gameObject.GetComponentInParent<Player>();
                // 아이템 효과 적용
                effect(player);

                // 아이템은 역할이 끝났으니 소멸
                Destroy(gameObject);
            }
        }


        public void DropItem(Vector3 dropPos)
        {
            GameObject itemInstance = Instantiate(this.gameObject); // 저장되어있는 프리펩 출격
            itemInstance.transform.position = dropPos;
        }
    }
}
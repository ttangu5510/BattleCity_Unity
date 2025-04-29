using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public Sprite icon;
    public int effectValue;

    private void OnTriggerEnter(Collider other) //OnCollision을 사용하려고했으나 원작게임에선 벽위에도 아이템이 등장하기에 OnTrigger로 변경
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player collided with {itemName}");

            // 아이템 효과 적용은 ItemManager가 담당
            ItemManager.Instance.ApplyEffect(this, other.gameObject);

            // 아이템은 역할이 끝났으니 소멸
            Destroy(gameObject);
        }
    }
}

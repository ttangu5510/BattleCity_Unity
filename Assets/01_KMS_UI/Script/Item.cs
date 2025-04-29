using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public Sprite icon;
    public int effectValue;

    private void OnTriggerEnter(Collider other) //OnCollision�� ����Ϸ��������� ���۰��ӿ��� �������� �������� �����ϱ⿡ OnTrigger�� ����
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player collided with {itemName}");

            // ������ ȿ�� ������ ItemManager�� ���
            ItemManager.Instance.ApplyEffect(this, other.gameObject);

            // �������� ������ �������� �Ҹ�
            Destroy(gameObject);
        }
    }
}

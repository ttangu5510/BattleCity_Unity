using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private int itemID;
    private string itemName;
    private Sprite icon;
    private float timer;
    public float duration;
    private int effectValue;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                EndEffect(); // ���� �ð��� ������ ȿ���� ����
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player collided with {itemName}");
            //ApplyEffect(collision.gameObject.GetComponent<PlayerDate>());
            Destroy(gameObject); // ������ �Ҹ�
        }
    }
    private void EndEffect()
    {
        Debug.Log($"{itemName} effect ended.");
    }
}

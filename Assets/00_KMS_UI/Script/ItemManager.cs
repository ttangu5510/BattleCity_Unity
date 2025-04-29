using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    private Dictionary<int, ItemData> itemDataDictionary = new Dictionary<int, ItemData>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializeItems();
    }

    private void InitializeItems()
    {
        Sprite invincibilityIcon = Resources.Load<Sprite>("Sprites/Invincibility"); //�ϴ� �⺻���� ���ҽ������� ��������Ʈ�� �����ϰ� ����
        Sprite upgradeIcon = Resources.Load<Sprite>("Sprites/Upgrade"); //�ּҴ� �뷫������ Assets/Resources/Sprites.���ϸ�.png
        Sprite lifeUpIcon = Resources.Load<Sprite>("Sprites/LifeUp"); //�ʿ�� �ּҸ� �����Ͽ� ��밡��
        Sprite speedUpIcon = Resources.Load<Sprite>("Sprites/SpeedUp");
        Sprite stopEnemiesIcon = Resources.Load<Sprite>("Sprites/StopEnemies");
        Sprite destroyAllIcon = Resources.Load<Sprite>("Sprites/DestroyAllEnemies");
        Sprite baseUpIcon = Resources.Load<Sprite>("Sprites/BaseUp");

        //����
        // itemDataDictionary.Add(0, new ItemData(0, "Invincibility", (player) => 
        // {
        //     player.GetComponent<Player>().EnableInvincibility(5);
        // }, 5f, invincibilityIcon));
        // Debug.Log($"[Load Sprite] invincibilityIcon: {(invincibilityIcon != null ? "OK" : "FAILED")}");

        //���׷��̵�
        itemDataDictionary.Add(1, new ItemData(1, "Upgrade", (player) =>
        {
            player.GetComponent<Player>().Upgrade(10, 10); // �ӽ� +10 +10
        }, 0f, upgradeIcon));
        Debug.Log($"[Load Sprite] Upgrade: {(upgradeIcon != null ? "OK" : "FAILED")}");

        // ��� + 1 
        // itemDataDictionary.Add(2, new ItemData(2, "LifeUp", (player) =>
        // {
        //     player.GetComponent<Player>().GetLife(1); //player�� GetLife �߰�����
        // }, 0f, lifeUpIcon));
        // Debug.Log($"[Load Sprite] LifeUpIcon: {(lifeUpIcon != null ? "OK" : "FAILED")}");

        // ���ǵ��
        // itemDataDictionary.Add(3, new ItemData(3, "SpeedUp", (player) =>
        // {
        //     player.GetComponent<PlayerController>().(1);
        // }, 0f, speedUpIcon));
        // Debug.Log($"[Load Sprite] SpeedUpIcon: {(speedUpIcon != null ? "OK" : "FAILED")}");

        // ��� �� ����
        // itemDataDictionary.Add(4, new ItemData(4, "StopEnemies", (player) =>
        // {
        //     StopEnemies(5f); // 5�� ���� �� ����
        // }, 0f, stopEnemiesIcon));
        // Debug.Log($"[Load Sprite] StopEnemies: {(stopEnemiesIcon != null ? "OK" : "FAILED")}");

        // ��� �� �ı�
        itemDataDictionary.Add(5, new ItemData(5, "DestroyAllEnemies", (player) =>
        {
            DestroyAllEnemies(); // ��� ���� �ı�
        }, 0f, destroyAllIcon));
        Debug.Log($"[Load Sprite] DestroyAll: {(destroyAllIcon != null ? "OK" : "FAILED")}");

        // ���� ��ȭ //��ǥ ������Ŀ� ���� ���� �ʿ�
        //itemDataDictionary.Add(6, new ItemData(6, "BaseUp", (player) =>
        //{
        //    ObjectManager.Instance.ApplyAreaEffect("RegionTag", player.transform.position);
        //}, 0f, baseUpIcon));
        //
        //Debug.Log($"[Load Sprite] BaseUp: {(baseUpIcon != null ? "OK" : "FAILED")}");

    }


    public void ApplyEffect(Item item, GameObject player)
    {
        if (itemDataDictionary.TryGetValue(item.itemID, out var itemData))
        {
            Debug.Log($"Applying {itemData.name} to player");
            itemData.effect.Invoke(player);
        }
        else
        {
            Debug.LogWarning("Unknown item ID");
        }
    }

    //private void StopEnemies(float duration)
    //{
    //    LayerMask enemyLayer = LayerMask.GetMask("Enemy");
    //
    //    // "Enemy" ���̾ ���� �� ã��
    //    Collider[] enemies = Physics.OverlapSphere(Vector3.zero, 100f, enemyLayer);
    //    foreach (Collider enemy in enemies)
    //    {
    //        if //����Ʈ�ѷ������� ���� ã�� �߰�
    //        {
    //            //enemyController.StopMovement(duration); // �� ���� ó��
    //            Debug.Log($"Stopped enemy: {enemy.name} for {duration} seconds");
    //        }
    //    }
    //}

    private void DestroyAllEnemies()
    {
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");

        // Enemy ���̾��� ��� �� ã��
        Collider[] enemies = Physics.OverlapSphere(Vector3.zero, 100f, enemyLayer); //��ũ�� 16*16, ���� 100f
        foreach (Collider enemy in enemies)
        {
            Destroy(enemy.gameObject); // �� �ı�
            Debug.Log($"Destroyed enemy: {enemy.name}");
        }
    }
}

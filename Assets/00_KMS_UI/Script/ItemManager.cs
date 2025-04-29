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
        Sprite invincibilityIcon = Resources.Load<Sprite>("Sprites/Invincibility"); //일단 기본적인 리소스폴더의 스프라이트를 참조하게 만듦
        Sprite upgradeIcon = Resources.Load<Sprite>("Sprites/Upgrade"); //주소는 대략적으로 Assets/Resources/Sprites.파일명.png
        Sprite lifeUpIcon = Resources.Load<Sprite>("Sprites/LifeUp"); //필요시 주소를 변경하여 사용가능
        Sprite speedUpIcon = Resources.Load<Sprite>("Sprites/SpeedUp");
        Sprite stopEnemiesIcon = Resources.Load<Sprite>("Sprites/StopEnemies");
        Sprite destroyAllIcon = Resources.Load<Sprite>("Sprites/DestroyAllEnemies");
        Sprite baseUpIcon = Resources.Load<Sprite>("Sprites/BaseUp");

        //무적
        // itemDataDictionary.Add(0, new ItemData(0, "Invincibility", (player) => 
        // {
        //     player.GetComponent<Player>().EnableInvincibility(5);
        // }, 5f, invincibilityIcon));
        // Debug.Log($"[Load Sprite] invincibilityIcon: {(invincibilityIcon != null ? "OK" : "FAILED")}");

        //업그레이드
        itemDataDictionary.Add(1, new ItemData(1, "Upgrade", (player) =>
        {
            player.GetComponent<Player>().Upgrade(10, 10); // 임시 +10 +10
        }, 0f, upgradeIcon));
        Debug.Log($"[Load Sprite] Upgrade: {(upgradeIcon != null ? "OK" : "FAILED")}");

        // 목숨 + 1 
        // itemDataDictionary.Add(2, new ItemData(2, "LifeUp", (player) =>
        // {
        //     player.GetComponent<Player>().GetLife(1); //player에 GetLife 추가예정
        // }, 0f, lifeUpIcon));
        // Debug.Log($"[Load Sprite] LifeUpIcon: {(lifeUpIcon != null ? "OK" : "FAILED")}");

        // 스피드업
        // itemDataDictionary.Add(3, new ItemData(3, "SpeedUp", (player) =>
        // {
        //     player.GetComponent<PlayerController>().(1);
        // }, 0f, speedUpIcon));
        // Debug.Log($"[Load Sprite] SpeedUpIcon: {(speedUpIcon != null ? "OK" : "FAILED")}");

        // 모든 적 멈춤
        // itemDataDictionary.Add(4, new ItemData(4, "StopEnemies", (player) =>
        // {
        //     StopEnemies(5f); // 5초 동안 적 정지
        // }, 0f, stopEnemiesIcon));
        // Debug.Log($"[Load Sprite] StopEnemies: {(stopEnemiesIcon != null ? "OK" : "FAILED")}");

        // 모든 적 파괴
        itemDataDictionary.Add(5, new ItemData(5, "DestroyAllEnemies", (player) =>
        {
            DestroyAllEnemies(); // 모든 몬스터 파괴
        }, 0f, destroyAllIcon));
        Debug.Log($"[Load Sprite] DestroyAll: {(destroyAllIcon != null ? "OK" : "FAILED")}");

        // 기지 강화 //좌표 설정방식에 따라 수정 필요
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
    //    // "Enemy" 레이어에 속한 적 찾기
    //    Collider[] enemies = Physics.OverlapSphere(Vector3.zero, 100f, enemyLayer);
    //    foreach (Collider enemy in enemies)
    //    {
    //        if //적컨트롤러에대한 참조 찾기 추가
    //        {
    //            //enemyController.StopMovement(duration); // 적 멈춤 처리
    //            Debug.Log($"Stopped enemy: {enemy.name} for {duration} seconds");
    //        }
    //    }
    //}

    private void DestroyAllEnemies()
    {
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");

        // Enemy 레이어의 모든 적 찾기
        Collider[] enemies = Physics.OverlapSphere(Vector3.zero, 100f, enemyLayer); //맵크기 16*16, 임의 100f
        foreach (Collider enemy in enemies)
        {
            Destroy(enemy.gameObject); // 적 파괴
            Debug.Log($"Destroyed enemy: {enemy.name}");
        }
    }
}

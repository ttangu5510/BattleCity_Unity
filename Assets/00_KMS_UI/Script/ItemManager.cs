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
      //Sprite shieldIcon = Resources.Load<Sprite>("Sprites/AddShield"); //일단 기본적인 리소스폴더의 스프라이트를 참조하게 만듦
      //Sprite upgradeIcon = Resources.Load<Sprite>("Sprites/Upgrade"); //주소는 대략적으로 Assets/Resources/Sprites.파일명.png
      //Sprite lifeUpIcon = Resources.Load<Sprite>("Sprites/LifeUp"); //필요시 주소를 변경하여 사용가능
      //
      //itemDataDictionary.Add(0, new ItemData(0, "Shield", (player) =>
      //{
      //    player.GetComponent<PlayerController>().AddShield(5);
      //}, 5f, shieldIcon));
      //Debug.Log($"[Load Sprite] ShieldIcon: {(shieldIcon != null ? "OK" : "FAILED")}");
      //
      //itemDataDictionary.Add(1, new ItemData(1, "Upgrade", (player) =>
      //{
      //    player.GetComponent<Player>().Upgrade(10, 10); // 임시 +10 +10
      //}, 0f, upgradeIcon));
      //Debug.Log($"[Load Sprite] Upgrade: {(upgradeIcon != null ? "OK" : "FAILED")}");
      //
      //itemDataDictionary.Add(2, new ItemData(2, "LifeUp", (player) =>
      //{
      //    player.GetComponent<PlayerController>().life(1);
      //}, 0f, lifeUpIcon));
      //Debug.Log($"[Load Sprite] LifeUpIcon: {(lifeUpIcon != null ? "OK" : "FAILED")}");
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
}

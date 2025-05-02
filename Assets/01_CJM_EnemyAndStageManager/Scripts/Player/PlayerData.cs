using UnityEngine;

public class PlayerData
{
    private static PlayerData instance;
    public static PlayerData Instance { get { if (instance == null) instance = new PlayerData(); return instance; } }


    [SerializeField] public UpgradeType grade { get; private set; }

    [SerializeField] public int score { get; private set; }
    [SerializeField] public int life { get; private set; }
    [SerializeField] public float moveSpeed { get; private set; }
    [SerializeField] public float shotSpeed { get; private set; }



    public void SaveData(int life, float moveSpeed, float shotSpeed, UpgradeType grade)
    {
        this.grade = grade;
        this.life = life;
        this.moveSpeed = moveSpeed;
        this.shotSpeed = shotSpeed;
    }

    public void UpdateScore(int score)
    {
        this.score = score;
    }

}

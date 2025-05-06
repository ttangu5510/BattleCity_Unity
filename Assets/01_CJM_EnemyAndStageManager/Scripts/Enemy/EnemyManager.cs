using UnityEngine;

namespace CJM
{
    public class EnemyManager : MonoBehaviour
    {
        private static EnemyManager instance;
        public static EnemyManager Instance { get { return instance; } }

        //[Header("Common Setting")]
        [HideInInspector] public GameObject gameOverFlag;


        [Header("[Normal] Grade Setting")]
        [SerializeField] public int hp_N;
        [SerializeField] public float moveSpeed_N;
        [SerializeField] public float shotSpeed_N;
        [SerializeField] public float shotCycleRandomSeed_min_N;
        [SerializeField] public float shotCycleRandomSeed_max_N;

        [Header("[Normal_Fast] Grade Setting")]
        [SerializeField] public int hp_NF;
        [SerializeField] public float moveSpeed_NF;
        [SerializeField] public float shotSpeed_NF;
        [SerializeField] public float shotCycleRandomSeed_min_NF;
        [SerializeField] public float shotCycleRandomSeed_max_NF;

        [Header("[Normal_Strong] Grade Setting")]
        [SerializeField] public int hp_NS;
        [SerializeField] public float moveSpeed_NS;
        [SerializeField] public float shotSpeed_NS;
        [SerializeField] public float shotCycleRandomSeed_min_NS;
        [SerializeField] public float shotCycleRandomSeed_max_NS;

        [Header("[Elite] Grade Setting")]
        [SerializeField] public int hp_E;
        [SerializeField] public float moveSpeed_E;
        [SerializeField] public float shotSpeed_E;
        [SerializeField] public float shotCycleRandomSeed_min_E;
        [SerializeField] public float shotCycleRandomSeed_max_E;


        [Header("[Boss] Grade Setting")]
        [SerializeField] public int hp_B;
        [SerializeField] public float moveSpeed_B;
        [SerializeField] public float shotSpeed_B;
        [SerializeField] public float shotCycleRandomSeed_min_B;
        [SerializeField] public float shotCycleRandomSeed_max_B;


        [Header("ScoreData by Grade")]
        [SerializeField] public int score_Normal;
        [SerializeField] public int score_Normal_Fast;
        [SerializeField] public int score_Normal_Strong;
        [SerializeField] public int score_Elite;
        [SerializeField] public int score_Boss;


        private void Awake()
        {
            // 싱글톤 인스턴스 생성
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StatSetting(out int hp, out float moveSpeed, out float shotSpeed, out int score, out float seed1, out float seed2, EnemyGrade grade)
        {
            switch (grade)
            {
                case EnemyGrade.normal:
                    hp = hp_N;
                    moveSpeed = moveSpeed_N;
                    shotSpeed = shotSpeed_N;
                    score = score_Normal;
                    seed1 = shotCycleRandomSeed_min_N;
                    seed2 = shotCycleRandomSeed_max_N;
                    break;
                case EnemyGrade.normalFast:
                    hp = hp_NF;
                    moveSpeed = moveSpeed_NF;
                    shotSpeed = shotSpeed_NF;
                    score = score_Normal_Fast;
                    seed1 = shotCycleRandomSeed_min_NF;
                    seed2 = shotCycleRandomSeed_max_NF;
                    break;
                case EnemyGrade.normalStrong:
                    hp = hp_NS;
                    moveSpeed = moveSpeed_NS;
                    shotSpeed = shotSpeed_NS;
                    score = score_Normal_Strong;
                    seed1 = shotCycleRandomSeed_min_NS;
                    seed2 = shotCycleRandomSeed_max_NS;
                    break;
                case EnemyGrade.elite:
                    hp = hp_E;
                    moveSpeed = moveSpeed_E;
                    shotSpeed = shotSpeed_E;
                    score = score_Elite;
                    seed1 = shotCycleRandomSeed_min_E;
                    seed2 = shotCycleRandomSeed_max_E;
                    break;
                case EnemyGrade.boss:
                    hp = hp_B;
                    moveSpeed = moveSpeed_B;
                    shotSpeed = shotSpeed_B;
                    score = score_Boss;
                    seed1 = shotCycleRandomSeed_min_B;
                    seed2 = shotCycleRandomSeed_max_B;
                    break;
                default:
                    hp = 0;
                    moveSpeed = 0;
                    shotSpeed = 0;
                    score = 0;
                    seed1 = 0;
                    seed2 = 1;
                    break;
            }
        }


        public int GetSumPointByGrade(int slayCount, EnemyGrade grade)
        {
            int sumedScore = 0;
            switch (grade)
            {
                case EnemyGrade.normal:
                    sumedScore = slayCount * score_Normal;
                    break;
                case EnemyGrade.normalFast:
                    sumedScore = slayCount * score_Normal_Fast;
                    break;
                case EnemyGrade.normalStrong:
                    sumedScore = slayCount * score_Normal_Strong;
                    break;
                case EnemyGrade.elite:
                    sumedScore = slayCount * score_Elite;
                    break;
                case EnemyGrade.boss:
                    sumedScore = slayCount * score_Boss;
                    break;
            }
            return sumedScore;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace CJM
{
    public class StageData : MonoBehaviour
    {
        private StageManager sm;
        private UIManager um;

        [Header("스테이지 조건 저장")]
        [Tooltip("맵 안에 동시에 존재할 수 있는 최대 적 수")]
        [SerializeField] private int maxActiveEnemyCount;    // 맵 상에 동시에 존재할 수 있는 최대 적 수
        [Tooltip("처치해야 되는 몬스터 수 / 승리 조건")]
        [SerializeField] private int enemyLifeCount;         // 처치해야 되는 몬스터 수 / 승리 조건

        // 스테이지 매니저 싱글톤 인스턴스가 Awake에서 할당되므로 Start에서 정보 전달
        private void Start()
        {
            sm = StageManager.Instance;
            um = UIManager.Instance;

            // 스테이지 매니저 데이터 초기화 (이전 스테이지 적 리스트 정보)
            sm.StageDataInit();
            // 스테이지 매니저에 현재 데이터 동기화
            sm.SynchronizeStageData(maxActiveEnemyCount, enemyLifeCount);

            // UI에 처치 해야할 몬스터 수 정보 전달하기
            um.inGameUI_Instance.ShowEnemyLife();
        }
    }
}
using UnityEngine;

public class Test_GameManager : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            // 스테이지 컴플리트 테스트
            GameManager.Instance.StageComplete();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            // 게임 클리어 테스트
            GameManager.Instance.GameComplete();
        }
    }

}


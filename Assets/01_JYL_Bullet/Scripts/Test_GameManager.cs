using UnityEngine;

public class Test_GameManager : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            // �������� ���ø�Ʈ �׽�Ʈ
            GameManager.Instance.StageComplete();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            // ���� Ŭ���� �׽�Ʈ
            GameManager.Instance.GameComplete();
        }
    }

}


using UnityEngine;

public class Test_GameManager : MonoBehaviour
{
 
    void Start()
    {

        
    }

    // Update is called once per frame
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


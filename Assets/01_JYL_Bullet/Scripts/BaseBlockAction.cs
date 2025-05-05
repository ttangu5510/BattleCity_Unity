using System.Collections;
using UnityEngine;

// 맨 처음에 베이스 블록만 깜 ( 스크립트 포함) enum 초기블럭
// 스크립트에서 F 키 누르면 파괴 -> 2중베이스블록 생성(스크립트 포함)
// 2중베이스 블록은 기본 블록 비활성화(Awake) -> if State enum 무적 먹은 후
// Awake 에서 코루틴으로 yield return waitSec 으로 무적시간동안 멈춤
// 무적시간 끝난 후, 살아있는 블럭만 SetActive 교체

public class BaseBlockAction : MonoBehaviour
{
    enum BlockType { InitBlock, DoubleBlock }
    [SerializeField] BlockType blockType;
    [SerializeField] float InvincibleTime;
    WaitForSecondsRealtime waitSec;
    private void Awake()
    {
        waitSec = new WaitForSecondsRealtime(InvincibleTime);

        if (blockType == BlockType.DoubleBlock)
        {
            for (int i = 0; i < gameObject.transform.childCount - 1; i++)
            {
                Transform child = gameObject.transform.GetChild(i);
                if (child != null && child.childCount > 1)
                {
                    child.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            StartCoroutine(FortifyBase());
        }
    }

    IEnumerator FortifyBase()
    {
        yield return waitSec;
        for (int i = 0; i < gameObject.transform.childCount - 1; i++)
        {
            Transform child = gameObject.transform.GetChild(i);
            if (child != null && child.childCount > 1)
            {
                if (child.gameObject.transform.GetChild(1)!=null)
                {
                    child.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    child.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
}

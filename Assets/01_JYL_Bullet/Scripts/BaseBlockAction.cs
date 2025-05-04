using System.Collections;
using UnityEngine;

// �� ó���� ���̽� ��ϸ� �� ( ��ũ��Ʈ ����) enum �ʱ��
// ��ũ��Ʈ���� F Ű ������ �ı� -> 2�ߺ��̽���� ����(��ũ��Ʈ ����)
// 2�ߺ��̽� ����� �⺻ ��� ��Ȱ��ȭ(Awake) -> if State enum ���� ���� ��
// Awake ���� �ڷ�ƾ���� yield return waitSec ���� �����ð����� ����
// �����ð� ���� ��, ����ִ� ���� SetActive ��ü

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

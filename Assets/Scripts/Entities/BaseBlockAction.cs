using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if (child.gameObject.transform.GetChild(1) != null)
                {
                    child.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    child.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
}

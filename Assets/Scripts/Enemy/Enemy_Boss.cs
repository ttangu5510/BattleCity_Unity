using CJM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    [SerializeField] private Vector3 dir;
    [SerializeField] private float seedMin_DirChangeCycle;
    [SerializeField] private float seedMax_DirChangeCycle;
    [SerializeField] private float moveSpeed;
    public EnemyState state;

    Coroutine coroutine_stopItem;   // 같은부분 리팩토링 필요

    // 스테이지 매니저에 참조할 수 있는 공간 만들기. 일시정지용. 터렛들도 같이

    [SerializeField] GameObject onTriggerObj;
    Coroutine moveCycle;
    private void Start()
    {
        StageManager.Instance.boss = this;

        SwitchingDir();
        moveCycle = StartCoroutine(MovePattern());
    }


    public void SwitchingDir()
    {
        if (dir == Vector3.right)
            dir = Vector3.left;
        else if (dir == Vector3.left)
            dir = Vector3.right;
        else dir = Vector3.right;
    }
    public void Move(Vector3 dir)
    {
        if (state == EnemyState.Stop) return;

        transform.Translate(Time.deltaTime * moveSpeed * dir);
    }

    IEnumerator MovePattern()
    {
        bool flag = false;
        while (true)
        {
            if (state == EnemyState.Stop)
            {
                yield return new WaitUntil(() => state == EnemyState.General);
            }

            // 벽이 있는 경우
            if (onTriggerObj != null && !flag)
            {
                flag = true;
                SwitchingDir();
                Move(dir);
                yield return null;
                continue;
            }
            Move(dir);
            flag = false;
            yield return null;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            onTriggerObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            onTriggerObj = null;
        }
    }

    private void OnDisable()
    {
        if (moveCycle != null)
            StopCoroutine(moveCycle);
    }

    #region 일반 몬스터들과 같은 부분. 리팩토링 때 수정하자

    public void TimeStopItemEffect(float duration)
    {
        if (coroutine_stopItem == null)
            coroutine_stopItem = StartCoroutine(TimeStopItemEffectCycle(duration));
        else
        {
            StopCoroutine(coroutine_stopItem);
            coroutine_stopItem = StartCoroutine(TimeStopItemEffectCycle(duration));
        }
    }
    IEnumerator TimeStopItemEffectCycle(float duration)
    {
        state = EnemyState.Stop;
        yield return new WaitForSeconds(duration);
        state = EnemyState.General;
    }
    #endregion

}

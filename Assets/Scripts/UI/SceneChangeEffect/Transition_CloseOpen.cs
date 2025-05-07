using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_CloseOpen : TransitionSceneEffect
{

    [Header("ȭ�� ���� �����¿� �̹��� ����")]
    [SerializeField] RectTransform imageLeft;
    [SerializeField] RectTransform imageRight;
    [SerializeField] RectTransform imageUp;
    [SerializeField] RectTransform imageDown;
    [SerializeField] float closeOpenTime;
    [SerializeField] float waitTime;
    [SerializeField] TwinCloseOpenMode mode;



    Vector2 SidePosCenter = new Vector2(0, 0);
    Vector2 SidePosBottom;
    Vector2 SidePosUp;
    Vector2 SidePosLeft;   // ���� ���� ��ġ
    Vector2 SidePosRight;

    private void Start()
    {
        RectTransform canvasRect = transform.root.GetComponent<RectTransform>();

        SidePosBottom = new Vector2(0, -canvasRect.rect.height / 2);
        SidePosLeft = new Vector2(-canvasRect.rect.width / 2, 0);
        SidePosUp = -SidePosBottom;
        SidePosRight = -SidePosLeft;
    }


    public override IEnumerator TransitionPattern(string sceneName)
    {
        // 1. �ʱ� ����
        //*************************************//
        RectTransform rect_A;
        RectTransform rect_B;

        Vector2 rectA_Temp = Vector2.zero;
        Vector2 rectB_Temp = Vector2.zero;

        if (mode == TwinCloseOpenMode.Vertical)
        {
            rect_A = imageUp;
            rect_A.anchoredPosition = SidePosUp;

            rect_B = imageDown;
            rect_B.anchoredPosition = SidePosBottom;

        }
        else if (mode == TwinCloseOpenMode.Horizontal)
        {
            rect_A = imageLeft;
            rect_A.anchoredPosition = SidePosLeft;
            
            rect_B = imageRight;
            rect_B.anchoredPosition = SidePosRight;
        }
        else
        {
            rect_A = null;
            rect_B = null;
            Debug.LogError("��� ���� ����!");
        }
        rectA_Temp = rect_A.anchoredPosition;
        rectB_Temp = rect_B.anchoredPosition;
        //*************************************//

        // Ȥ�� ���� tween�� �����ִٸ� ����
        rect_A.DOKill();
        rect_B.DOKill();

        yield return null; // �� ������ �ѱ�� �ִϸ��̼� ���� (UI ���� ����ȭ)

        // 2. �ݱ� �ִϸ��̼�
        Sequence closeSeq = DOTween.Sequence();
        closeSeq.Append(rect_A.DOAnchorPos(SidePosCenter, closeOpenTime).SetEase(Ease.InOutQuad));
        closeSeq.Join(rect_B.DOAnchorPos(SidePosCenter, closeOpenTime).SetEase(Ease.InOutQuad));
        yield return closeSeq.WaitForCompletion();  // �ݵ�� ��ٷ��� ��

        // 3. �ε� �ؽ�Ʈ (���ϸ� �� ��ġ���� ó��)
        loadingText.text = sceneName;
        loadingText.gameObject.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        loadingText.gameObject.SetActive(false);

        // ���� �ڷ�ƾ ����

        // 4. �񵿱� �� �ε�
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitUntil(() => oper.isDone);  // �ε� ���� ������ ���

        // 5. ���� �ִϸ��̼�
        imageLeft.DOKill(); imageRight.DOKill();
        yield return null; // �� ������ ���
        Sequence openSeq = DOTween.Sequence();
        openSeq.Append(rect_A.DOAnchorPos(rectA_Temp, closeOpenTime).SetEase(Ease.InOutQuad));
        openSeq.Join(rect_B.DOAnchorPos(rectB_Temp, closeOpenTime).SetEase(Ease.InOutQuad));
        yield return openSeq.WaitForCompletion();
    }
}

enum TwinCloseOpenMode { Vertical, Horizontal }
using UnityEngine;

public class ScaleMatcher : MonoBehaviour
{
    [SerializeField] private Transform target; // ���� ������Ʈ A
    [SerializeField] private Transform targetToScale; // ���� ������Ʈ B
    [SerializeField] private float scaleMultiplier = 1f; // 2��, 4�� �� ���� ����

    void Start()
    {
        MatchScale();
    }

    void MatchScale()
    {
        Vector3 baseSize = target.GetComponent<Renderer>().bounds.size;
        Vector3 mySize = targetToScale.GetComponent<Renderer>().bounds.size;

        float scaleX = mySize.x != 0 ? baseSize.x / mySize.x : 1f;
        float scaleZ = mySize.z != 0 ? baseSize.z / mySize.z : 1f;

        // Y�� ���� �� �״�� ����
        Vector3 newScale = new Vector3(
            targetToScale.localScale.x * scaleX * scaleMultiplier,
            targetToScale.localScale.y,
            targetToScale.localScale.z * scaleZ * scaleMultiplier
        );

        targetToScale.localScale = newScale;
    }
}
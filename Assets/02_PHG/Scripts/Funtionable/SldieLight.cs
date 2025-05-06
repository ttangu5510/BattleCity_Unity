using UnityEngine;

public class SlideLight : MonoBehaviour
{
    public float swingAngle = 30f;      // �¿� �ִ� ���� (��: ��30��)
    public float swingSpeed = 2f;       // ���� �ӵ�

    private float initialYRotation;
    [SerializeField] private bool reverseSwing;


    void Start()
    {
        initialYRotation = transform.localEulerAngles.y;
    }

    void Update()
    {
        float t = Time.time * swingSpeed;
        if (reverseSwing) t *= -1;

        float angleOffset = Mathf.Sin(t) * swingAngle;
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            initialYRotation + angleOffset,
            transform.localEulerAngles.z
        );
    }
}

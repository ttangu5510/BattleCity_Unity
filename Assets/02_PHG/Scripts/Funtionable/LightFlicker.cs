using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickerLight;             // ��� ����
    public AudioSource sfxSource;
    public AudioClip flickerClip;
    public float minInterval = 0.05f;      // �ּ� ������ ����
    public float maxInterval = 5f;       // �ִ� ������ ����

    public float offDuration = 0.05f;      // ���� �ִ� �ð�
    private float timer;

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        StartCoroutine(FlickerRoutine());
    }

    System.Collections.IEnumerator FlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            flickerLight.enabled = false;
            if (sfxSource != null && flickerClip != null)
            {
                sfxSource.PlayOneShot(flickerClip);
            }
            yield return new WaitForSeconds(offDuration);
            flickerLight.enabled = true;
        }
    }
}
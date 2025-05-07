using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickerLight;             // ��� ����
    public AudioSource sfxSource;
    public AudioClip flickerClip;

    public float startDelay = 0f;          // ���� �� ������
    public float minInterval = 0.05f;      // �ּ� ������ ����
    public float maxInterval = 5f;         // �ִ� ������ ����
    public float offDuration = 0.05f;      // ���� �ִ� �ð�

    public bool startOn = true;            // ó���� ���� ������ ����

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        // ���� ���� ����
        flickerLight.enabled = startOn;

        StartCoroutine(FlickerRoutine());
    }

    private void PlayFlickerSound()
    {
        if (sfxSource && flickerClip)
        {
            sfxSource.PlayOneShot(flickerClip);
        }
    }

    private System.Collections.IEnumerator FlickerRoutine()
    {
        // ������ ����
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // ���� ���� �ݴ�� ��ȯ
            flickerLight.enabled = !flickerLight.enabled;
            PlayFlickerSound();

            // �������� offDuration, �������� ���� �ð� ���
            float waitTime = flickerLight.enabled ? Random.Range(minInterval, maxInterval) : offDuration;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
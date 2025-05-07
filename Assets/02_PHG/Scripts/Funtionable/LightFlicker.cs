using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickerLight;             // 대상 조명
    public AudioSource sfxSource;
    public AudioClip flickerClip;

    public float startDelay = 0f;          // 시작 전 딜레이
    public float minInterval = 0.05f;      // 최소 깜빡임 간격
    public float maxInterval = 5f;         // 최대 깜빡임 간격
    public float offDuration = 0.05f;      // 꺼져 있는 시간

    public bool startOn = true;            // 처음에 켜져 있을지 여부

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        // 시작 상태 설정
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
        // 딜레이 시작
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // 현재 상태 반대로 전환
            flickerLight.enabled = !flickerLight.enabled;
            PlayFlickerSound();

            // 꺼졌으면 offDuration, 켜졌으면 랜덤 시간 대기
            float waitTime = flickerLight.enabled ? Random.Range(minInterval, maxInterval) : offDuration;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
using UnityEngine;

public class EarthquakeEffect : MonoBehaviour
{
    public float shakeMagnitude = 0.05f;
    public float shakeSpeed = 0.5f;
    [SerializeField] private float shakeInterval = 5f; // 몇 초마다 흔들릴지
    [SerializeField] private float shakeDuration = 1f; // 흔들리는 시간
    public AudioSource quakeAudio;
    private float nextShakeTime;
    private float shakeTimer;
    private bool isShaking = false;
    private Vector3 originalPos;
    private float seed;

    void Start()
    {
        originalPos = transform.localPosition;
        seed = Random.Range(0f, 1000f);

        if (quakeAudio != null)
        {
            quakeAudio.Stop();         //  꼬인 상태 초기화
            quakeAudio.loop = true;    //  먼저 loop 설정
            quakeAudio.Play();         //  
        }
    }

    void Update()
    {
        if (isShaking)
        {
            shakeTimer -= Time.deltaTime;
            float x = (Mathf.PerlinNoise(seed, Time.time * shakeSpeed) - 0.5f) * 2f;
            float y = (Mathf.PerlinNoise(seed + 1, Time.time * shakeSpeed) - 0.5f) * 2f;
            transform.localPosition = originalPos + new Vector3(x, y, 0) * shakeMagnitude;

            if (shakeTimer <= 0f)
            {
                isShaking = false;
                transform.localPosition = originalPos;
            }
        }
        else
        {
            if (Time.time >= nextShakeTime)
            {
                isShaking = true;
                shakeTimer = shakeDuration;
                nextShakeTime = Time.time + shakeInterval;
            }
        }
    }
}
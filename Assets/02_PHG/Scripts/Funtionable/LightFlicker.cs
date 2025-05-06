using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickerLight;             // ¥ÎªÛ ¡∂∏Ì
    public AudioSource sfxSource;
    public AudioClip flickerClip;
    public float minInterval = 0.05f;      // √÷º“ ±Ù∫˝¿” ∞£∞›
    public float maxInterval = 5f;       // √÷¥Î ±Ù∫˝¿” ∞£∞›
    public bool startOn =  true;
    public float offDuration = 0.05f;      // ≤®¡Æ ¿÷¥¬ Ω√∞£
    [SerializeField] public float startDelay = 0f;

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();


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
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        flickerLight.enabled = !flickerLight.enabled;
        PlayFlickerSound();
        yield return new WaitForSeconds(offDuration);

        flickerLight.enabled = !flickerLight.enabled;
        PlayFlickerSound();

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            flickerLight.enabled = false;
            PlayFlickerSound();
            yield return new WaitForSeconds(offDuration);

            flickerLight.enabled = true;
            PlayFlickerSound();
        }
    }
}
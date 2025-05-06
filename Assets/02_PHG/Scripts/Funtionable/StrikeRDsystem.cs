using System.Collections;
using UnityEngine;

public class StrikeRDsystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private float minDelay = 0.5f;
    [SerializeField] private float maxDelay = 1.5f;
    private void Start()
    {
        StartCoroutine(PlayEffectLoop());
    }

    IEnumerator PlayEffectLoop()
    {
        while (true)
        {
            effect.Play(); // 이펙트 재생
            yield return new WaitForSeconds(effect.main.duration); // 현재 루프 재생 시간만큼 대기

            effect.Stop(); // 꺼줌
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime); // 랜덤 대기 후 다음 루프
        }
    }
}

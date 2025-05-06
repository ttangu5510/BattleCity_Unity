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
            effect.Play(); // ����Ʈ ���
            yield return new WaitForSeconds(effect.main.duration); // ���� ���� ��� �ð���ŭ ���

            effect.Stop(); // ����
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime); // ���� ��� �� ���� ����
        }
    }
}

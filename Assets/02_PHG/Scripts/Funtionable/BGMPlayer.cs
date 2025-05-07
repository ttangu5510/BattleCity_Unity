using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMPlayer : MonoBehaviour
{
    public AudioClip battleCityBGM;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().name == "Battle City")
        {
            audioSource.clip = battleCityBGM;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
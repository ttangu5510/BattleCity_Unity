using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverBtn : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] Button quitButton;

    public void ContinueGame()
    {
        GameManager.Instance.isChoice = true;
        GameManager.Instance.isContinue = true;
    }
    public void QuitGame()
    {
        GameManager.Instance.isChoice = true;


    }
}

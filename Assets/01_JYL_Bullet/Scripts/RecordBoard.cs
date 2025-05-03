using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecordBoard : MonoBehaviour
{
    public TMP_Text[] rank = new TMP_Text[10];
    private void Awake()
    {
        for (int i = 0; i < GameManager.Instance.scores.Length; i++)
        {
                rank[i].text = $"{i + 1} {GameManager.Instance.scores[i].name} {GameManager.Instance.scores[i].score}";
        }
    }
}

using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] RectTransform gameOverUI;
    Vector2 endPos;
    [SerializeField] RectTransform movePosition;
    [SerializeField] float moveSpeed = 2f;
    private void Awake()
    {
        endPos = movePosition.position;
    }
    private void Update()
    {
        gameOverUI.position = Vector2.MoveTowards(gameOverUI.position, endPos, moveSpeed * Time.deltaTime);
    }
}

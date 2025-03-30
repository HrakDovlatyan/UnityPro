using UnityEngine.SceneManagement;
using UnityEngine;
using ShootEmUp.Controllers;
using ShootEmUp.Systems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private EnemySystem enemySystem;

    private bool isGameOver = false;

    private void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        if (enemySystem != null)
        {
            enemySystem.OnAllEnemiesDefeated += OnAllEnemiesDefeated;
        }
    }

    private void OnDestroy()
    {
        if (enemySystem != null)
        {
            enemySystem.OnAllEnemiesDefeated -= OnAllEnemiesDefeated;
        }
    }

    public void FinishGame(bool victory = false)
    {
        if (isGameOver) return;

        isGameOver = true;

        if (victory)
        {
            if (victoryPanel != null) victoryPanel.SetActive(true);
        }
        else
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    private void OnAllEnemiesDefeated()
    {
        FinishGame(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

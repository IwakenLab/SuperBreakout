using UnityEngine;
using UnityEngine.SceneManagement;
using SuperBreakout.Player;
using SuperBreakout.Ball;
using SuperBreakout.Blocks;

namespace SuperBreakout.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private int initialLives = 3;
        [SerializeField] private float ballResetDelay = 1f;
        
        [Header("References")]
        [SerializeField] private PaddleController paddle;
        [SerializeField] private BallController ball;
        [SerializeField] private Transform ballStartPosition;
        
        [Header("UI References")]
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        [SerializeField] private TMPro.TextMeshProUGUI livesText;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TMPro.TextMeshProUGUI finalScoreText;
        
        private int currentScore = 0;
        private int currentLives;
        private int remainingBlocks;
        private bool isGameActive = true;
        
        private static GameManager instance;
        public static GameManager Instance => instance;
        
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        
        private void Start()
        {
            InitializeGame();
            SubscribeToEvents();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        
        private void InitializeGame()
        {
            currentLives = initialLives;
            currentScore = 0;
            isGameActive = true;
            
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);
            
            UpdateUI();
            CountRemainingBlocks();
            
            if (ballStartPosition == null && paddle != null)
            {
                GameObject ballStartObj = new GameObject("BallStartPosition");
                ballStartPosition = ballStartObj.transform;
                ballStartPosition.position = paddle.transform.position + Vector3.up * 0.5f;
            }
            
            ResetBallPosition();
        }
        
        private void SubscribeToEvents()
        {
            if (ball != null)
                ball.OnBallLost += HandleBallLost;
            
            Block.OnBlockDestroyed += HandleBlockDestroyed;
            Block.OnBlockHit += HandleBlockHit;
        }
        
        private void UnsubscribeFromEvents()
        {
            if (ball != null)
                ball.OnBallLost -= HandleBallLost;
            
            Block.OnBlockDestroyed -= HandleBlockDestroyed;
            Block.OnBlockHit -= HandleBlockHit;
        }
        
        private void Update()
        {
            if (!isGameActive)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    RestartGame();
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Application.Quit();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) && ball != null)
                {
                    ball.Launch();
                }
            }
        }
        
        private void HandleBallLost()
        {
            if (!isGameActive) return;
            
            currentLives--;
            UpdateUI();
            
            if (currentLives <= 0)
            {
                GameOver();
            }
            else
            {
                Invoke(nameof(ResetBallPosition), ballResetDelay);
            }
        }
        
        private void HandleBlockDestroyed(int score)
        {
            if (!isGameActive) return;
            
            currentScore += score;
            remainingBlocks--;
            UpdateUI();
            
            if (remainingBlocks <= 0)
            {
                LevelComplete();
            }
        }
        
        private void HandleBlockHit()
        {
            // Additional effects or sounds can be added here
        }
        
        private void ResetBallPosition()
        {
            if (ball != null && ballStartPosition != null)
            {
                ball.ResetBall(ballStartPosition.position);
            }
        }
        
        private void UpdateUI()
        {
            if (scoreText != null)
                scoreText.text = "Score: " + currentScore;
            
            if (livesText != null)
                livesText.text = "Lives: " + currentLives;
        }
        
        private void CountRemainingBlocks()
        {
            remainingBlocks = FindObjectsOfType<Block>().Length;
        }
        
        private void GameOver()
        {
            isGameActive = false;
            
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                if (finalScoreText != null)
                    finalScoreText.text = "Final Score: " + currentScore;
            }
        }
        
        private void LevelComplete()
        {
            isGameActive = false;
            // In a full game, this would load the next level
            // For now, we'll just show the game over screen with a victory message
            GameOver();
        }
        
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
        
        public int GetScore()
        {
            return currentScore;
        }
        
        public int GetLives()
        {
            return currentLives;
        }
        
        public bool IsGameActive()
        {
            return isGameActive;
        }
    }
}
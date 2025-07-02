using UnityEngine;
using UnityEngine.SceneManagement;
using SuperBreakout.Player;
using SuperBreakout.Ball;
using UnityEngine.Serialization;
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
        [FormerlySerializedAs("ball")]
        [SerializeField] private MonoBehaviour ballController; // BallController or BallController3D
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
            if (ballController != null)
            {
                if (ballController is BallController bc)
                    bc.OnBallLost += HandleBallLost;
                else if (ballController is BallController3D bc3d)
                    bc3d.OnBallLost += HandleBallLost;
            }
            
            Block.OnBlockDestroyed += HandleBlockDestroyed;
            Block.OnBlockHit += HandleBlockHit;
        }
        
        private void UnsubscribeFromEvents()
        {
            if (ballController != null)
            {
                if (ballController is BallController bc)
                    bc.OnBallLost -= HandleBallLost;
                else if (ballController is BallController3D bc3d)
                    bc3d.OnBallLost -= HandleBallLost;
            }
            
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
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Space key pressed! Ball: " + (ballController != null ? "exists" : "null"));
                    if (ballController != null)
                    {
                        if (ballController is BallController bc)
                            bc.Launch();
                        else if (ballController is BallController3D bc3d)
                            bc3d.Launch();
                    }
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
            if (ballController != null && ballStartPosition != null)
            {
                if (ballController is BallController bc)
                    bc.ResetBall(ballStartPosition.position);
                else if (ballController is BallController3D bc3d)
                    bc3d.ResetBall(ballStartPosition.position);
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
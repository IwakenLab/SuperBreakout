using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperBreakout.Player;
using SuperBreakout.Ball;
using SuperBreakout.Blocks;

namespace SuperBreakout.Core
{
    public class GameSetup : MonoBehaviour
    {
        [Header("Game Objects")]
        public GameObject playerObject;
        public GameObject ballObject;
        public GameObject blockSpawnerObject;
        public GameObject gameManagerObject;
        
        [Header("UI Setup")]
        public GameObject canvasObject;
        
        private void Awake()
        {
            SetupGameObjects();
            SetupUI();
        }
        
        private void SetupGameObjects()
        {
            // Setup Player
            if (playerObject != null)
            {
                playerObject.tag = TagManager.Player;
                if (playerObject.GetComponent<PaddleController>() == null)
                    playerObject.AddComponent<PaddleController>();
                if (playerObject.GetComponent<BoxCollider2D>() == null)
                    playerObject.AddComponent<BoxCollider2D>();
            }
            
            // Setup Ball
            if (ballObject != null)
            {
                ballObject.tag = TagManager.Ball;
                if (ballObject.GetComponent<BallController>() == null)
                    ballObject.AddComponent<BallController>();
                if (ballObject.GetComponent<CircleCollider2D>() == null)
                {
                    var collider = ballObject.AddComponent<CircleCollider2D>();
                    collider.isTrigger = true;
                }
            }
            
            // Setup Block Spawner
            if (blockSpawnerObject != null)
            {
                if (blockSpawnerObject.GetComponent<BlockSpawner>() == null)
                    blockSpawnerObject.AddComponent<BlockSpawner>();
            }
            
            // Setup GameManager
            if (gameManagerObject != null)
            {
                var gameManager = gameManagerObject.GetComponent<GameManager>();
                if (gameManager == null)
                    gameManager = gameManagerObject.AddComponent<GameManager>();
                
                // Connect references
                if (playerObject != null && ballObject != null)
                {
                    var paddleController = playerObject.GetComponent<PaddleController>();
                    var ballController = ballObject.GetComponent<BallController>();
                    
                    // Use reflection or make fields public in GameManager to set these
                    // For now, we'll need to set these manually in the Unity editor
                }
            }
            
            // Setup Walls
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach (var wall in walls)
            {
                if (wall.GetComponent<BoxCollider2D>() == null)
                {
                    var collider = wall.AddComponent<BoxCollider2D>();
                    collider.isTrigger = true;
                }
            }
        }
        
        private void SetupUI()
        {
            if (canvasObject == null) return;
            
            // Create Score Text
            GameObject scoreObj = new GameObject("ScoreText");
            scoreObj.transform.SetParent(canvasObject.transform, false);
            var scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
            scoreText.text = "Score: 0";
            scoreText.fontSize = 36;
            scoreText.alignment = TextAlignmentOptions.Left;
            
            RectTransform scoreRect = scoreObj.GetComponent<RectTransform>();
            scoreRect.anchorMin = new Vector2(0, 1);
            scoreRect.anchorMax = new Vector2(0, 1);
            scoreRect.pivot = new Vector2(0, 1);
            scoreRect.anchoredPosition = new Vector2(20, -20);
            scoreRect.sizeDelta = new Vector2(300, 50);
            
            // Create Lives Text
            GameObject livesObj = new GameObject("LivesText");
            livesObj.transform.SetParent(canvasObject.transform, false);
            var livesText = livesObj.AddComponent<TextMeshProUGUI>();
            livesText.text = "Lives: 3";
            livesText.fontSize = 36;
            livesText.alignment = TextAlignmentOptions.Right;
            
            RectTransform livesRect = livesObj.GetComponent<RectTransform>();
            livesRect.anchorMin = new Vector2(1, 1);
            livesRect.anchorMax = new Vector2(1, 1);
            livesRect.pivot = new Vector2(1, 1);
            livesRect.anchoredPosition = new Vector2(-20, -20);
            livesRect.sizeDelta = new Vector2(300, 50);
            
            // Create Game Over Panel
            GameObject gameOverPanel = new GameObject("GameOverPanel");
            gameOverPanel.transform.SetParent(canvasObject.transform, false);
            var panelImage = gameOverPanel.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.8f);
            
            RectTransform panelRect = gameOverPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            
            // Game Over Text
            GameObject gameOverText = new GameObject("GameOverText");
            gameOverText.transform.SetParent(gameOverPanel.transform, false);
            var goText = gameOverText.AddComponent<TextMeshProUGUI>();
            goText.text = "GAME OVER";
            goText.fontSize = 72;
            goText.alignment = TextAlignmentOptions.Center;
            
            RectTransform goRect = gameOverText.GetComponent<RectTransform>();
            goRect.anchorMin = new Vector2(0.5f, 0.6f);
            goRect.anchorMax = new Vector2(0.5f, 0.6f);
            goRect.pivot = new Vector2(0.5f, 0.5f);
            goRect.anchoredPosition = Vector2.zero;
            goRect.sizeDelta = new Vector2(600, 100);
            
            // Final Score Text
            GameObject finalScoreText = new GameObject("FinalScoreText");
            finalScoreText.transform.SetParent(gameOverPanel.transform, false);
            var fsText = finalScoreText.AddComponent<TextMeshProUGUI>();
            fsText.text = "Final Score: 0";
            fsText.fontSize = 48;
            fsText.alignment = TextAlignmentOptions.Center;
            
            RectTransform fsRect = finalScoreText.GetComponent<RectTransform>();
            fsRect.anchorMin = new Vector2(0.5f, 0.5f);
            fsRect.anchorMax = new Vector2(0.5f, 0.5f);
            fsRect.pivot = new Vector2(0.5f, 0.5f);
            fsRect.anchoredPosition = Vector2.zero;
            fsRect.sizeDelta = new Vector2(600, 60);
            
            // Restart Instructions
            GameObject restartText = new GameObject("RestartText");
            restartText.transform.SetParent(gameOverPanel.transform, false);
            var rsText = restartText.AddComponent<TextMeshProUGUI>();
            rsText.text = "Press R to Restart\nPress ESC to Quit";
            rsText.fontSize = 32;
            rsText.alignment = TextAlignmentOptions.Center;
            
            RectTransform rsRect = restartText.GetComponent<RectTransform>();
            rsRect.anchorMin = new Vector2(0.5f, 0.3f);
            rsRect.anchorMax = new Vector2(0.5f, 0.3f);
            rsRect.pivot = new Vector2(0.5f, 0.5f);
            rsRect.anchoredPosition = Vector2.zero;
            rsRect.sizeDelta = new Vector2(400, 100);
            
            gameOverPanel.SetActive(false);
        }
    }
}
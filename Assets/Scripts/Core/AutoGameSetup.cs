using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperBreakout.Player;
using SuperBreakout.Ball;
using SuperBreakout.Blocks;

namespace SuperBreakout.Core
{
    public class AutoGameSetup : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("AutoGameSetup: Starting automatic setup...");
            
            SetupTags();
            SetupPlayer();
            SetupBall();
            SetupWalls();
            SetupDeathZone();
            SetupBlockSpawner();
            SetupUI();
            SetupGameManager();
            
            Debug.Log("AutoGameSetup: Setup completed!");
        }
        
        private void SetupTags()
        {
            // Create tags if they don't exist (Unity doesn't allow runtime tag creation)
            // So we'll use the tags that exist and set them on objects
        }
        
        private void SetupPlayer()
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                player.tag = "Player";
                
                // Add PaddleController
                if (player.GetComponent<PaddleController>() == null)
                {
                    var paddle = player.AddComponent<PaddleController>();
                    Debug.Log("Added PaddleController to Player");
                }
                
                // Add BoxCollider2D
                if (player.GetComponent<BoxCollider2D>() == null)
                {
                    player.AddComponent<BoxCollider2D>();
                    Debug.Log("Added BoxCollider2D to Player");
                }
                
                // Remove MeshCollider if it exists
                MeshCollider meshCol = player.GetComponent<MeshCollider>();
                if (meshCol != null)
                {
                    Destroy(meshCol);
                }
            }
        }
        
        private void SetupBall()
        {
            GameObject ball = GameObject.Find("Ball");
            if (ball != null)
            {
                ball.tag = "Ball";
                
                // Add BallController
                if (ball.GetComponent<BallController>() == null)
                {
                    var ballCtrl = ball.AddComponent<BallController>();
                    Debug.Log("Added BallController to Ball");
                }
                
                // Add CircleCollider2D
                if (ball.GetComponent<CircleCollider2D>() == null)
                {
                    var col = ball.AddComponent<CircleCollider2D>();
                    col.isTrigger = true;
                    Debug.Log("Added CircleCollider2D to Ball");
                }
                
                // Remove MeshCollider if it exists
                MeshCollider meshCol = ball.GetComponent<MeshCollider>();
                if (meshCol != null)
                {
                    Destroy(meshCol);
                }
            }
        }
        
        private void SetupWalls()
        {
            string[] wallNames = { "LeftWall", "RightWall", "TopWall" };
            
            foreach (string wallName in wallNames)
            {
                GameObject wall = GameObject.Find(wallName);
                if (wall != null)
                {
                    wall.tag = "Wall";
                    
                    // Add BoxCollider2D
                    if (wall.GetComponent<BoxCollider2D>() == null)
                    {
                        var col = wall.AddComponent<BoxCollider2D>();
                        col.isTrigger = true;
                        Debug.Log($"Added BoxCollider2D to {wallName}");
                    }
                    
                    // Remove MeshCollider if it exists
                    MeshCollider meshCol = wall.GetComponent<MeshCollider>();
                    if (meshCol != null)
                    {
                        Destroy(meshCol);
                    }
                }
            }
        }
        
        private void SetupDeathZone()
        {
            GameObject deathZone = GameObject.Find("DeathZone");
            if (deathZone != null)
            {
                deathZone.tag = "DeathZone";
                
                // Add BoxCollider2D
                if (deathZone.GetComponent<BoxCollider2D>() == null)
                {
                    var col = deathZone.AddComponent<BoxCollider2D>();
                    col.isTrigger = true;
                    Debug.Log("Added BoxCollider2D to DeathZone");
                }
                
                // Remove MeshCollider if it exists
                MeshCollider meshCol = deathZone.GetComponent<MeshCollider>();
                if (meshCol != null)
                {
                    Destroy(meshCol);
                }
                
                // Make invisible
                MeshRenderer renderer = deathZone.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }
        
        private void SetupBlockSpawner()
        {
            GameObject blockSpawner = GameObject.Find("BlockSpawner");
            if (blockSpawner != null)
            {
                if (blockSpawner.GetComponent<BlockSpawner>() == null)
                {
                    var spawner = blockSpawner.AddComponent<BlockSpawner>();
                    Debug.Log("Added BlockSpawner component");
                }
            }
        }
        
        private void SetupUI()
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.LogError("Canvas not found!");
                return;
            }
            
            // Add CanvasScaler
            if (canvas.GetComponent<CanvasScaler>() == null)
            {
                var scaler = canvas.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
            }
            
            // Add GraphicRaycaster
            if (canvas.GetComponent<GraphicRaycaster>() == null)
            {
                canvas.AddComponent<GraphicRaycaster>();
            }
            
            // Create EventSystem if it doesn't exist
            if (GameObject.Find("EventSystem") == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
            
            // Create UI elements
            CreateScoreText(canvas);
            CreateLivesText(canvas);
            CreateGameOverPanel(canvas);
        }
        
        private void CreateScoreText(GameObject canvas)
        {
            if (GameObject.Find("ScoreText") != null) return;
            
            GameObject scoreObj = new GameObject("ScoreText");
            scoreObj.transform.SetParent(canvas.transform, false);
            
            var scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
            scoreText.text = "Score: 0";
            scoreText.fontSize = 36;
            scoreText.color = Color.white;
            scoreText.alignment = TextAlignmentOptions.Left;
            
            RectTransform rect = scoreObj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(20, -20);
            rect.sizeDelta = new Vector2(300, 50);
        }
        
        private void CreateLivesText(GameObject canvas)
        {
            if (GameObject.Find("LivesText") != null) return;
            
            GameObject livesObj = new GameObject("LivesText");
            livesObj.transform.SetParent(canvas.transform, false);
            
            var livesText = livesObj.AddComponent<TextMeshProUGUI>();
            livesText.text = "Lives: 3";
            livesText.fontSize = 36;
            livesText.color = Color.white;
            livesText.alignment = TextAlignmentOptions.Right;
            
            RectTransform rect = livesObj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(1, 1);
            rect.anchoredPosition = new Vector2(-20, -20);
            rect.sizeDelta = new Vector2(300, 50);
        }
        
        private void CreateGameOverPanel(GameObject canvas)
        {
            if (GameObject.Find("GameOverPanel") != null) return;
            
            GameObject panel = new GameObject("GameOverPanel");
            panel.transform.SetParent(canvas.transform, false);
            
            var image = panel.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.8f);
            
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            
            // Game Over Text
            GameObject goTextObj = new GameObject("GameOverText");
            goTextObj.transform.SetParent(panel.transform, false);
            
            var goText = goTextObj.AddComponent<TextMeshProUGUI>();
            goText.text = "GAME OVER";
            goText.fontSize = 72;
            goText.color = Color.white;
            goText.alignment = TextAlignmentOptions.Center;
            
            RectTransform goRect = goTextObj.GetComponent<RectTransform>();
            goRect.anchorMin = new Vector2(0.5f, 0.6f);
            goRect.anchorMax = new Vector2(0.5f, 0.6f);
            goRect.pivot = new Vector2(0.5f, 0.5f);
            goRect.anchoredPosition = Vector2.zero;
            goRect.sizeDelta = new Vector2(600, 100);
            
            // Final Score Text
            GameObject fsTextObj = new GameObject("FinalScoreText");
            fsTextObj.transform.SetParent(panel.transform, false);
            
            var fsText = fsTextObj.AddComponent<TextMeshProUGUI>();
            fsText.text = "Final Score: 0";
            fsText.fontSize = 48;
            fsText.color = Color.white;
            fsText.alignment = TextAlignmentOptions.Center;
            
            RectTransform fsRect = fsTextObj.GetComponent<RectTransform>();
            fsRect.anchorMin = new Vector2(0.5f, 0.5f);
            fsRect.anchorMax = new Vector2(0.5f, 0.5f);
            fsRect.pivot = new Vector2(0.5f, 0.5f);
            fsRect.anchoredPosition = Vector2.zero;
            fsRect.sizeDelta = new Vector2(600, 60);
            
            // Instructions
            GameObject instTextObj = new GameObject("InstructionsText");
            instTextObj.transform.SetParent(panel.transform, false);
            
            var instText = instTextObj.AddComponent<TextMeshProUGUI>();
            instText.text = "Press R to Restart\nPress ESC to Quit";
            instText.fontSize = 32;
            instText.color = Color.white;
            instText.alignment = TextAlignmentOptions.Center;
            
            RectTransform instRect = instTextObj.GetComponent<RectTransform>();
            instRect.anchorMin = new Vector2(0.5f, 0.3f);
            instRect.anchorMax = new Vector2(0.5f, 0.3f);
            instRect.pivot = new Vector2(0.5f, 0.5f);
            instRect.anchoredPosition = Vector2.zero;
            instRect.sizeDelta = new Vector2(400, 100);
            
            panel.SetActive(false);
        }
        
        private void SetupGameManager()
        {
            GameObject gmObj = GameObject.Find("GameManager");
            if (gmObj == null)
            {
                Debug.LogError("GameManager object not found!");
                return;
            }
            
            GameManager gm = gmObj.GetComponent<GameManager>();
            if (gm == null)
            {
                gm = gmObj.AddComponent<GameManager>();
                Debug.Log("Added GameManager component");
            }
            
            // Find and assign references using reflection or make them public
            // For now, we'll need to manually assign in Unity Editor
            // But we can try to find the UI elements
            
            var scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
            var livesText = GameObject.Find("LivesText")?.GetComponent<TextMeshProUGUI>();
            var gameOverPanel = GameObject.Find("GameOverPanel");
            var finalScoreText = GameObject.Find("FinalScoreText")?.GetComponent<TextMeshProUGUI>();
            
            // Unfortunately, we can't set private serialized fields at runtime
            // The user will need to assign these in the Unity Editor
            
            Debug.Log("GameManager setup complete. Please assign references in Unity Editor:");
            Debug.Log("- Paddle: Player GameObject");
            Debug.Log("- Ball: Ball GameObject");
            Debug.Log("- Score Text: ScoreText");
            Debug.Log("- Lives Text: LivesText");
            Debug.Log("- Game Over Panel: GameOverPanel");
            Debug.Log("- Final Score Text: FinalScoreText");
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperBreakout.Player;
using SuperBreakout.Ball;

namespace SuperBreakout.Core
{
    public class GameInitializer : MonoBehaviour
    {
        void Start()
        {
            InitializeSprites();
            SetupTags();
            SetupGameManager();
            Debug.Log("Game initialization complete!");
        }
        
        void InitializeSprites()
        {
            // Player sprite
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite == null)
                {
                    sr.sprite = SpriteHelper.CreateSquareSprite();
                    sr.color = new Color(0.2f, 0.5f, 1.0f);
                }
            }
            
            // Ball sprite
            GameObject ball = GameObject.Find("Ball");
            if (ball != null)
            {
                SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite == null)
                {
                    sr.sprite = SpriteHelper.CreateCircleSprite();
                    sr.color = Color.white;
                }
                
                // Scale the ball appropriately
                ball.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            }
            
            // Make DeathZone invisible
            GameObject deathZone = GameObject.Find("DeathZone");
            if (deathZone != null)
            {
                MeshRenderer mr = deathZone.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.enabled = false;
                }
            }
        }
        
        void SetupTags()
        {
            // Set tags (Unity's built-in tags)
            GameObject player = GameObject.Find("Player");
            if (player != null) player.tag = "Player";
            
            GameObject ball = GameObject.Find("Ball");
            if (ball != null) ball.tag = "Respawn"; // Using Respawn as Ball tag
            
            GameObject deathZone = GameObject.Find("DeathZone");
            if (deathZone != null) deathZone.tag = "Finish"; // Using Finish as DeathZone tag
            
            // Walls use "EditorOnly" tag as Wall tag
            GameObject[] walls = { GameObject.Find("LeftWall"), GameObject.Find("RightWall"), GameObject.Find("TopWall") };
            foreach (var wall in walls)
            {
                if (wall != null) wall.tag = "EditorOnly";
            }
        }
        
        void SetupGameManager()
        {
            GameObject gmObj = GameObject.Find("GameManager");
            if (gmObj == null) return;
            
            GameManager gm = gmObj.GetComponent<GameManager>();
            if (gm == null) return;
            
            // Create a simple UI for testing
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                // Add CanvasScaler if not present
                CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
                if (scaler == null)
                {
                    scaler = canvas.AddComponent<CanvasScaler>();
                    scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    scaler.referenceResolution = new Vector2(1920, 1080);
                }
                
                // Add GraphicRaycaster if not present
                if (canvas.GetComponent<GraphicRaycaster>() == null)
                {
                    canvas.AddComponent<GraphicRaycaster>();
                }
            }
            
            Debug.Log("GameManager setup complete. Please connect references in Inspector:");
            Debug.Log("- Paddle: Player GameObject with PaddleController");
            Debug.Log("- Ball: Ball GameObject with BallController");
            Debug.Log("- Ball Start Position: Create empty GameObject at (0, -3.5, 0)");
            Debug.Log("- UI references can be left empty for now");
        }
    }
}
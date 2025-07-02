using UnityEngine;
using SuperBreakout.Ball;

namespace SuperBreakout.Core
{
    public class Setup3D : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("Starting 3D Setup...");
            
            // Ballの設定
            GameObject ball = GameObject.Find("Ball");
            if (ball != null)
            {
                // 古い2D BallControllerを削除
                BallController oldController = ball.GetComponent<BallController>();
                if (oldController != null)
                {
                    Destroy(oldController);
                    Debug.Log("Removed old BallController");
                }
                
                // 新しい3D BallControllerを追加
                BallController3D newController = ball.GetComponent<BallController3D>();
                if (newController == null)
                {
                    newController = ball.AddComponent<BallController3D>();
                    Debug.Log("Added BallController3D");
                }
                
                // 2D物理を削除
                Rigidbody2D rb2d = ball.GetComponent<Rigidbody2D>();
                if (rb2d != null)
                {
                    Destroy(rb2d);
                    Debug.Log("Removed Rigidbody2D");
                }
                
                CircleCollider2D cc2d = ball.GetComponent<CircleCollider2D>();
                if (cc2d != null)
                {
                    Destroy(cc2d);
                    Debug.Log("Removed CircleCollider2D");
                }
                
                // 3D物理を確認/追加
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = ball.AddComponent<Rigidbody>();
                }
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                
                SphereCollider sc = ball.GetComponent<SphereCollider>();
                if (sc == null)
                {
                    sc = ball.AddComponent<SphereCollider>();
                }
                sc.isTrigger = true;
                
                ball.tag = "Ball";
                Debug.Log("Ball setup complete");
            }
            
            // Playerの設定
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                // 2Dコライダーを削除
                BoxCollider2D bc2d = player.GetComponent<BoxCollider2D>();
                if (bc2d != null)
                {
                    Destroy(bc2d);
                    Debug.Log("Removed BoxCollider2D from Player");
                }
                
                // 3Dコライダーを確認
                BoxCollider bc = player.GetComponent<BoxCollider>();
                if (bc == null)
                {
                    bc = player.AddComponent<BoxCollider>();
                }
                bc.isTrigger = false;
                
                player.tag = "Player";
            }
            
            // 壁の設定を修正
            FixWalls();
            
            // GameManagerの参照を更新
            UpdateGameManagerReferences();
            
            Debug.Log("3D Setup complete!");
        }
        
        void FixWalls()
        {
            string[] wallNames = { "LeftWall", "RightWall", "TopWall" };
            foreach (string wallName in wallNames)
            {
                GameObject wall = GameObject.Find(wallName);
                if (wall != null)
                {
                    // 2Dコライダーを削除
                    BoxCollider2D bc2d = wall.GetComponent<BoxCollider2D>();
                    if (bc2d != null)
                    {
                        Destroy(bc2d);
                        Debug.Log($"Removed BoxCollider2D from {wallName}");
                    }
                    
                    // 3Dコライダーを確認
                    BoxCollider bc = wall.GetComponent<BoxCollider>();
                    if (bc == null)
                    {
                        bc = wall.AddComponent<BoxCollider>();
                    }
                    bc.isTrigger = true;
                    
                    // タグを修正
                    wall.tag = "Wall";
                    Debug.Log($"Fixed {wallName} tag to 'Wall'");
                }
            }
        }
        
        void UpdateGameManagerReferences()
        {
            GameObject gmObj = GameObject.Find("GameManager");
            if (gmObj != null)
            {
                GameManager gm = gmObj.GetComponent<GameManager>();
                if (gm != null)
                {
                    // リフレクションまたはpublicフィールドを使用して参照を更新
                    // 手動で設定する必要があります
                    Debug.Log("Please update GameManager ball reference to use BallController3D");
                }
            }
        }
    }
}
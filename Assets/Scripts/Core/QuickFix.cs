using UnityEngine;

namespace SuperBreakout.Core
{
    // 一時的な修正スクリプト - 3D/2Dの混在問題を解決
    public class QuickFix : MonoBehaviour
    {
        void Start()
        {
            // BallのRigidbody2Dを正しく設定
            GameObject ball = GameObject.Find("Ball");
            if (ball != null)
            {
                Rigidbody2D rb2d = ball.GetComponent<Rigidbody2D>();
                if (rb2d != null)
                {
                    rb2d.bodyType = RigidbodyType2D.Kinematic;
                    rb2d.gravityScale = 0f;
                    Debug.Log("Fixed Ball Rigidbody2D settings");
                }
                
                // CircleCollider2Dを確認
                CircleCollider2D cc2d = ball.GetComponent<CircleCollider2D>();
                if (cc2d != null)
                {
                    cc2d.isTrigger = true;
                    Debug.Log("Fixed Ball CircleCollider2D to trigger");
                }
                
                // 3Dコライダーがあれば削除
                SphereCollider sc = ball.GetComponent<SphereCollider>();
                if (sc != null)
                {
                    Destroy(sc);
                    Debug.Log("Removed 3D SphereCollider from Ball");
                }
                
                // タグを設定
                ball.tag = "Ball";
            }
            
            // Playerの修正
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                // BoxCollider2Dを確認
                BoxCollider2D bc2d = player.GetComponent<BoxCollider2D>();
                if (bc2d == null)
                {
                    bc2d = player.AddComponent<BoxCollider2D>();
                    bc2d.size = new Vector2(1f, 1f);
                    Debug.Log("Added BoxCollider2D to Player");
                }
                
                // 3Dコライダーがあれば削除
                BoxCollider bc = player.GetComponent<BoxCollider>();
                if (bc != null)
                {
                    Destroy(bc);
                    Debug.Log("Removed 3D BoxCollider from Player");
                }
            }
            
            // 壁のタグを確認
            CheckWallTags();
            
            // DeathZoneのコライダーを修正
            GameObject deathZone = GameObject.Find("DeathZone");
            if (deathZone != null)
            {
                BoxCollider2D bc2d = deathZone.GetComponent<BoxCollider2D>();
                if (bc2d == null)
                {
                    bc2d = deathZone.AddComponent<BoxCollider2D>();
                    bc2d.size = new Vector2(20f, 1f);
                    bc2d.isTrigger = true;
                    Debug.Log("Added BoxCollider2D to DeathZone");
                }
                
                // 3DコライダーがあればisTriggerを設定
                BoxCollider bc = deathZone.GetComponent<BoxCollider>();
                if (bc != null)
                {
                    bc.isTrigger = true;
                }
            }
        }
        
        void CheckWallTags()
        {
            string[] wallNames = { "LeftWall", "RightWall", "TopWall" };
            foreach (string wallName in wallNames)
            {
                GameObject wall = GameObject.Find(wallName);
                if (wall != null && wall.tag != "Wall")
                {
                    Debug.LogWarning($"{wallName} tag is '{wall.tag}', should be 'Wall'");
                }
            }
        }
    }
}
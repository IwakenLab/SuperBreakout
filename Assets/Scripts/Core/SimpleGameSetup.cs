using UnityEngine;

namespace SuperBreakout.Core
{
    public class SimpleGameSetup : MonoBehaviour
    {
        private void Start()
        {
            // Set up tags for existing objects
            GameObject player = GameObject.Find("Player");
            if (player != null) player.tag = "Player";
            
            GameObject ball = GameObject.Find("Ball");
            if (ball != null) ball.tag = "Ball";
            
            GameObject deathZone = GameObject.Find("DeathZone");
            if (deathZone != null) deathZone.tag = "DeathZone";
            
            // Tag all walls
            GameObject leftWall = GameObject.Find("LeftWall");
            if (leftWall != null) leftWall.tag = "Wall";
            
            GameObject rightWall = GameObject.Find("RightWall");
            if (rightWall != null) rightWall.tag = "Wall";
            
            GameObject topWall = GameObject.Find("TopWall");
            if (topWall != null) topWall.tag = "Wall";
            
            Debug.Log("Simple Game Setup completed!");
        }
    }
}
using UnityEngine;

namespace SuperBreakout.Core
{
    public class CollisionDebugger : MonoBehaviour
    {
        void Start()
        {
            // ボールの設定を確認
            GameObject ball = GameObject.Find("Ball");
            if (ball != null)
            {
                SphereCollider sc = ball.GetComponent<SphereCollider>();
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                
                Debug.Log($"Ball - isTrigger: {sc?.isTrigger}, isKinematic: {rb?.isKinematic}, tag: {ball.tag}");
            }
            
            // ブロックの設定を確認
            Invoke(nameof(CheckBlocks), 1f);
        }
        
        void CheckBlocks()
        {
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
            Debug.Log($"Found {blocks.Length} blocks with 'Block' tag");
            
            if (blocks.Length > 0)
            {
                GameObject firstBlock = blocks[0];
                BoxCollider bc = firstBlock.GetComponent<BoxCollider>();
                Debug.Log($"First block - isTrigger: {bc?.isTrigger}, tag: {firstBlock.tag}");
            }
        }
    }
}
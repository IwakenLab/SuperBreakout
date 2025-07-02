using UnityEngine;
using SuperBreakout.Blocks;

namespace SuperBreakout.Core
{
    public class BlockFixer : MonoBehaviour
    {
        void Start()
        {
            Invoke(nameof(FixBlocks), 0.5f); // BlockSpawnerの後に実行
        }
        
        void FixBlocks()
        {
            Block[] blocks = FindObjectsOfType<Block>();
            Debug.Log($"Found {blocks.Length} blocks to fix");
            
            foreach (Block block in blocks)
            {
                GameObject blockObj = block.gameObject;
                
                // タグを設定
                blockObj.tag = "Block";
                
                // BoxColliderを確認
                BoxCollider bc = blockObj.GetComponent<BoxCollider>();
                if (bc == null)
                {
                    bc = blockObj.AddComponent<BoxCollider>();
                    Debug.Log($"Added BoxCollider to {blockObj.name}");
                }
                
                // isTriggerを設定
                bc.isTrigger = true;
                
                // サイズを調整（SpriteRendererに合わせる）
                SpriteRenderer sr = blockObj.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    bc.size = new Vector3(sr.sprite.bounds.size.x, sr.sprite.bounds.size.y, 0.1f);
                }
                else
                {
                    // デフォルトサイズ
                    bc.size = new Vector3(0.9f, 0.4f, 0.1f);
                }
                
                Debug.Log($"Fixed block {blockObj.name}: tag={blockObj.tag}, trigger={bc.isTrigger}, size={bc.size}");
            }
        }
    }
}
using UnityEngine;

namespace SuperBreakout.Blocks
{
    public class BlockSpawner : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private int rows = 5;
        [SerializeField] private int columns = 10;
        [SerializeField] private float spacing = 0.1f;
        [SerializeField] private Vector2 startPosition = new Vector2(-4.5f, 3f);
        
        [Header("Block Settings")]
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private Vector2 blockSize = new Vector2(0.9f, 0.4f);
        
        [Header("Difficulty")]
        [SerializeField] private int[] rowHealthValues = new int[] { 3, 3, 2, 2, 1 };
        [SerializeField] private int[] rowScoreValues = new int[] { 30, 30, 20, 20, 10 };
        [SerializeField] private Color[] rowColors = new Color[] { Color.red, Color.red, Color.yellow, Color.yellow, Color.green };
        
        private void Start()
        {
            if (blockPrefab == null)
            {
                CreateDefaultBlockPrefab();
            }
            
            SpawnBlocks();
        }
        
        private void CreateDefaultBlockPrefab()
        {
            blockPrefab = new GameObject("BlockPrefab");
            blockPrefab.AddComponent<SpriteRenderer>();
            BoxCollider collider = blockPrefab.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            blockPrefab.AddComponent<Block>();
            blockPrefab.tag = "Block";
            blockPrefab.SetActive(false);
        }
        
        private void SpawnBlocks()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    SpawnBlock(row, col);
                }
            }
        }
        
        private void SpawnBlock(int row, int col)
        {
            Vector2 position = CalculateBlockPosition(row, col);
            GameObject blockObj = Instantiate(blockPrefab, position, Quaternion.identity, transform);
            blockObj.SetActive(true);
            blockObj.name = $"Block_{row}_{col}";
            
            SetupBlockComponent(blockObj, row);
            SetupBlockVisuals(blockObj, row);
        }
        
        private Vector2 CalculateBlockPosition(int row, int col)
        {
            float x = startPosition.x + col * (blockSize.x + spacing);
            float y = startPosition.y - row * (blockSize.y + spacing);
            return new Vector2(x, y);
        }
        
        private void SetupBlockComponent(GameObject blockObj, int row)
        {
            Block block = blockObj.GetComponent<Block>();
            if (block != null)
            {
                int health = row < rowHealthValues.Length ? rowHealthValues[row] : 1;
                int score = row < rowScoreValues.Length ? rowScoreValues[row] : 10;
                
                block.SetHealth(health);
                block.SetScoreValue(score);
            }
        }
        
        private void SetupBlockVisuals(GameObject blockObj, int row)
        {
            SpriteRenderer sr = blockObj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = CreateBlockSprite();
                sr.color = row < rowColors.Length ? rowColors[row] : Color.white;
            }
            
            BoxCollider2D collider = blockObj.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = blockSize;
                collider.isTrigger = true;
            }
            
            blockObj.transform.localScale = new Vector3(blockSize.x, blockSize.y, 1f);
        }
        
        private Sprite CreateBlockSprite()
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        }
        
        private void OnValidate()
        {
            rows = Mathf.Max(1, rows);
            columns = Mathf.Max(1, columns);
            spacing = Mathf.Max(0f, spacing);
        }
    }
}
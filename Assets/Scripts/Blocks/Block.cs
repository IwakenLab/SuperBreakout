using UnityEngine;

namespace SuperBreakout.Blocks
{
    public class Block : MonoBehaviour
    {
        [Header("Block Settings")]
        [SerializeField] private int health = 1;
        [SerializeField] private int scoreValue = 10;
        [SerializeField] private Color[] healthColors = new Color[] { Color.green, Color.yellow, Color.red };
        
        [Header("Effects")]
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private GameObject destroyEffectPrefab;
        
        private SpriteRenderer spriteRenderer;
        private int currentHealth;
        
        public delegate void BlockDestroyedEvent(int score);
        public static event BlockDestroyedEvent OnBlockDestroyed;
        
        public delegate void BlockHitEvent();
        public static event BlockHitEvent OnBlockHit;
        
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }
        }
        
        private void Start()
        {
            currentHealth = health;
            UpdateBlockAppearance();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Block hit by: {other.name} with tag: {other.tag}");
            if (other.CompareTag("Ball"))
            {
                Debug.Log("Ball tag matched! Taking damage.");
                TakeDamage();
            }
        }
        
        public void TakeDamage()
        {
            currentHealth--;
            Debug.Log($"Block took damage! Health: {currentHealth}/{health}");
            OnBlockHit?.Invoke();
            
            if (currentHealth <= 0)
            {
                Debug.Log("Block health depleted, destroying...");
                DestroyBlock();
            }
            else
            {
                UpdateBlockAppearance();
                PlayHitEffect();
            }
        }
        
        private void UpdateBlockAppearance()
        {
            if (healthColors.Length > 0 && spriteRenderer != null)
            {
                int colorIndex = Mathf.Clamp(currentHealth - 1, 0, healthColors.Length - 1);
                spriteRenderer.color = healthColors[colorIndex];
            }
        }
        
        private void PlayHitEffect()
        {
            if (hitEffectPrefab != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 1f);
            }
        }
        
        private void DestroyBlock()
        {
            OnBlockDestroyed?.Invoke(scoreValue * health);
            
            if (destroyEffectPrefab != null)
            {
                GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 2f);
            }
            
            Destroy(gameObject);
        }
        
        public void SetHealth(int newHealth)
        {
            health = Mathf.Max(1, newHealth);
            currentHealth = health;
            UpdateBlockAppearance();
        }
        
        public void SetScoreValue(int value)
        {
            scoreValue = Mathf.Max(1, value);
        }
        
        public int GetHealth()
        {
            return currentHealth;
        }
        
        public int GetMaxHealth()
        {
            return health;
        }
    }
}
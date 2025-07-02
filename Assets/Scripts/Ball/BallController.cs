using UnityEngine;

namespace SuperBreakout.Ball
{
    public class BallController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float initialSpeed = 5f;
        [SerializeField] private float maxSpeed = 15f;
        [SerializeField] private float minSpeed = 3f;
        [SerializeField] private float speedIncreasePerHit = 0.1f;
        
        [Header("Paddle Bounce")]
        [SerializeField] private float maxBounceAngle = 75f;
        
        private Rigidbody2D rb;
        private Vector2 velocity;
        private float currentSpeed;
        private bool isMoving = false;
        
        public delegate void BallLostEvent();
        public event BallLostEvent OnBallLost;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }
            
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        
        private void Start()
        {
            currentSpeed = initialSpeed;
        }
        
        private void FixedUpdate()
        {
            if (isMoving)
            {
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            }
        }
        
        public void Launch()
        {
            if (!isMoving)
            {
                float randomAngle = Random.Range(-45f, 45f);
                Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
                velocity = direction * currentSpeed;
                isMoving = true;
            }
        }
        
        public void ResetBall(Vector3 position)
        {
            transform.position = position;
            velocity = Vector2.zero;
            isMoving = false;
            currentSpeed = initialSpeed;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Wall"))
            {
                HandleWallCollision(other);
            }
            else if (other.CompareTag("Paddle"))
            {
                HandlePaddleCollision(other);
            }
            else if (other.CompareTag("Block"))
            {
                HandleBlockCollision(other);
            }
            else if (other.CompareTag("DeathZone"))
            {
                HandleDeathZone();
            }
        }
        
        private void HandleWallCollision(Collider2D wall)
        {
            Vector2 normal = GetCollisionNormal(wall);
            velocity = Vector2.Reflect(velocity, normal);
        }
        
        private void HandlePaddleCollision(Collider2D paddle)
        {
            float paddleWidth = paddle.bounds.size.x;
            float hitPosition = (transform.position.x - paddle.transform.position.x) / (paddleWidth / 2f);
            hitPosition = Mathf.Clamp(hitPosition, -1f, 1f);
            
            float bounceAngle = hitPosition * maxBounceAngle;
            Vector2 direction = Quaternion.Euler(0, 0, -bounceAngle) * Vector2.up;
            
            currentSpeed = Mathf.Min(currentSpeed + speedIncreasePerHit, maxSpeed);
            velocity = direction * currentSpeed;
        }
        
        private void HandleBlockCollision(Collider2D block)
        {
            Vector2 normal = GetCollisionNormal(block);
            velocity = Vector2.Reflect(velocity, normal);
        }
        
        private void HandleDeathZone()
        {
            isMoving = false;
            OnBallLost?.Invoke();
        }
        
        private Vector2 GetCollisionNormal(Collider2D collider)
        {
            Vector2 closestPoint = collider.ClosestPoint(transform.position);
            Vector2 normal = ((Vector2)transform.position - closestPoint).normalized;
            
            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                normal = new Vector2(Mathf.Sign(normal.x), 0);
            }
            else
            {
                normal = new Vector2(0, Mathf.Sign(normal.y));
            }
            
            return normal;
        }
        
        private void OnValidate()
        {
            if (minSpeed > maxSpeed)
            {
                minSpeed = maxSpeed;
            }
            
            if (initialSpeed < minSpeed)
            {
                initialSpeed = minSpeed;
            }
            else if (initialSpeed > maxSpeed)
            {
                initialSpeed = maxSpeed;
            }
        }
    }
}
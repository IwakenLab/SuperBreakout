using UnityEngine;

namespace SuperBreakout.Ball
{
    public class BallController3D : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float initialSpeed = 5f;
        [SerializeField] private float maxSpeed = 15f;
        [SerializeField] private float minSpeed = 3f;
        [SerializeField] private float speedIncreasePerHit = 0.1f;
        
        [Header("Paddle Bounce")]
        [SerializeField] private float maxBounceAngle = 75f;
        
        private Rigidbody rb;
        private Vector3 velocity;
        private float currentSpeed;
        private bool isMoving = false;
        
        public delegate void BallLostEvent();
        public event BallLostEvent OnBallLost;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("BallController3D: Rigidbody not found, adding one");
                rb = gameObject.AddComponent<Rigidbody>();
            }
            
            // 3D設定
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            Debug.Log("BallController3D: Initialized with 3D Rigidbody");
        }
        
        private void Start()
        {
            currentSpeed = initialSpeed;
        }
        
        private void FixedUpdate()
        {
            if (isMoving)
            {
                rb.linearVelocity = velocity;
            }
        }
        
        public void Launch()
        {
            Debug.Log("Launch called! isMoving: " + isMoving);
            if (!isMoving)
            {
                float randomAngle = Random.Range(-45f, 45f);
                Vector3 direction = Quaternion.Euler(0, 0, randomAngle) * Vector3.up;
                velocity = direction * currentSpeed;
                isMoving = true;
                Debug.Log($"Ball launched! Velocity: {velocity}, Speed: {currentSpeed}");
            }
        }
        
        public void ResetBall(Vector3 position)
        {
            transform.position = position;
            velocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            isMoving = false;
            currentSpeed = initialSpeed;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Ball hit: {other.name} with tag: {other.tag}");
            
            if (other.CompareTag("Wall") || other.CompareTag("Block"))
            {
                HandleWallCollision(other);
            }
            else if (other.CompareTag("Player"))
            {
                HandlePaddleCollision(other);
            }
            else if (other.CompareTag("Block"))
            {
                HandleBlockCollision(other);
            }
            else if (other.CompareTag("Finish"))
            {
                HandleDeathZone();
            }
        }
        
        private void HandleWallCollision(Collider wall)
        {
            Vector3 normal = GetCollisionNormal(wall);
            velocity = Vector3.Reflect(velocity.normalized, normal) * currentSpeed;
            Debug.Log($"Wall bounce: new velocity = {velocity}");
        }
        
        private void HandlePaddleCollision(Collider paddle)
        {
            float paddleWidth = paddle.bounds.size.x;
            float hitPosition = (transform.position.x - paddle.transform.position.x) / (paddleWidth / 2f);
            hitPosition = Mathf.Clamp(hitPosition, -1f, 1f);
            
            float bounceAngle = hitPosition * maxBounceAngle;
            Vector3 direction = Quaternion.Euler(0, 0, -bounceAngle) * Vector3.up;
            
            currentSpeed = Mathf.Min(currentSpeed + speedIncreasePerHit, maxSpeed);
            velocity = direction * currentSpeed;
            Debug.Log($"Paddle bounce: angle = {bounceAngle}, new velocity = {velocity}");
        }
        
        private void HandleBlockCollision(Collider block)
        {
            Vector3 normal = GetCollisionNormal(block);
            velocity = Vector3.Reflect(velocity.normalized, normal) * currentSpeed;
        }
        
        private void HandleDeathZone()
        {
            isMoving = false;
            rb.linearVelocity = Vector3.zero;
            OnBallLost?.Invoke();
            Debug.Log("Ball lost!");
        }
        
        private Vector3 GetCollisionNormal(Collider collider)
        {
            Vector3 closestPoint = collider.ClosestPoint(transform.position);
            Vector3 normal = (transform.position - closestPoint).normalized;
            
            // XY平面に制限
            normal.z = 0;
            
            // 軸に整列
            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                normal = new Vector3(Mathf.Sign(normal.x), 0, 0);
            }
            else
            {
                normal = new Vector3(0, Mathf.Sign(normal.y), 0);
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
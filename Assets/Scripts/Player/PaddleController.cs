using UnityEngine;

namespace SuperBreakout.Player
{
    public class PaddleController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float smoothTime = 0.1f;
        [SerializeField] private bool useMouseControl = false;
        
        [Header("Boundaries")]
        [SerializeField] private float leftBoundary = -8f;
        [SerializeField] private float rightBoundary = 8f;
        
        private Vector3 velocity = Vector3.zero;
        private float targetXPosition;
        private Camera mainCamera;
        
        private void Start()
        {
            mainCamera = Camera.main;
            targetXPosition = transform.position.x;
        }
        
        private void Update()
        {
            HandleInput();
            MoveToTargetPosition();
        }
        
        private void HandleInput()
        {
            if (useMouseControl)
            {
                HandleMouseInput();
            }
            else
            {
                HandleKeyboardInput();
            }
        }
        
        private void HandleKeyboardInput()
        {
            float horizontalInput = 0f;
            
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalInput = -1f;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                horizontalInput = 1f;
            }
            
            targetXPosition += horizontalInput * moveSpeed * Time.deltaTime;
            targetXPosition = Mathf.Clamp(targetXPosition, leftBoundary, rightBoundary);
        }
        
        private void HandleMouseInput()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, 0, 10f));
            targetXPosition = Mathf.Clamp(worldPosition.x, leftBoundary, rightBoundary);
        }
        
        private void MoveToTargetPosition()
        {
            Vector3 targetPosition = new Vector3(targetXPosition, transform.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        
        public void SetMouseControl(bool enable)
        {
            useMouseControl = enable;
        }
        
        public void ResetPosition()
        {
            transform.position = new Vector3(0f, transform.position.y, transform.position.z);
            targetXPosition = 0f;
        }
    }
}
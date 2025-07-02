using UnityEngine;

namespace SuperBreakout.Core
{
    public class BallColliderFix : MonoBehaviour
    {
        void Start()
        {
            GameObject ball = GameObject.Find("Ball");
            if (ball != null)
            {
                SphereCollider sc = ball.GetComponent<SphereCollider>();
                if (sc != null)
                {
                    sc.isTrigger = false;
                    sc.radius = 0.5f; // Make sure radius is correct
                    Debug.Log("Ball collider fixed: isTrigger = false, radius = 0.5");
                }
                
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = false;
                    rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    Debug.Log("Ball Rigidbody settings updated");
                }
            }
        }
    }
}
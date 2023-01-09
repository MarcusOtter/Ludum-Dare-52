using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2;
    [SerializeField] private bool flipY;

    [Header("Tilt")]
    [SerializeField] private int tiltX = 30;
    [SerializeField] private int tiltY = 10;
    [SerializeField] private int tiltZ = 50;
    [SerializeField] private float tiltSpeed = 5f;
    
    private Rigidbody _rigidbody;
    private Transform _transform;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    private void Update()
    {
        // Get input
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical") * (flipY ? -1 : 1);

        // Move
        var movement = Vector3.ClampMagnitude(new Vector3(horizontalInput, verticalInput, 0), 1);
        _rigidbody.velocity = movement * speed;
        
        // Tilt
        var targetRotationX = movement.y > 0 ? -tiltX : movement.y < 0 ? tiltX : 0;
        var targetRotationY = movement.x > 0 ? -tiltY : movement.x < 0 ? tiltY : 0;
        var targetRotationZ = movement.x > 0 ? -tiltZ : movement.x < 0 ? tiltZ : 0;

        var targetRotation = Quaternion.Euler(targetRotationX, targetRotationY, targetRotationZ);
        _transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        var asteroid = collision.gameObject.GetComponentInParent<Asteroid>();
        if (!asteroid) return;
        
        gameObject.SetActive(false);
    }
}

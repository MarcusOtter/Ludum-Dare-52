using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 100;
    
    private Rigidbody _rigidbody;
    private Transform _transform;

    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _movement;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        
        // TODO: Option to flip vertical input

        _movement = Vector3.ClampMagnitude(new Vector3(_horizontalInput, _verticalInput, 0), 1);
        _rigidbody.velocity = _movement * _speed;
    }
}

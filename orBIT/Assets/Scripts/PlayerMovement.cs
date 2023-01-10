using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2;

    [Header("Tilt")]
    [SerializeField] private int tiltX = 30;
    [SerializeField] private int tiltY = 10;
    [SerializeField] private int tiltZ = 50;
    [SerializeField] private float tiltSpeed = 5f;
    
    [Header("Fuel")]
    [SerializeField] private Image fuelImage;
    [SerializeField] private Text fuelText;
    [SerializeField] private float maxFuel;
    
    private Rigidbody _rigidbody;
    private Transform _transform;

    private bool _flipY;
    private float _fuel;
    
    public void AddFuel(float fuel)
    {
        if (_fuel + fuel >= maxFuel)
        {
            _fuel = maxFuel;
            return;
        }
        
        _fuel += fuel;
    }

    public void FlipY(bool flip)
    {
        _flipY = flip;
    }

    private void OnEnable()
    {
        Difficulty.OnPreGameStart += OnBeforeGameBegin;
        Difficulty.OnGameEnd += OnGameOver;
    }

    private void OnGameOver()
    {
        _rigidbody.velocity = Vector3.zero;
        _transform.position = new Vector3(0, 1, 1.5f);
        _transform.rotation = Quaternion.identity;
    }

    private void OnBeforeGameBegin()
    {
        _rigidbody.velocity = Vector3.zero;
        _transform.position = new Vector3(0, 1, 1.5f);
        _transform.rotation = Quaternion.identity;
        
        _fuel = Difficulty.StartFuel;
        fuelImage.fillAmount = _fuel / maxFuel;
        fuelText.text = "fuel " + (_fuel / maxFuel * 100).ToString("##0") + "%";
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
        _fuel = fuelImage.fillAmount * maxFuel;
    }

    private void Update()
    {
        gameObject.SetActive(Difficulty.IsRunning);
        if (!Difficulty.IsRunning) return;
        
        // Get input
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical") * (_flipY ? -1 : 1);

        // Move
        var movement = Vector3.ClampMagnitude(new Vector3(horizontalInput, verticalInput, 0), 1);
        _rigidbody.velocity = movement * speed;
        
        // Tilt
        var targetRotationX = movement.y > 0 ? -tiltX : movement.y < 0 ? tiltX : 0;
        var targetRotationY = movement.x > 0 ? -tiltY : movement.x < 0 ? tiltY : 0;
        var targetRotationZ = movement.x > 0 ? -tiltZ : movement.x < 0 ? tiltZ : 0;

        var targetRotation = Quaternion.Euler(targetRotationX, targetRotationY, targetRotationZ);
        _transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
        
        // Calculate fuel
        _fuel -= Time.deltaTime * Difficulty.Instance.EarthSpeed;
        fuelImage.fillAmount = _fuel / maxFuel;
        fuelText.text = "fuel " + (_fuel / maxFuel * 100).ToString("##0") + "%";
        
        // TODO: If fuel is empty, game over
        if (_fuel <= 0)
        {
            Difficulty.Instance.EndGame();
            gameObject.SetActive(false);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        var asteroid = collision.gameObject.GetComponentInParent<Asteroid>();
        if (!asteroid) return;

        Difficulty.Instance.EndGame();
        gameObject.SetActive(false);
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Loot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private new MeshRenderer renderer;
    [SerializeField] private AudioSource audioSource;
    
    [Header("Settings")]
    [SerializeField] private Vector2 emergeSpeedMinMax = new(25, 125);
    [SerializeField] private Color fuelColor;
    [SerializeField] private Color ammoColor;
    [SerializeField] private AudioClip pickupSound;

    private Rigidbody _rigidbody;
    private float _fuelLoot;
    private int _ammoLoot;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public void Init(float fuelLoot, int ammoLoot)
    {
        _fuelLoot = fuelLoot;
        _ammoLoot = ammoLoot;
        
        renderer.material.color = _fuelLoot > 0 ? fuelColor : _ammoLoot > 0 ? ammoColor : Color.clear;

        var direction = VectorHelpers.GetRandomDirection();
        var speed = Random.Range(emergeSpeedMinMax.x, emergeSpeedMinMax.y);
        _rigidbody.AddForce(direction * speed, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (_fuelLoot > 0)
        {
            var playerMovement = other.GetComponentInParent<PlayerMovement>();
            playerMovement.AddFuel(_fuelLoot);
        }

        if (_ammoLoot > 0)
        {
            var playerShooter = other.GetComponentInParent<LaserShooter>();
            playerShooter.AddAmmo(_ammoLoot);
        }

        if (audioSource && pickupSound)
        {
            audioSource.PlayOneShot(pickupSound);
        }
        
        gameObject.SetActive(false);
        Destroy(gameObject, 2);
    }
}

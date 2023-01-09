using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private Bullet bulletPrefab;
    
    [Header("Settings")]
    [SerializeField] private float fireDelay = 0.25f;
    [SerializeField] private float speed = 10f;

    private Transform _transform;
    private float _lastFireTime;
    
    
    
    private void Update()
    {
        if (!Input.GetButton("Fire1")) return;
        if (_lastFireTime + fireDelay > Time.time) return;
        
        var bulletLeft = Instantiate(bulletPrefab, left.position, left.rotation);
        var bulletRight = Instantiate(bulletPrefab, right.position, right.rotation);
        
        bulletLeft.Shoot(speed);
        bulletRight.Shoot(speed);
        
        // TODO: Play sound
        
        _lastFireTime = Time.time;
    }
}

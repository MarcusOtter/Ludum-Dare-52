using UnityEngine;
using UnityEngine.UI;

public class LaserShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Image ammoImage;
    [SerializeField] private Text ammoText;
    
    [Header("Settings")]
    [SerializeField] private float fireDelay = 0.25f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private int tiltDegrees = 15;
    [SerializeField] private int maxAmmo = 100;

    private float _lastFireTime;
    private int _ammo;

    public void AddAmmo(int ammo)
    {
        if (_ammo + ammo > maxAmmo)
        {
            _ammo = maxAmmo;
            UpdateUI();
            return;
        }
        
        _ammo += ammo;
        UpdateUI();
    }
    
    private void OnEnable()
    {
        Difficulty.OnPreGameStart += OnBeforeGameBegin;
        Difficulty.OnGameEnd += OnGameOver;
    }

    private void OnGameOver()
    {
        
    }

    private void OnBeforeGameBegin()
    {
        _ammo = Difficulty.StartAmmo;
        UpdateUI();
    }
    
    private void Awake()
    {
        _ammo = (int) (ammoImage.fillAmount * maxAmmo);
    }

    private void Update()
    {
        if (!Difficulty.IsRunning) return;
        
        if (_ammo <= 0) return;
        if (!Input.GetButton("Fire1")) return;
        if (_lastFireTime + fireDelay > Time.time) return;
        
        var bulletLeft = Instantiate(bulletPrefab, left.position, left.rotation);
        var bulletRight = Instantiate(bulletPrefab, right.position, right.rotation);
        
        bulletLeft.Shoot(speed, tiltDegrees);
        bulletRight.Shoot(speed, tiltDegrees);

        audioSource.PlayOneShot(shootSound, 0.025f);
        
        _ammo--;
        UpdateUI();
        
        _lastFireTime = Time.time;
    }

    private void UpdateUI()
    {
        ammoImage.fillAmount = _ammo / (float) maxAmmo;
        ammoText.text = "ammo " + (_ammo / (float) maxAmmo * 100).ToString("##0") + "%";
    }
}

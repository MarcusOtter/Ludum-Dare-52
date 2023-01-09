using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxCollider2D spawnArea;

    [Header("Settings")]
    [SerializeField] private float spawnRate = 1f;
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

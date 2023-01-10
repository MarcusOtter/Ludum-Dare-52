using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private Transform earth;
    [SerializeField] private Asteroid asteroidPrefab;

    private float _lastSpawnTime;

    private void Update()
    {
        if (!Difficulty.IsRunning) return;
        if (_lastSpawnTime + Difficulty.Instance.SpawnDelay > Time.time) return;

        var spawnPosition = GetSpawnPosition(spawnArea.bounds);
        var asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity, earth);
        asteroid.Init();
        
        _lastSpawnTime = Time.time;
    }

    private static Vector3 GetSpawnPosition(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}

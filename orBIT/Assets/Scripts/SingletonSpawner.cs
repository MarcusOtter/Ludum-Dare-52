using UnityEngine;

public class SingletonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSpawn;

    private void Awake()
    {
        foreach (var prefab in prefabsToSpawn)
        {
            Instantiate(prefab);
        }
    }
}

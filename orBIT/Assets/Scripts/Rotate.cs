using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float randomizeMaxSpeed;
    [SerializeField] private Vector3 speed;

    private void Awake()
    {
        if (randomizeMaxSpeed == 0) return;
        
        speed.x = Random.Range(-randomizeMaxSpeed, randomizeMaxSpeed);
        speed.y = Random.Range(-randomizeMaxSpeed, randomizeMaxSpeed);
        speed.z = Random.Range(-randomizeMaxSpeed, randomizeMaxSpeed);
    }
    
    private void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
    }
}

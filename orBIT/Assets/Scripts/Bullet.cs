using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private new Rigidbody rigidbody;
	
	public void Shoot(float speed)
	{
		rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
	}
	
	private void OnCollisionEnter(Collision collision)
	{
		gameObject.SetActive(false);
		
		var asteroid = collision.gameObject.GetComponentInParent<Asteroid>();
		if (!asteroid) return;
        
		asteroid.TakeDamage();
	}
}


using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private new Rigidbody rigidbody;
	
	public void Shoot(float speed, int tiltDegrees)
	{
		if (Input.GetAxisRaw("Horizontal") > 0)
		{
			transform.Rotate(Vector3.up, tiltDegrees, Space.World);
		}
		else if (Input.GetAxisRaw("Horizontal") < 0)
		{
			transform.Rotate(Vector3.up, -tiltDegrees, Space.World);
		}

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


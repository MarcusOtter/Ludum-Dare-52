using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
	[SerializeField] private float delay;

	private void Start()
	{
		Destroy(gameObject, delay);
	}
}


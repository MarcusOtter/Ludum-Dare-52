using System;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
	[SerializeField] private int largeHealth = 3;
	[SerializeField] private int mediumHealth = 2;
	[SerializeField] private int smallHealth = 1;
	[SerializeField] private Transform modelsParent;

	// TODO: Thing to spawn when dies?
	
	private int health;
	
	public void Init()
	{
		var random = new System.Random();

		var sizes = Enum.GetValues(typeof(AsteroidSize));
		var asteroidSize = (AsteroidSize) sizes.GetValue(random.Next(sizes.Length));

		health = asteroidSize switch
		{
			AsteroidSize.Large1  or AsteroidSize.Large2  => largeHealth,
			AsteroidSize.Medium1 or AsteroidSize.Medium2 => mediumHealth,
			AsteroidSize.Small1  or AsteroidSize.Small2  => smallHealth,
			_ => health
		};

		var types = Enum.GetValues(typeof(AsteroidType));
		var asteroidType = (AsteroidType) types.GetValue(random.Next(types.Length));
		
		var asteroidParent = modelsParent.GetChild((int) asteroidSize);
		var asteroid = asteroidParent.GetChild((int) asteroidType);
		
		asteroid.gameObject.SetActive(true);
	}

	public void TakeDamage()
	{
		health -= 1;

		// TODO: Play hurt animation and impact sound
		
		if (health <= 0)
		{
			gameObject.SetActive(false);
		}
	}
}


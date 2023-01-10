using System;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform modelsParent;
	[SerializeField] private Loot lootPrefab;
	
	[Header("Settings")]
	[SerializeField] private int largeHealth = 3;
	[SerializeField] private int mediumHealth = 2;
	[SerializeField] private int smallHealth = 1;
	[SerializeField] private int smallLootMultiplier = 1;
	[SerializeField] private int mediumLootMultiplier = 2;
	[SerializeField] private int largeLootMultiplier = 3;
	[SerializeField] private int fullAsteroidMultiplier = 2;

	private int _maxHealth;
	private int _health;
	private int _ammoLoot;
	private float _fuelLoot;
	
	public void Init()
	{
		var random = new System.Random();

		var sizes = Enum.GetValues(typeof(AsteroidSize));
		var asteroidSize = (AsteroidSize) sizes.GetValue(random.Next(sizes.Length));

		var types = Enum.GetValues(typeof(AsteroidType));
		var asteroidType = (AsteroidType) types.GetValue(random.Next(types.Length));
		
		var isLarge = asteroidSize is AsteroidSize.Large1 or AsteroidSize.Large2;
		var isMedium = asteroidSize is AsteroidSize.Medium1 or AsteroidSize.Medium2;
		var isSmall = asteroidSize is AsteroidSize.Small1 or AsteroidSize.Small2;
		
		var isAmmo = asteroidType is AsteroidType.Iron1 or AsteroidType.Iron2;
		var isFuel = asteroidType is AsteroidType.Gold1 or AsteroidType.Gold2;
		var isFull = asteroidType is AsteroidType.Iron2 or AsteroidType.Gold2;

		var baseAmmoLoot = Mathf.CeilToInt(Difficulty.Instance.AmmoLoot);
		var baseFuelLoot = Difficulty.Instance.FuelLoot;
		
		if (isLarge)
		{
			_health = largeHealth;
			_ammoLoot = isAmmo ? baseAmmoLoot * largeLootMultiplier : 0;
			_fuelLoot = isFuel ? baseFuelLoot * largeLootMultiplier : 0;
		}
		else if (isMedium)
		{
			_health = mediumHealth;
			_ammoLoot = isAmmo ? baseAmmoLoot * mediumLootMultiplier : 0;
			_fuelLoot = isFuel ? baseFuelLoot * mediumLootMultiplier : 0;
		}
		else if (isSmall)
		{
			_health = smallHealth;
			_ammoLoot = isAmmo ? baseAmmoLoot * smallLootMultiplier : 0;
			_fuelLoot = isFuel ? baseFuelLoot * smallLootMultiplier : 0;
		}

		if (isFull)
		{
			_ammoLoot *= fullAsteroidMultiplier;
			_fuelLoot *= fullAsteroidMultiplier;
		}

		var asteroidParent = modelsParent.GetChild((int) asteroidSize);
		var asteroid = asteroidParent.GetChild((int) asteroidType);

		_maxHealth = _health;
		
		asteroid.gameObject.SetActive(true);
	}

	public void TakeDamage()
	{
		if (!Difficulty.IsRunning) return;
		
		_health--;

		if (_health > 0)
		{
			// print("Ouchie " + _health);
			// TODO: Play hurt animation and impact sound

			return;
		}

		Difficulty.Instance.AddScore(_maxHealth * 5_000);
		
		gameObject.SetActive(false);
		Destroy(gameObject, 10);

		if (_ammoLoot <= 0 && _fuelLoot <= 0) return;

		var orbsToSpawn = (int) (_ammoLoot > 0 ? _ammoLoot / Difficulty.Instance.AmmoLoot * 3 :
			_fuelLoot > 0 ? _fuelLoot / Difficulty.Instance.FuelLoot * 3 : 0);
		
		var ammoLootPerOrb = _ammoLoot > 0 ? Mathf.CeilToInt(_ammoLoot / (float)orbsToSpawn) : 0;
		var fuelLootPerOrb = _fuelLoot > 0 ? _fuelLoot / orbsToSpawn : 0;
		
		for (var i = 0; i < orbsToSpawn; i++)
		{
			var loot = Instantiate(lootPrefab, transform.position, Quaternion.identity, transform.parent);
			loot.Init(fuelLootPerOrb, ammoLootPerOrb);
		}
	}
}


using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[Header("Toggles")]
	[SerializeField] private Toggle musicToggle;
	[SerializeField] private Toggle invertYToggle;

	[Header("References")]
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private string toGame = "ToGame";
	[SerializeField] private string toMenu = "ToMenu";
	[SerializeField] internal Animator[] animators;
	[SerializeField] private UnityEvent onPlay;
	[SerializeField] private UnityEvent onGameOver;
	
	private void OnEnable()
	{
		Difficulty.OnGameEnd += OnGameOver;
		Difficulty.OnGameStart += OnGameStart;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			musicToggle.isOn = !musicToggle.isOn;
			musicSource.mute = !musicToggle.isOn;
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			invertYToggle.isOn = !invertYToggle.isOn;
			FindObjectOfType<PlayerMovement>().FlipY(invertYToggle.isOn);
		}

		if (!Difficulty.IsRunning && Input.GetKeyDown(KeyCode.Space))
		{
			Difficulty.Instance.StartGame();
			foreach (var animator in animators)
			{
				animator.SetTrigger(toGame);
			}
			
			var asteroids = FindObjectsOfType<Asteroid>();
			foreach (var asteroid in asteroids)
			{
				Destroy(asteroid.gameObject);
			}

			var loots = FindObjectsOfType<Loot>();
			foreach (var loot in loots)
			{
				Destroy(loot.gameObject);
			}
		}
	}
	
	private void OnGameOver()
	{
		foreach (var animator in animators)
		{
			animator.SetTrigger(toMenu);
		}
		
		onGameOver?.Invoke();
	}

	private void OnGameStart()
	{
		onPlay?.Invoke();
	}
	
	private void OnDisable()
	{
		Difficulty.OnGameEnd -= OnGameOver;
		Difficulty.OnGameStart -= OnGameStart;
	}

}

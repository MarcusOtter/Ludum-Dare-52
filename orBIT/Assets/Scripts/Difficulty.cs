using System;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : SingletonMonoBehaviour<Difficulty>
{
    public static event Action OnPreGameStart;
    public static event Action OnGameStart;
    public static event Action OnGameEnd;

    internal static bool IsRunning { get; private set; }

    internal static float StartFuel = 600f; // hack
    internal static int StartAmmo = 75; // hack
    
    internal float EarthSpeed => earthRotationSpeed.Evaluate(_currentCurveTime);
    internal float SpawnDelay => timeBetweenSpawns.Evaluate(_currentCurveTime);
    internal float AmmoLoot => baseAmmoLoot.Evaluate(_currentCurveTime);
    internal float FuelLoot => baseFuelLoot.Evaluate(_currentCurveTime);
    
    [SerializeField] private float timeUntilMaxDifficulty = 10f;
    [SerializeField] private AnimationCurve earthRotationSpeed;
    [SerializeField] private AnimationCurve timeBetweenSpawns;
    [SerializeField] private AnimationCurve baseAmmoLoot;
    [SerializeField] private AnimationCurve baseFuelLoot;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;

    private float _currentCurveTime => Mathf.Clamp01((Time.time - _startTime) / timeUntilMaxDifficulty);
    private static int _score;
    private static int _bestScore;
    
    private static float _startTime;
    
    public void StartGame()
    {
        if (IsRunning) return;

        _score = 0;
        OnPreGameStart?.Invoke();
        
        this.FireAndForgetWithDelay(3.5f, () =>
        {
            _startTime = Time.time;
            IsRunning = true;
            OnGameStart?.Invoke();
        });
    }
    
    public void EndGame()
    {
        if (!IsRunning) return;

        if (_score > _bestScore)
        {
            _bestScore = _score;
        }
        
        _startTime = 0;
        IsRunning = false;
        OnGameEnd?.Invoke();
    }

    public void AddScore(int score)
    {
        _score += score;
    }
    
    private void Start()
    {
        _startTime = Time.time;
    }

    private void FixedUpdate()
    {
        scoreText.text = _score.ToString();
        bestScoreText.text = _bestScore.ToString();
        
        if (!IsRunning) return;
        _score += (int) EarthSpeed;
    }
}

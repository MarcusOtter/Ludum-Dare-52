using UnityEngine;

public class Difficulty : SingletonMonoBehaviour<Difficulty>
{
    internal float EarthSpeed => earthRotationSpeed.Evaluate(currentCurveTime);
    internal float SpawnDelay => timeBetweenSpawns.Evaluate(currentCurveTime);
    
    [SerializeField] private float timeUntilMaxDifficulty = 10f;
    [SerializeField] private AnimationCurve earthRotationSpeed;
    [SerializeField] private AnimationCurve timeBetweenSpawns;
    
    private float currentCurveTime => Mathf.Clamp01(Time.time / timeUntilMaxDifficulty);
    
}

using UnityEngine;

public class StateTimer
{
    public bool Reached { get; private set; }
    
    private float _targetTime;
    private float _elapsedTime;

    public void SetTime(float time)
    {
        _targetTime = time;
        _elapsedTime = 0f;
        Reached = false;
    }

    public void OnUpdate()
    {
        if (Reached) return;
        
        _elapsedTime += Time.deltaTime;
        
        if (_elapsedTime >= _targetTime)
        {
            Reached = true;
        }
    }

    public void ResetTime()
    {
        _elapsedTime = 0f;
        _targetTime = 0f;
        Reached = false;
    }

    public float GetProgress()
    {
        if (_targetTime <= 0f) return 0f;
        return Mathf.Clamp01(_elapsedTime / _targetTime);
    }

    public float GetRemainingTime()
    {
        return Mathf.Max(0f, _targetTime - _elapsedTime);
    }

    
    public float GetElapsedTime()
    {
        return _elapsedTime;
    }
}
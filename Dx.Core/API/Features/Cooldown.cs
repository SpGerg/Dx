using Exiled.API.Features;
using UnityEngine;

namespace Dx.Core.API.Features;

public class Cooldown
{
    public Cooldown(float cooldownTime)
    {
        CooldownTime = cooldownTime;
        
        Log.Error(PassedTime);
    }
    
    public float CooldownTime { get; }
    
    public float PassedTime => Time.time - _lastUsedTime;
    
    public bool IsReady => !_isUsed || RemainingTime <= Mathf.Epsilon;
    
    public float RemainingTime => Mathf.Max(0f, CooldownTime - PassedTime);
    
    private float _lastUsedTime;

    private bool _isUsed;
    
    public void ForceUse()
    {
        _lastUsedTime = Time.time;
        _isUsed = true;
    }
}
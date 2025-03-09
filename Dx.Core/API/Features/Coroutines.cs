using System;
using System.Collections.Generic;
using System.Linq;
using MEC;

namespace Dx.Core.API.Features;

public class Coroutines
{
    private readonly Dictionary<string, CoroutineHandle> _coroutines = new();

    public void Start(string id, IEnumerator<float> coroutine)
    {
        if (_coroutines.TryGetValue(id, out var value))
        {
            Stop(id);
        }
        
        _coroutines.Add(id, Timing.RunCoroutine(coroutine));
    }

    public void CallDelayed(float delay, Action action)
    {
        _coroutines.Add(Guid.NewGuid().ToString(), Timing.CallDelayed(delay, action));
    }

    public void Stop(string id)
    {
        if (!_coroutines.TryGetValue(id, out var coroutine))
        {
            return;
        }

        Timing.KillCoroutines(coroutine);
        _coroutines.Remove(id);
    }
    
    public void Clear()
    {
        Timing.KillCoroutines(_coroutines.Values.ToArray());
        _coroutines.Clear();
    }
}
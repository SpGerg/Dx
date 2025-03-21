using System.Collections.Generic;
using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi;
using SCPSLAudioApi.AudioCore;
using UnityEngine;

namespace Dx.Core.API.Features.Audio;

public abstract class AudioBot
{
    static AudioBot()
    {
        Startup.SetupDependencies();
    }

    public static void DisconnectAllDummies()
    {
        foreach (var dummy in _audioBots)
        {
            dummy.Value.Disconnect();
        }
        
        _audioBots.Clear();
    }

    public static Dictionary<int, AudioBot> AudioBots => _audioBots;

    internal static readonly Dictionary<int, AudioBot> _audioBots = new();

    protected AudioBot(AudioSettings audioSettings)
    {
        AudioSettings = audioSettings;
    }
    
    public AudioPlayerBase Base { get; protected set; }
    
    public Npc Npc { get; protected set; }

    public AudioSettings AudioSettings { get; set; }

    public abstract void Play(Vector3 position, Quaternion? quaternion = null, Vector3? scale = null, RoleTypeId roleTypeId = RoleTypeId.Tutorial);
    
    public abstract bool Stop();

    public void Disconnect()
    {
        if (Npc is null)
        {
            return;
        }

        Npc.Disconnect();
        Base.Stoptrack(true);
        
        NetworkServer.Destroy(Npc.GameObject);

        _audioBots.Remove(Npc.Id);

        Npc = null;
        Base = null;
        
        Log.Info(Npc);
    }
}
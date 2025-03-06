using System.IO;
using Exiled.API.Features;
using PlayerRoles;
using SCPSLAudioApi.AudioCore;
using UnityEngine;

namespace Dx.Core.API.Features.Audio;

public class BasicAudioBot : AudioBot
{
    public BasicAudioBot(AudioSettings audioSettings) : base(audioSettings)
    {
    }
    
    public override void Play(Vector3 position, Quaternion? quaternion = null, Vector3? scale = null,
        RoleTypeId roleTypeId = RoleTypeId.Tutorial)
    {
        if (string.IsNullOrEmpty(AudioSettings.Filepath))
        {
            return;
        }
        
        Base.Loop = AudioSettings.IsLoop;
        Base.Volume = AudioSettings.Volume;
        
        Npc.Role.Set(roleTypeId);
        Npc.Position = position;
        
        if (quaternion is not null)
        {
            Npc.Rotation = quaternion.Value;
        }

        if (scale is not null)
        {
            Npc.Scale = scale.Value;
        }

        PlayerRoles.Voice.Intercom.TrySetOverride(Npc.ReferenceHub, AudioSettings.IsIntercom);
        
        Base.Enqueue(Path.Combine(Plugin.AudiosFilepath, AudioSettings.Filepath), 0);
        Base.Play(0);
    }

    public override bool Stop()
    {
        if (Npc.Role.Type is RoleTypeId.None)
        {
            return false;
        }
        
        Base.Stoptrack(true);
        return true;
    }
}
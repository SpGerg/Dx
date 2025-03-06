using System.IO;
using Dx.Core.API.Features.Webhooks;
using Exiled.API.Features;

namespace Dx.Core;

public class Plugin : Plugin<Config>
{
    public static Plugin Instance { get; private set; }

    public static string ServerName => Instance.Config.ServerName;
    
    public static Webhook Webhook { get; private set; }
    
    public static string AudiosFilepath { get; private set; }

    public override void OnEnabled()
    {
        Instance = this;
        Webhook = new Webhook();

        AudiosFilepath = Path.Combine(Paths.IndividualConfigs, "audios");

        Directory.CreateDirectory(AudiosFilepath);
        
        API.Features.Audio.Events.Internal.Server.Register();
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        API.Features.Audio.Events.Internal.Server.Unregister();
        
        base.OnDisabled();
    }
}
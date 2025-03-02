using Dx.Core.API.Features.Webhooks;
using Exiled.API.Features;

namespace Dx.Core;

public class Plugin : Plugin<Config>
{
    public static Plugin Instance { get; private set; }
    
    public static Webhook Webhook { get; private set; }

    public override void OnEnabled()
    {
        Instance = this;
        Webhook = new Webhook();
        
        base.OnEnabled();
    }
}
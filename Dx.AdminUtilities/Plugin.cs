using System;
using System.IO;
using Dx.AdminUtilities.Features.Admin;
using Dx.AdminUtilities.Features.Admin.Interfaces;
using Exiled.API.Features;
using Newtonsoft.Json;
using Player = Dx.AdminUtilities.Events.Internal.Player;
using Server = Dx.AdminUtilities.Features.Admin.Events.Internal.Server;

namespace Dx.AdminUtilities;

public class Plugin : Plugin<Config>
{
    public static Plugin Instance { get; private set; }

    public static Config Config { get; private set; }

    public override string Prefix => "dx-admin-utilities";

    public static IAdminRepository AdminRepository { get; private set; }

    private AdminNormChecker _adminNormChecker;

    public override void OnEnabled()
    {
        Instance = this;
        Config = base.Config;

        Directory.CreateDirectory(Path.Combine(Paths.IndividualConfigs, Prefix));

        AdminRepository = new AdminRepository(
            Path.Combine(Paths.IndividualConfigs, Prefix, $"{Core.Plugin.ServerName}_admins.json"));
        AdminRepository.Load();

        _adminNormChecker = new AdminNormChecker(
            Path.Combine(Paths.IndividualConfigs, Prefix, $"{Core.Plugin.ServerName}_admins_norm.json"));
        _adminNormChecker.CheckAndSendWebhook();

        Player.Register();
        
        Events.Internal.Server.Register();
        
        Features.Admin.Events.Internal.Player.Register();
        Features.Admin.Events.Internal.Server.Register();
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Player.Unregister();
        
        Events.Internal.Server.Unregister();
        
        Features.Admin.Events.Internal.Player.Unregister();
        Features.Admin.Events.Internal.Server.Unregister();

        base.OnDisabled();
    }

    public override void OnReloaded()
    {
        Server.SaveRepositoryOnRoundEnded(null);
        
        base.OnReloaded();
    }
}
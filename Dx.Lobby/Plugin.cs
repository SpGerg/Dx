using Dx.Lobby.API.Features.Serializables;
using Exiled.API.Features;
using HarmonyLib;
using MapEditorReborn.API.Features.Objects;

namespace Dx.Lobby
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }

        public static LobbySchematicSerializable SelectedSchematic { get; internal set; }
        
        public static SchematicObject SchematicObject { get; internal set; }
        
        private Harmony _harmony;

        public override void OnEnabled()
        {
            _harmony = new Harmony("dx-lobby");
            _harmony.PatchAll();
            
            Instance = this;

            Events.Internal.Server.Register();
            Events.Internal.Player.Register();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Events.Internal.Server.Unregister();
            Events.Internal.Player.Unregister();
            
            base.OnDisabled();
        }
    }
}
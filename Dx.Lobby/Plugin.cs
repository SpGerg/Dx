using System.Collections.Generic;
using Dx.Lobby.API.Features.Serializables;
using Dx.Lobby.Hints;
using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using HintServiceMeow.Core.Models.Hints;
using MapEditorReborn.API.Features.Objects;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace Dx.Lobby
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }

        public static Config Config => _config;

        public static bool IsUsingSchematic => Config.SpawnRoomType is not RoomType.Unknown;
        
        public static LobbySchematicSerializable SelectedSchematic { get; internal set; }
        
        public static SchematicObject SchematicObject { get; internal set; }

        public static IReadOnlyList<AbstractHint> Hints => _hints;

        private static List<AbstractHint> _hints = new List<AbstractHint>();

        private static Config _config;
        
        private Harmony _harmony;

        public override void OnEnabled()
        {
            _harmony = new Harmony("dx-lobby");
            _harmony.PatchAll();
            
            Instance = this;
            _config = Config;
            
            _hints.Add(new Hint
            {
                Text = Config.WaitingPlayersHint.Text,
                XCoordinate = Config.WaitingPlayersHint.Position.x,
                YCoordinate = Config.WaitingPlayersHint.Position.y,
                FontSize = Config.WaitingPlayersHint.Size,
                AutoText = WaitingPlayersHint.OnRender
            });
            
            _hints.Add(new Hint
            {
                Text = Config.TimerHint.Text,
                XCoordinate = Config.TimerHint.Position.x,
                YCoordinate = Config.TimerHint.Position.y,
                FontSize = Config.TimerHint.Size,
                AutoText = TimerHint.OnRender
            });

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
using System.Collections.Generic;
using System.ComponentModel;
using Dx.Core.API.Features;
using Dx.Lobby.API.Features.Serializables;
using Exiled.API.Interfaces;
using PlayerRoles;
using UnityEngine;

namespace Dx.Lobby
{
    public class Config : IConfig
    {
        [Description("Включен или нет")]
        public bool IsEnabled { get; set; }
        
        [Description("Дебаг или нет")]
        public bool Debug { get; set; }

        [Description("Схематы лобби")]
        public LobbySchematicSerializable[] Schematics { get; set; } =
        {
            new LobbySchematicSerializable()
            {
                SchematicName = "lobby_aquaswim",
                Position = new Vector3(200.17f, 1001.633f, -10.649f),
                SpawnPosition = new Vector3(200.17f, 1001.633f, -10.649f)
            }
        };

        [Description("Можно ли поднимать предметы")]
        public bool IsCanPickingUp { get; set; } = false;
        
        [Description("Роль в лобби")]
        public RoleTypeId LobbyRole { get; set; } = RoleTypeId.Tutorial;
        
        [Description("Раунд приостановлен")]
        public string RoundLocked { get; set; } = "Приостановлен";

        [Description("Хинты")]
        public HintSettings[] Hints { get; set; } =
        {
            new HintSettings
            {
                Enabled = true,
                Text = "Ожидание игроков - %players_count%",
                Position = Vector2.one
            },
            new HintSettings
            {
                Enabled = true,
                Text = "До начала - %before_round_time%",
                Position = Vector2.one
            }
        };
    }
}
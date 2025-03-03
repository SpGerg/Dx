using System.ComponentModel;
using Dx.Core.API.Features;
using Dx.Lobby.API.Features.Serializables;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using PlayerRoles;
using UnityEngine;

namespace Dx.Lobby
{
    public class Config : IConfig
    {
        [Description("Включен или нет")] public bool IsEnabled { get; set; }

        [Description("Дебаг или нет")] public bool Debug { get; set; }

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

        [Description("Комната спавна (Unknown если использовать схематик)")]
        public RoomType SpawnRoomType { get; set; } = RoomType.Unknown;
        
        [Description("Место спавна в комнате")]
        public Vector3 RoomOffset { get; set; }

        [Description("Хинт ожидания игроков")]
        public HintSettings WaitingPlayersHint { get; set; } = new HintSettings
        {
            Enabled = true,
            Text = "Ожидание игроков: %count%",
            Position = new Vector2(0, 800),
            Size = 25
        };
        
        [Description("Хинт ожидания игроков")]
        public HintSettings TimerHint { get; set; } = new HintSettings
        {
            Enabled = true,
            Text = "До начало: %time%",
            Position = new Vector2(0, 700),
            Size = 25
        };
    }
}
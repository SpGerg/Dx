using System.ComponentModel;
using Dx.Core.API.Features;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using UnityEngine;

namespace Dx.NoRules.API.Features.CustomRoles.Scp575Role
{
    public class Scp575Config : IConfig
    {
        [Description("Включён или нет")]
        public bool IsEnabled { get; set; }
        
        [Description("Дебаг или нет")]
        public bool Debug { get; set; }
        
        [Description("Кулдаун специальной способности")]
        public float SpecialAbilityCooldown { get; set; } = 30f;
        
        [Description("Урон каждую секунду на поверхности Scp-575")]
        public float DamagePerSecondOnSurface { get; set; } = 12.5f;
        
        [Description("Урон от флэшки")]
        public float FlashlightDamage { get; set; } = 125;

        [Description("Сообщение тёмной комнаты Scp-575")]
        public HintSettings DarkRoomHint { get; set; } = new()
        {
            Text = "Тьма наносит по вам урон, скорее покиньте комнату!",
            Position = new Vector2(0, 700),
            Size = 30
        };
        
        [Description("Специальная способность не готова хинт")]
        public HintSettings SpecialAbilityIsNotReadyHint { get; set; } = new()
        {
            Text = "Специальная способность не работает! Осталось: %cooldown%",
            Position = new Vector2(0, 500),
            Size = 30
        };

        [Description("Урон тёмной комнаты Scp-575")]
        public float DarkRoomDamage { get; set; } = 10;
        
        [Description("Время насколько блокируются двери у специальной способности")]
        public float SpecialAbilityDoorLockTime { get; set; } = 30;
        
        [Description("Хинт если 575 на поверхности")]
        public HintSettings DeathOnSurfaceHint { get; set; } = new()
        {
            Text = "На улице находиться нельзя!",
            Position = new Vector2(0, 600),
            Size = 30
        };

        [Description("Место спавна")]
        public RoomType SpawnRoom { get; set; } = RoomType.Hcz079;
        
        [Description("Место спавна")]
        public Vector3 SpawnRoomOffset { get; set; } = Vector3.zero;
        
        [Description("Шанс спавна")]
        public float SpawnChance { get; set; } = 50f;
        
        [Description("Кэсси при смерти")]
        public CassieMessage CassieOnDeath { get; set; } = new()
        {
            Message = "SCP 575 dead",
            Translation = "SCP-575 умер"
        };
        
        [Description("Кэсси при спавне")]
        public CassieMessage CassieOnSpawn { get; set; } = new()
        {
            Message = "SCP 575 spawn",
            Translation = "SCP-575 заспавнился"
        };

        [Description("Хинт при спавне")]
        public HintSettings HintOnSpawn { get; set; } = new()
        {
            Text = "Вы Scp-575",
            Position = new Vector2(0, 300),
            Size = 24,
            Duration = 3
        };

        [Description("Задержка кэсси при спавне")]
        public float CassieOnSpawnDelay { get; set; } = 5f;

        [Description("Минимальное кол-во игроков для спавна")]
        public int MinimumPlayersToSpawn { get; set; } = 5;
    }
}
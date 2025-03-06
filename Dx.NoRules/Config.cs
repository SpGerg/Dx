using System.ComponentModel;
using Dx.Core.API.Features;
using Dx.Core.API.Features.Audio;
using Exiled.API.Interfaces;
using PlayerRoles;
using UnityEngine;

namespace Dx.NoRules
{
    public class Config : IConfig
    {
        [Description("Включен или нет")]
        public bool IsEnabled { get; set; }
        
        [Description("Дебаг или нет")]
        public bool Debug { get; set; }
        
        [Description("Включёны ли теслы")]
        public bool IsTeslaDisabled { get; set; } = true;

        [Description("Время через которое можно доспавнить")]
        public int TimeSpawn { get; set; } = 90;

        [Description("Сообщение кэсси при старте раунда")]
        public CassieMessage CassieMessageAtRoundStart { get; set; } = new()
        {
            Message = "Round starting",
            Translation = "Начало раунда"
        };

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

        [Description("Кулдаун специальной способности")]
        public float SpecialAbilityCooldown { get; set; } = 30f;
        
        [Description("Урон каждую секунду на поверхности Scp-575")]
        public float DamagePerSecondOnSurface { get; set; } = 12.5f;
        
        [Description("Урон каждую секунду на поверхности Scp-575")]
        public float FlashlightDamage { get; set; } = 125;

        [Description("Роли хаоса")]
        public RoleTypeId[] ChaosRoles { get; set; } = 
        {
            RoleTypeId.ChaosConscript,
            RoleTypeId.ChaosMarauder,
            RoleTypeId.ChaosRepressor
        };
        
        [Description("Роли нтф")]
        public RoleTypeId[] NtfRoles { get; set; } = 
        {
            RoleTypeId.NtfPrivate,
            RoleTypeId.NtfSergeant,
            RoleTypeId.NtfSpecialist
        };

        [Description("Сколько нужно энергии для .stalk")]
        public float StalkVigorCost { get; set; } = 100;
        
        [Description("Время перезарядки .stalk")]
        public float StalkCooldown { get; set; } = 100;
        
        [Description("Войс чаты")]
        public RoleTypeId[] ProximityChatRoles { get; set; } =
        {
            RoleTypeId.Scp049,
            RoleTypeId.Scp079,
            RoleTypeId.Scp096,
            RoleTypeId.Scp106,
            RoleTypeId.Scp173,
            RoleTypeId.Scp0492,
            RoleTypeId.Scp939
        };
        
        [Description("Хинт при включении")]
        public HintSettings VoiceEnabledHint { get; set; } = new()
        {
            Text = "Войс чат включён",
            Position = new Vector2(0, 800),
            Size = 25,
            Duration = 3
        };
        
        [Description("Хинт при выключении")]
        public HintSettings VoiceDisabledHint { get; set; } = new()
        {
            Text = "Войс чат выключен",
            Position = new Vector2(0, 800),
            Size = 25,
            Duration = 3
        };

        [Description("Дистанция чата")]
        public float ProximityChatDistance { get; set; } = 15f;

        [Description("Настройки аудио в начале")]
        public AudioSettings RoundStartAudio { get; set; } = new()
        {
            Name = "C.A.S.S.I.E",
            IsIntercom = true,
            Volume = 100
        };
    }
}
using System.ComponentModel;
using Dx.Core.API.Features;
using Dx.Core.API.Features.Audio;
using Exiled.API.Interfaces;
using PlayerRoles;
using UnityEngine;
using VoiceChat;

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

        [Description("Настройки аудио в начале")]
        public AudioSettings[] RoundStartAudios { get; set; } =
        {
            new()
            {
                Name = "C.A.S.S.I.E",
                Filepath = "start1.ogg",
                Channels = VoiceChatChannel.Intercom
            },
            new()
            {
                Name = "C.A.S.S.I.E",
                Filepath = "start2.ogg",
                Channels = VoiceChatChannel.Intercom
            },
            new()
            {
                Name = "C.A.S.S.I.E",
                Filepath = "start3.ogg",
                Channels = VoiceChatChannel.Intercom
            }
        };

        [Description("Урон снайперской винтовки")]
        public float SniperRifleDamage { get; set; } = 30;
        
        [Description("Перезарядка снайперской винтовки")]
        public float SniperRifleCooldown { get; set; } = 10;

        [Description("Бесконечные ли патроны")]
        public bool IsInfinityAmmo { get; set; } = true;
        
        [Description("Удалять ли патроны")]
        public bool IsDestroyAmmo { get; set; }
        
        [Description("Бесконечные ли патроны")]
        public bool IsInfinityRadio { get; set; } = true;
        
        [Description("Выключать ли свет при начале раунда")]
        public bool IsDisableLightOnRoundStarted { get; set; } = true;
        
        [Description("Выключать ли свет при начале раунда")]
        public bool IsLockDoorsOnRoundStarted { get; set; } = true;

        [Description("Сообщение когда игрок был доспавнен")]
        public HintSettings RespawnedAfterDieHint { get; set; } = new()
        {
            Enabled = true,
            Text = "Вы были доспавнены за %team%",
            Duration = 3,
            Position = new Vector2(0, 300),
            Size = 24
        };
        
        public HintSettings SniperRifleHint { get; set; } = new()
        {
            Position = new Vector2(0, 700),
            Size = 32
        };
    }
}
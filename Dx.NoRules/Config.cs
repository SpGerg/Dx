using System.ComponentModel;
using Dx.Core.API.Features;
using Dx.Core.API.Features.Audio;
using Dx.NoRules.API.Features.CustomRoles.Scp575Role;
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

        [Description("Конфиг Scp-575")]
        public Scp575Config Scp575Config { get; set; } = new();

        [Description("Урон снайперской винтовки")]
        public float SniperRifleDamage { get; set; } = 30;
        
        [Description("Перезарядка снайперской винтовки")]
        public float SniperRifleCooldown { get; set; } = 10; 
        
        public HintSettings SniperRifleHint { get; set; } = new()
        {
            Position = new Vector2(0, 700),
            Size = 32
        }; 
    }
}
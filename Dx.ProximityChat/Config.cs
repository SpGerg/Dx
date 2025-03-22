using System.ComponentModel;
using Dx.Core.API.Features;
using Exiled.API.Interfaces;
using PlayerRoles;
using UnityEngine;

namespace Dx.ProximityChat
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; }
        
        public bool Debug { get; set; }

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
    }
}
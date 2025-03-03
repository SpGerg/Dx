using System.ComponentModel;
using Dx.Core.API.Features;
using Exiled.API.Interfaces;
using PlayerRoles;

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
    }
}
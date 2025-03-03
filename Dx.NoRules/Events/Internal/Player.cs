using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using Respawning;
using Respawning.Waves;
using EventTarget = Exiled.Events.Handlers.Player;

namespace Dx.NoRules.Events.Internal
{
    internal static class Player
    {
        private static readonly Dictionary<string, DateTime> Times = new();

        public static void Register()
        {
            EventTarget.TriggeringTesla += CancelTeslaOnTeslaTriggering;
            EventTarget.Shooting += CancelAmmoDecreaseOnShooting;
            EventTarget.Died += SpawnPlayerInTeamOnDied;
            EventTarget.Verified += SpawnPlayerInTeamOnVerified;
            EventTarget.UsingRadioBattery += CancelBatteryDecreaseOnUsingRadioBattery;
        }
        
        public static void Unregister()
        {
            EventTarget.TriggeringTesla -= CancelTeslaOnTeslaTriggering;
            EventTarget.Shooting -= CancelAmmoDecreaseOnShooting;
            EventTarget.Died -= SpawnPlayerInTeamOnDied;
            EventTarget.Verified -= SpawnPlayerInTeamOnVerified;
            EventTarget.UsingRadioBattery -= CancelBatteryDecreaseOnUsingRadioBattery;
        }

        /// <summary>
        /// Деактивирует теслу если она выключена
        /// </summary>
        /// <param name="ev"></param>
        private static void CancelTeslaOnTeslaTriggering(TriggeringTeslaEventArgs ev)
        {
            if (ev.DisableTesla)
            {
                return;
            }
            
            ev.DisableTesla = Plugin.Config.IsTeslaDisabled;
        }

        /// <summary>
        /// Отменяет потребление патронов
        /// </summary>
        /// <param name="ev"></param>
        private static void CancelAmmoDecreaseOnShooting(ShootingEventArgs ev)
        {
            if (ev.Firearm.AmmoDrain is 0)
            {
                return;
            }

            ev.Firearm.MagazineAmmo++;
            ev.Firearm.AmmoDrain = 0;
        }
        
        /// <summary>
        /// Доспавнить игрока если спавн команды был N кол-во сек.
        /// </summary>
        /// <param name="ev"></param>
        private static void SpawnPlayerInTeamOnVerified(VerifiedEventArgs ev)
        {
            if (Times.TryGetValue(ev.Player.UserId, out var dateTime))
            {
                if ((DateTime.Now - dateTime).TotalSeconds < Plugin.Config.TimeSpawn)
                {
                    return;
                }
            }
            else
            {
                Times.Add(ev.Player.UserId, DateTime.Now);
            }
            
            foreach (var wave in WaveManager.Waves)
            {
                if (wave is not TimeBasedWave timeBasedWave)
                {
                    continue;
                }

                if (timeBasedWave.Timer.TimePassed > Plugin.Config.TimeSpawn)
                {
                    continue;
                }

                switch (timeBasedWave)
                {
                    case NtfSpawnWave:
                        ev.Player.Role.Set(Plugin.Config.NtfRoles.GetRandomValue(), SpawnReason.Respawn);
                        break;
                    case ChaosSpawnWave:
                        ev.Player.Role.Set(Plugin.Config.ChaosRoles.GetRandomValue(), SpawnReason.Respawn);
                        break;
                }
            }
        }
        
        /// <summary>
        /// Доспавнить игрока если спавн команды был N кол-во сек.
        /// </summary>
        /// <param name="ev"></param>
        private static void SpawnPlayerInTeamOnDied(DiedEventArgs ev)
        {
            SpawnPlayerInTeamOnVerified(new VerifiedEventArgs(ev.Player));
        }

        /// <summary>
        /// Отменить потребление энергии рации
        /// </summary>
        /// <param name="ev"></param>
        private static void CancelBatteryDecreaseOnUsingRadioBattery(UsingRadioBatteryEventArgs ev)
        {
            ev.IsAllowed = false;
        }
    }
}
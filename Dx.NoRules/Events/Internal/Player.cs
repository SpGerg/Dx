using System;
using System.Collections.Generic;
using Dx.NoRules.API.Extensions;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Respawning.Waves;
using EventTarget = Exiled.Events.Handlers.Player;
using KeycardPermissions = Interactables.Interobjects.DoorUtils.KeycardPermissions;

namespace Dx.NoRules.Events.Internal
{
    internal static class Player
    {
        private static readonly Dictionary<string, DateTime> Times = new();

        public static void Register()
        {
            EventTarget.TriggeringTesla += CancelTeslaOnTeslaTriggering;
            EventTarget.Shooting += CancelAmmoDecreaseOnShooting;
            EventTarget.Verified += SpawnPlayerInTeamOnVerified;
            EventTarget.UsingRadioBattery += CancelBatteryDecreaseOnUsingRadioBattery;
            EventTarget.InteractingDoor += OpenDoorIfHasPermissionOnInteractingDoor;
            EventTarget.ActivatingWarheadPanel += OpenWarheadIfHasPermissionOnActivatingWarheadPanel;
            EventTarget.InteractingLocker += OpenLockerIfHasPermissionOnInteractingLocker;
            EventTarget.UnlockingGenerator += OpenGeneratorIfHasPermissionOnUnlockingGenerator;
        }
        
        public static void Unregister()
        {
            EventTarget.TriggeringTesla -= CancelTeslaOnTeslaTriggering;
            EventTarget.Shooting -= CancelAmmoDecreaseOnShooting;
            EventTarget.Verified -= SpawnPlayerInTeamOnVerified;
            EventTarget.UsingRadioBattery -= CancelBatteryDecreaseOnUsingRadioBattery;
            EventTarget.InteractingDoor -= OpenDoorIfHasPermissionOnInteractingDoor;
            EventTarget.ActivatingWarheadPanel -= OpenWarheadIfHasPermissionOnActivatingWarheadPanel;
            EventTarget.InteractingLocker -= OpenLockerIfHasPermissionOnInteractingLocker;
            EventTarget.UnlockingGenerator -= OpenGeneratorIfHasPermissionOnUnlockingGenerator;
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
        /// Открыть дверь если даже карточка не в руке
        /// </summary>
        /// <param name="ev"></param>
        private static void OpenDoorIfHasPermissionOnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.IsAllowed ||
                !ev.Player.HasKeycardPermission(ev.Door.RequiredPermissions.RequiredPermissions))
            {
                return;
            }
            
            ev.IsAllowed = true;
        }
        
        /// <summary>
        /// Открыть боеголовку если даже карточка не в руке
        /// </summary>
        /// <param name="ev"></param>
        private static void OpenWarheadIfHasPermissionOnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (ev.IsAllowed ||
                !ev.Player.HasKeycardPermission(KeycardPermissions.AlphaWarhead))
            {
                return;
            }

            ev.IsAllowed = true;
        }
        
        /// <summary>
        /// Открыть боеголовку если даже карточка не в руке
        /// </summary>
        /// <param name="ev"></param>
        private static void OpenLockerIfHasPermissionOnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.IsAllowed ||
                !ev.Player.HasKeycardPermission(ev.InteractingChamber.Base.RequiredPermissions))
            {
                return;
            }

            ev.IsAllowed = true;
        }
        
        /// <summary>
        /// Открыть боеголовку если даже карточка не в руке
        /// </summary>
        /// <param name="ev"></param>
        private static void OpenGeneratorIfHasPermissionOnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (ev.IsAllowed ||
                !ev.Player.HasKeycardPermission((KeycardPermissions) ev.Generator.KeycardPermissions))
            {
                return;
            }

            ev.IsAllowed = true;
        }

        /// <summary>
        /// Доспавнить игрока если спавн команды был N кол-во сек.
        /// </summary>
        /// <param name="ev"></param>
        private static void SpawnPlayerInTeamOnVerified(VerifiedEventArgs ev)
        {
            if (!Round.InProgress)
            {
                return;
            }

            if (Times.TryGetValue(ev.Player.UserId, out var dateTime))
            {
                if (dateTime == Plugin.LastSpawnedTime)
                {
                    return;
                }
            }
            else
            {
                Times.Add(ev.Player.UserId, Plugin.LastSpawnedTime);
            }
            
            if (Plugin.LastSpawnedWave.Timer.TimePassed > Plugin.Config.TimeSpawn)
            {
                return;
            }
            
            switch (Plugin.LastSpawnedWave)
            {
                case NtfSpawnWave:
                    ev.Player.Role.Set(Plugin.Config.NtfRoles.GetRandomValue(), SpawnReason.Respawn);
                    break;
                case ChaosSpawnWave:
                    ev.Player.Role.Set(Plugin.Config.ChaosRoles.GetRandomValue(), SpawnReason.Respawn);
                    break;
            }
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
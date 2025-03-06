using System;
using Dx.Core.API.Features.Audio;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Server;
using MEC;
using Respawning.Waves;
using UnityEngine;
using EventTarget = Exiled.Events.Handlers.Server;

namespace Dx.NoRules.Events.Internal
{
    internal static class Server
    {
        private static AudioBot _audioBot;
        
        public static void Register()
        {
            EventTarget.RoundStarted += BlockDoorsAndOffLightOnRoundStarted;
            EventTarget.WaitingForPlayers += ClearCoroutinesOnWaitingForPlayers;
            EventTarget.RespawnedTeam += SetLastSpawnedWaveOnSpawned;

            _audioBot = new BasicAudioBot(Plugin.Config.RoundStartAudio);
        }
        
        public static void Unregister()
        {
            EventTarget.RoundStarted -= BlockDoorsAndOffLightOnRoundStarted;
            EventTarget.WaitingForPlayers -= ClearCoroutinesOnWaitingForPlayers;
            EventTarget.RespawnedTeam -= SetLastSpawnedWaveOnSpawned;
        }

        /// <summary>
        /// Заблокировать двери, выключить свет и вывести кэсси при старте раунда.
        /// </summary>
        private static void BlockDoorsAndOffLightOnRoundStarted()
        {
            //Другие плагины могут блокировать до начала раунда и также их открывать, заблокируем после открытия.
            Timing.CallDelayed(Timing.WaitForOneFrame, () =>
            {
                foreach (var door in Door.List)
                {
                    door.Lock(5f, DoorLockType.AdminCommand);
                }
                
                Exiled.API.Features.Map.TurnOffAllLights(5f);
                
                Plugin.Config.CassieMessageAtRoundStart.Speak();
            });
            
            _audioBot.Play(Vector3.zero);
        }

        /// <summary>
        /// Установить последнуюю заспавненную волну
        /// </summary>
        /// <param name="ev"></param>
        private static void SetLastSpawnedWaveOnSpawned(RespawnedTeamEventArgs ev)
        {
            Plugin.LastSpawnedWave = (TimeBasedWave) ev.Wave;
            Plugin.LastSpawnedTime = DateTime.Now;
        }
        
        /// <summary>
        /// Очистить корутины
        /// </summary>
        private static void ClearCoroutinesOnWaitingForPlayers()
        {
            Plugin.Coroutines.Clear();
        }
    }
}
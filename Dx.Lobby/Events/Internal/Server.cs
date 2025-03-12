using System.Collections.Generic;
using Dx.Lobby.API.Features.Serializables;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using HintServiceMeow.Core.Utilities;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Serializable;
using MEC;
using UnityEngine;
using EventTarget = Exiled.Events.Handlers.Server;

namespace Dx.Lobby.Events.Internal
{
    internal static class Server
    {
        private static CoroutineHandle _coroutine;

        private const float _hintDuration = 3f;
        
        public static void Register()
        {
            EventTarget.WaitingForPlayers += SpawnSchematicOnWaitingForPlayers;
            EventTarget.RoundStarted += DisableGodModeOnRoundStarted;
        }

        public static void Unregister()
        {
            EventTarget.WaitingForPlayers -= SpawnSchematicOnWaitingForPlayers;
            EventTarget.RoundStarted -= DisableGodModeOnRoundStarted;
        }

        private static void SpawnSchematicOnWaitingForPlayers()
        {
            var schematicInfo = Plugin.Config.Schematics.GetRandomValue();
            
            Plugin.SelectedSchematic = schematicInfo;
            
            GameObject.Find("StartRound").transform.localScale = Vector3.zero;
            
            if (!Plugin.IsUsingSchematic)
            {
                var room = Room.Get(Plugin.Config.SpawnRoomType);

                foreach (var door in room.Doors)
                {
                    door.Lock(DoorLockType.AdminCommand);
                }

                Plugin.SpawnPosition = room.WorldPosition(Plugin.Config.RoomOffset);
                
                return;
            }
            
            var data = MapUtils.GetSchematicDataByName(schematicInfo.SchematicName);

            if (data is null)
            {
                Log.Error($"Не найдена лобби с именем {schematicInfo.SchematicName}");
            }

            Timing.RunCoroutine(SchematicSpawnCoroutine(schematicInfo, data));
        }

        private static void DisableGodModeOnRoundStarted()
        {
            foreach (var player in Exiled.API.Features.Player.List)
            {
                if (player.IsNPC || player.ReferenceHub.playerStats is null)
                {
                    continue;
                }
                
                player.IsGodModeEnabled = false;
                
                var playerDisplay = PlayerDisplay.Get(player);
            
                foreach (var hint in Plugin.Hints)
                {
                    playerDisplay.RemoveHint(hint);
                }
            }
            
            foreach (var door in Door.List)
            {
                if (door.DoorLockType is not DoorLockType.AdminCommand)
                {
                    continue;
                }
                
                door.Unlock();                
            }
            
            Plugin.SchematicObject?.Destroy();
        }

        private static IEnumerator<float> SchematicSpawnCoroutine(LobbySchematicSerializable serializable, SchematicObjectDataList data)
        {
            Plugin.SpawnPosition = serializable.Position;
            
            while (MapEditorReborn.API.API.ObjectPrefabs is null)
            {
                yield return Timing.WaitForSeconds(Timing.WaitForOneFrame);
            }

            Plugin.SchematicObject = ObjectSpawner.SpawnSchematic(serializable.SchematicName, serializable.Position, Quaternion.identity, Vector3.one, data, true);
            MapEditorReborn.API.API.SpawnedObjects.Add(Plugin.SchematicObject);
        }
    }
}
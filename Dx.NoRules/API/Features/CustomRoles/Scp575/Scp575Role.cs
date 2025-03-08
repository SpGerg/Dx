using System.Collections.Generic;
using System.Linq;
using Dx.Core.API.Features;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.Core.Models.HintContent;
using HintServiceMeow.Core.Utilities;
using MEC;
using PlayerRoles;
using PluginAPI.Events;
using UnityEngine;
using EventTargetPlayer = Exiled.Events.Handlers.Player;
using EventTargetMap = Exiled.Events.Handlers.Map;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace Dx.NoRules.API.Features.CustomRoles.Scp575
{
    [CustomRole(RoleTypeId.Scp106)]
    public class Scp575Role : CustomRole
    {
        private static readonly Dictionary<Player, Cooldown> _cooldowns = new();

        public override uint Id { get; set; } = 50;

        public override int MaxHealth { get; set; } = 700;

        public override string Name { get; set; } = "Scp-575";

        public override string Description { get; set; } = "Тьма";

        public override string CustomInfo { get; set; } = "Тьма";

        public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;

        public override bool IgnoreSpawnSystem { get; set; } = true;

        private const string _cooldownHintId = "special-ability-cooldown";

        private const float _flashbangDistance = 10;

        private Hint _targetHint;

        private Hint _deathOnSurfaceHint;

        protected override void SubscribeEvents()
        {
            SpawnChance = Plugin.Config.Scp575Config.SpawnChance;
            SpawnProperties.RoomSpawnPoints.Add(new RoomSpawnPoint
            {
                Chance = SpawnChance,
                Offset = Plugin.Config.Scp575Config.SpawnRoomOffset,
                Room = Plugin.Config.Scp575Config.SpawnRoom
            });

            _targetHint = new Hint
            {
                Content = new StringContent(Plugin.Scp575Config.DarkRoomHint.Text),
                XCoordinate = Plugin.Scp575Config.DarkRoomHint.Position.x,
                YCoordinate = Plugin.Scp575Config.DarkRoomHint.Position.y,
                FontSize = Plugin.Scp575Config.DarkRoomHint.Size
            };
            
            _deathOnSurfaceHint = new Hint
            {
                Content = new StringContent(Plugin.Scp575Config.DeathOnSurfaceHint.Text),
                XCoordinate = Plugin.Scp575Config.DeathOnSurfaceHint.Position.x,
                YCoordinate = Plugin.Scp575Config.DeathOnSurfaceHint.Position.y,
                FontSize = Plugin.Scp575Config.DeathOnSurfaceHint.Size
            };
            
            EventTargetPlayer.PickingUpItem += CancelPickingUpOnPickingUp;
            EventTargetPlayer.InteractingDoor += CancelDoorOpenOnInteractingDoor;
            EventTargetPlayer.TogglingNoClip += ExecuteSafeAbilityOnTogglingNoClip;
            EventTargetPlayer.Hurting += CancelOtherDamagesOnHurting;

            EventTargetMap.ExplodingGrenade += TakeDamageFromFlashlightOnExplodingGrenade;
            EventTargetMap.GeneratorActivating += KillScp575OnGeneratorActivated;

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            EventTargetPlayer.PickingUpItem -= CancelPickingUpOnPickingUp;
            EventTargetPlayer.InteractingDoor -= CancelDoorOpenOnInteractingDoor;
            EventTargetPlayer.TogglingNoClip -= ExecuteSafeAbilityOnTogglingNoClip;
            EventTargetPlayer.Hurting -= CancelOtherDamagesOnHurting;
            
            EventTargetMap.ExplodingGrenade -= TakeDamageFromFlashlightOnExplodingGrenade;
            EventTargetMap.GeneratorActivating -= KillScp575OnGeneratorActivated;

            base.UnsubscribeEvents();
        }

        protected override void RoleAdded(Player player)
        {
            Timing.CallDelayed(0.75f, () =>
            {
                player.ChangeAppearance(RoleTypeId.Scp106);
            });
            
            _cooldowns.Add(player, new Cooldown(Plugin.Scp575Config.SpecialAbilityCooldown));
            Plugin.Coroutines.Start($"{player.UserId}-dark-room-coroutine", DarkRoomCoroutine(player));
            
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            _cooldowns.Remove(player);
            
            base.RoleRemoved(player);
        }

        private void KillScp575OnGeneratorActivated(GeneratorActivatingEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                return;
            }

            // + 1 т.к -ing ивент
            if ((Recontainer.EngagedGeneratorCount + 1) != Generator.List.Count)
            {
                return;
            }

            foreach (var player in Player.List.Where(Check))
            {
                player.Kill("Recontain");
            }
        }

        /// <summary>
        /// Нанести урон 
        /// </summary>
        /// <param name="ev"></param>
        private void TakeDamageFromFlashlightOnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            if (ev.Projectile.ProjectileType is not ProjectileType.Flashbang)
            {
                return;
            }

            foreach (var player in Player.List)
            {
                if (player.IsDead)
                {
                    continue;
                }
                
                if (Vector3.Distance(player.Position, ev.Projectile.Position) > _flashbangDistance)
                {
                    continue;
                }
                
                if (!Check(player))
                {
                    continue;
                }
                
                player.Hurt(Plugin.Scp575Config.FlashlightDamage, DamageType.Custom);
            }
        }
        
        /// <summary>
        /// Отменить другие виды урона
        /// </summary>
        /// <param name="ev"></param>
        private void CancelOtherDamagesOnHurting(HurtingEventArgs ev)
        {
            if (!Check(ev.Player))
            {
                return;
            }

            if (ev.DamageHandler.Type is DamageType.MicroHid or DamageType.Decontamination or DamageType.Custom)
            {
                return;
            }

            ev.IsAllowed = false;
        }
        
        /// <summary>
        /// Использовать сейф способность при нажатии Alt
        /// </summary>
        /// <param name="ev"></param>
        private void ExecuteSafeAbilityOnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (!Check(ev.Player))
            {
                return;
            }

            if (!_cooldowns.TryGetValue(ev.Player, out var cooldown))
            {
                _cooldowns.Add(ev.Player, cooldown = new Cooldown(Plugin.Scp575Config.SpecialAbilityCooldown));
            }

            if (!cooldown.IsReady)
            {
                var playerDisplay = PlayerDisplay.Get(ev.Player);

                var hint = playerDisplay.GetHint(_cooldownHintId);

                if (hint is not null)
                {
                    hint.Text = Plugin.Scp575Config.SpecialAbilityIsNotReadyHint.Text.Replace("%cooldown%",
                        ((int) cooldown.RemainingTime).ToString());
                    
                    return;
                }
                
                hint = new Hint
                {
                    Id = _cooldownHintId,
                    Content = new StringContent(Plugin.Scp575Config.SpecialAbilityIsNotReadyHint.Text.Replace("%cooldown%", ((int) cooldown.RemainingTime).ToString())),
                    XCoordinate = Plugin.Scp575Config.SpecialAbilityIsNotReadyHint.Position.x,
                    YCoordinate = Plugin.Scp575Config.SpecialAbilityIsNotReadyHint.Position.y,
                    FontSize = Plugin.Scp575Config.SpecialAbilityIsNotReadyHint.Size
                };

                playerDisplay.AddHint(hint);

                Plugin.Coroutines.CallDelayed(Plugin.Scp575Config.SpecialAbilityIsNotReadyHint.Duration, () =>
                {
                    playerDisplay.RemoveHint(hint);
                });
                
                return;
            }

            foreach (var door in ev.Player.CurrentRoom.Doors)
            {
                door.Lock(Plugin.Scp575Config.SpecialAbilityDoorLockTime, DoorLockType.AdminCommand);
            }

            cooldown.ForceUse();
        }

        /// <summary>
        /// Отменить открытие определённых дверей если игрок 575
        /// </summary>
        /// <param name="ev"></param>
        private void CancelDoorOpenOnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!Check(ev.Player))
            {
                return;
            }

            if (!ev.Door.Type.IsElevator() && !ev.Door.Type.IsGate())
            {
                return;
            }
            
            ev.IsAllowed = false;
        }
        /// <summary>
        /// Отменить подбирание предмета если игрок 575
        /// </summary>
        /// <param name="ev"></param>
        private void CancelPickingUpOnPickingUp(PickingUpItemEventArgs ev)
        {
            CancelEventIfScp575(ev.Player, ev);
        }

        /// <summary>
        /// Отменить ивент если роль Scp575
        /// </summary>
        /// <param name="player"></param>
        /// <param name="deniableEvent"></param>
        private void CancelEventIfScp575(Player player, IDeniableEvent deniableEvent)
        {
            if (!Check(player))
            {
                return;
            }

            deniableEvent.IsAllowed = false;
        }

        private IEnumerator<float> DarkRoomCoroutine(Player player)
        {
            var targetRoom = player.CurrentRoom;

            var damage = Plugin.Scp575Config.DarkRoomDamage;

            var lastPlayers = targetRoom.Players;

            while (player.IsConnected && Round.InProgress && Check(player))
            {
                if (targetRoom != player.CurrentRoom)
                {
                    //Включаем свет
                    targetRoom.TurnOffLights(0.0f);

                    if (player.CurrentRoom.Type is RoomType.Surface)
                    {
                        damage /= 2;

                        Plugin.Coroutines.Start($"{player.UserId}-death-on-surface-coroutine", DeathOnSurfaceCoroutine(player));
                    }
                    else
                    {
                        damage = Plugin.Scp575Config.DarkRoomDamage;
                    }

                    var merged = new List<Player>(player.CurrentRoom.Players.Count() + lastPlayers.Count());
                    merged.AddRange(player.CurrentRoom.Players);
                    merged.AddRange(lastPlayers);

                    foreach (var target in merged)
                    {
                        var playerDisplay = PlayerDisplay.Get(target);
                        playerDisplay.RemoveHint(_targetHint);
                    }

                    targetRoom = player.CurrentRoom;
                    targetRoom.TurnOffLights();

                    Plugin.Coroutines.CallDelayed(Timing.WaitForOneFrame, () =>
                    {
                        player.SendFakeSyncVar(targetRoom.RoomLightControllerNetIdentity, typeof(RoomLightController), nameof(RoomLightController.NetworkLightsEnabled), true);
                    });

                    lastPlayers = player.CurrentRoom.Players;

                    continue;
                }

                var except = targetRoom.Players.Except(lastPlayers);

                foreach (var leaved in except)
                {
                    var playerDisplay = PlayerDisplay.Get(leaved);

                    if (leaved.CurrentRoom == targetRoom)
                    {
                        playerDisplay.AddHint(_targetHint);
                    }
                    else
                    {
                        playerDisplay.RemoveHint(_targetHint);
                    }
                }

                foreach (var target in targetRoom.Players)
                {
                    if (target.IsScp || target == player)
                    {
                        continue;
                    }
                        
                    var playerDisplay = PlayerDisplay.Get(target);
                    playerDisplay.AddHint(_targetHint);
                        
                    target.Hurt(player, damage, DamageType.Bleeding, deathText: null);
                }

                lastPlayers = targetRoom.Players;

                yield return Timing.WaitForSeconds(1f);
            }
            
            targetRoom.TurnOffLights(0.0f);
            
            var merged1 = new List<Player>(player.CurrentRoom.Players.Count() + lastPlayers.Count());
            merged1.AddRange(player.CurrentRoom.Players);
            merged1.AddRange(lastPlayers);

            foreach (var target in merged1)
            {
                var playerDisplay = PlayerDisplay.Get(target);
                playerDisplay.RemoveHint(_targetHint);
            }
        }

        private IEnumerator<float> DeathOnSurfaceCoroutine(Player player)
        {
            var playerDisplay = PlayerDisplay.Get(player);
            playerDisplay.AddHint(_deathOnSurfaceHint);

            while (player.IsAlive && Check(player))
            {
                if (player.CurrentRoom?.Type is not RoomType.Surface)
                {
                    break;
                }
                
                player.Hurt(Plugin.Scp575Config.DamagePerSecondOnSurface, DamageType.Custom);

                yield return Timing.WaitForSeconds(1);
            }
            
            playerDisplay.RemoveHint(_deathOnSurfaceHint);
        }
    }
}
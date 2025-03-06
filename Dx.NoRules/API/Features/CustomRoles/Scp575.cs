using System.Collections.Generic;
using System.Linq;
using Dx.Core.API.Features;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.Core.Models.HintContent;
using HintServiceMeow.Core.Utilities;
using MEC;
using PlayerRoles;
using EventTargetPlayer = Exiled.Events.Handlers.Player;
using EventTargetMap = Exiled.Events.Handlers.Map;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace Dx.NoRules.API.Features.CustomRoles
{
    [CustomRole(RoleTypeId.Scp106)]
    public class Scp575 : CustomRole
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

        private static readonly Hint TargetHint = new()
        {
            Content = new StringContent(Plugin.Config.DarkRoomHint.Text),
            XCoordinate = Plugin.Config.DarkRoomHint.Position.x,
            YCoordinate = Plugin.Config.DarkRoomHint.Position.y,
            FontSize = Plugin.Config.DarkRoomHint.Size
        };

        private static readonly Hint DeathOnSurfaceHint = new()
        {
            Content = new StringContent(Plugin.Config.DeathOnSurfaceHint.Text),
            XCoordinate = Plugin.Config.DeathOnSurfaceHint.Position.x,
            YCoordinate = Plugin.Config.DeathOnSurfaceHint.Position.y,
            FontSize = Plugin.Config.DeathOnSurfaceHint.Size
        };

        protected override void SubscribeEvents()
        {
            EventTargetPlayer.PickingUpItem += CancelPickingUpOnPickingUp;
            EventTargetPlayer.InteractingDoor += CancelDoorOpenOnInteractingDoor;
            EventTargetPlayer.TogglingNoClip += ExecuteSafeAbilityOnTogglingNoClip;
            EventTargetPlayer.Hurting += CancelOtherDamagesOnHurting;

            EventTargetMap.ExplodingGrenade += TakeDamageFromFlashlightOnExplodingGrenade;

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            EventTargetPlayer.PickingUpItem -= CancelPickingUpOnPickingUp;
            EventTargetPlayer.InteractingDoor -= CancelDoorOpenOnInteractingDoor;
            EventTargetPlayer.TogglingNoClip -= ExecuteSafeAbilityOnTogglingNoClip;
            EventTargetPlayer.Hurting -= CancelOtherDamagesOnHurting;
            
            EventTargetMap.ExplodingGrenade -= TakeDamageFromFlashlightOnExplodingGrenade;

            base.UnsubscribeEvents();
        }

        protected override void RoleAdded(Player player)
        {
            player.ChangeAppearance(RoleTypeId.Scp106);
            
            _cooldowns.Add(player, new Cooldown(Plugin.Config.SpecialAbilityCooldown));
            Plugin.Coroutines.Start($"{player.UserId}-dark-room-coroutine", DarkRoomCoroutine(player));
            
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            _cooldowns.Remove(player);
            
            base.RoleRemoved(player);
        }

        /// <summary>
        /// Нанести урон 
        /// </summary>
        /// <param name="ev"></param>
        private void TakeDamageFromFlashlightOnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            if (ev.Projectile.Type is not ItemType.Flashlight)
            {
                return;
            }

            var room = Room.Get(ev.Projectile.Position);

            if (room is null)
            {
                return;
            }

            foreach (var player in room.Players)
            {
                if (!Check(player))
                {
                    continue;
                }
                
                player.Hurt(Plugin.Config.FlashlightDamage, DamageType.Bleeding);
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

            if (ev.DamageHandler.Type is DamageType.MicroHid or DamageType.Decontamination)
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
                _cooldowns.Add(ev.Player, cooldown = new Cooldown(Plugin.Config.SpecialAbilityCooldown));
            }

            if (!cooldown.IsReady)
            {
                var playerDisplay = PlayerDisplay.Get(ev.Player);

                var hint = playerDisplay.GetHint(_cooldownHintId);

                if (hint is not null)
                {
                    hint.Text = Plugin.Config.SpecialAbilityIsNotReadyHint.Text.Replace("%cooldown%",
                        cooldown.RemainingTime.ToString());
                    
                    return;
                }
                
                hint = new Hint
                {
                    Id = _cooldownHintId,
                    Content = new StringContent(Plugin.Config.SpecialAbilityIsNotReadyHint.Text.Replace("%cooldown%", cooldown.RemainingTime.ToString())),
                    XCoordinate = Plugin.Config.SpecialAbilityIsNotReadyHint.Position.x,
                    YCoordinate = Plugin.Config.SpecialAbilityIsNotReadyHint.Position.y,
                    FontSize = Plugin.Config.SpecialAbilityIsNotReadyHint.Size
                };

                playerDisplay.AddHint(hint);

                Plugin.Coroutines.CallDelayed(Plugin.Config.SpecialAbilityIsNotReadyHint.Duration, () =>
                {
                    playerDisplay.RemoveHint(hint);
                });
            }

            foreach (var door in ev.Player.CurrentRoom.Doors)
            {
                door.Lock(Plugin.Config.SpecialAbilityDoorLockTime, DoorLockType.AdminCommand);
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
            targetRoom.TurnOffLights();
            
            //player.SendFakeSyncVar(targetRoom.RoomLightControllerNetIdentity, typeof(RoomLightController), "LightsEnabled", true);
            
            var damage = Plugin.Config.DarkRoomDamage;

            var lastPlayers = targetRoom.Players;

            while (player.IsConnected && Round.InProgress && Check(player))
            {
                if (targetRoom != player.CurrentRoom)
                {
                    //Включаем свет
                    targetRoom.TurnOffLights(0.0f);
                    //player.SendFakeSyncVar(targetRoom.RoomLightControllerNetIdentity, typeof(RoomLightController), "LightsEnabled", false);

                    if (player.CurrentRoom.Type is RoomType.Surface)
                    {
                        damage = Plugin.Config.DarkRoomDamage / 2;

                        Plugin.Coroutines.Start($"{player.UserId}-death-on-surface-coroutine", DeathOnSurfaceCoroutine(player));
                    }
                    else
                    {
                        damage = Plugin.Config.DarkRoomDamage;
                    }

                    var merged = new List<Player>(player.CurrentRoom.Players.Count() + lastPlayers.Count());
                    merged.AddRange(player.CurrentRoom.Players);
                    merged.AddRange(lastPlayers);

                    foreach (var target in merged)
                    {
                        var playerDisplay = PlayerDisplay.Get(target);
                        playerDisplay.RemoveHint(TargetHint);
                    }

                    targetRoom = player.CurrentRoom;
                    targetRoom.TurnOffLights();
                    //player.SendFakeSyncVar(targetRoom.RoomLightControllerNetIdentity, typeof(RoomLightController), "LightsEnabled", true);
                        
                    lastPlayers = player.CurrentRoom.Players;

                    continue;
                }

                var except = targetRoom.Players.Except(lastPlayers);

                foreach (var leaved in except)
                {
                    var playerDisplay = PlayerDisplay.Get(leaved);

                    if (leaved.CurrentRoom == targetRoom)
                    {
                        playerDisplay.AddHint(TargetHint);
                    }
                    else
                    {
                        playerDisplay.RemoveHint(TargetHint);
                    }
                }

                foreach (var target in targetRoom.Players)
                {
                    if (target == player)
                    {
                        continue;
                    }
                        
                    var playerDisplay = PlayerDisplay.Get(target);
                    playerDisplay.AddHint(TargetHint);
                        
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
                playerDisplay.RemoveHint(TargetHint);
            }
        }

        private IEnumerator<float> DeathOnSurfaceCoroutine(Player player)
        {
            var playerDisplay = PlayerDisplay.Get(player);
            playerDisplay.AddHint(DeathOnSurfaceHint);

            while (player.IsAlive && Check(player))
            {
                if (player.CurrentRoom?.Type is not RoomType.Surface)
                {
                    playerDisplay.RemoveHint(DeathOnSurfaceHint);
                    
                    yield break;
                }
                
                player.Hurt(Plugin.Config.DamagePerSecondOnSurface, "Вы слишком долго были на поверхности!");

                yield return Timing.WaitForSeconds(1);
            }
            
            playerDisplay.RemoveHint(DeathOnSurfaceHint);
        }
    }
}
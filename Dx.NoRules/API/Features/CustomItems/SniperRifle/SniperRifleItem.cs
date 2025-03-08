using System.Collections.Generic;
using Dx.Core.API.Features;
using Dx.NoRules.API.Features.CustomItems.SniperRifle.Hints;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.Core.Utilities;
using InventorySystem.Items;
using InventorySystem.Items.Firearms.Attachments;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace Dx.NoRules.API.Features.CustomItems.SniperRifle
{
    [CustomItem(ItemType.GunE11SR)]
    public class SniperRifleItem : CustomWeapon
    {
        public static IReadOnlyDictionary<ItemBase, Cooldown> Cooldowns => _cooldowns;

        private static readonly Dictionary<ItemBase, Cooldown> _cooldowns = new();

        public override uint Id { get; set; } = 50;

        public override string Name { get; set; } = "Снайперская винтовка";

        public override string Description { get; set; } = "Снайперская винтовка";

        public override float Weight { get; set; } = ItemType.GunE11SR.GetItemBase().Weight;
        
        public override SpawnProperties SpawnProperties { get; set; }

        public override ItemType Type { get; set; } = ItemType.GunE11SR;
        
        public override float Damage { get; set; }

        private Hint _reloadingHint;

        public override AttachmentName[] Attachments { get; set; } =
        {
            AttachmentName.ScopeSight
        };

        protected override void ShowSelectedMessage(Player player)
        {
            var playerDisplay = PlayerDisplay.Get(player);
            playerDisplay.AddHint(_reloadingHint);
            
            _cooldowns.Add(player.CurrentItem.Base, new Cooldown(Plugin.Config.SniperRifleCooldown));

            base.ShowSelectedMessage(player);
        }

        protected override void SubscribeEvents()
        {
            _reloadingHint = new Hint
            {
                AutoText = SniperRifleHint.OnRender,
                XCoordinate = Plugin.Config.SniperRifleHint.Position.x,
                YCoordinate = Plugin.Config.SniperRifleHint.Position.y,
                FontSize = Plugin.Config.SniperRifleHint.Size
            };
            
            Damage = Plugin.Config.SniperRifleDamage;
            
            base.SubscribeEvents();
        }

        protected override void OnShooting(ShootingEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                return;
            }

            if (!_cooldowns.TryGetValue(ev.Item.Base, out var customItem))
            {
                return;
            }

            if (!customItem.IsReady)
            {
                ev.IsAllowed = false;
            }
            
            ev.Firearm.Damage = Damage;

            base.OnShooting(ev);
        }
    }
}
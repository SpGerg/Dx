using AdminToys;
using Dx.NoRules.API.Features.ShootingEffects.Components;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using Mirror;
using UnityEngine;
using EventTarget = Exiled.Events.Handlers.Player;
using Light = Exiled.API.Features.Toys.Light;

namespace Dx.NoRules.API.Features.ShootingEffects.Events.Internal
{
    internal static class Player
    {
        public static void Register()
        {
            EventTarget.Shooting += OnShooting;
        }
        
        public static void Unregister()
        {
            EventTarget.Shooting -= OnShooting;
        }

        private static void OnShooting(ShootingEventArgs ev)
        {
            var player = ev.Player;

            var primitive = Primitive.Create(PrimitiveType.Cube);

            primitive.Flags -= PrimitiveFlags.Collidable;
            primitive.Scale = Vector3.one * 0.2f;

            primitive.Spawn();
            
            var mainPrimitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mainPrimitive.transform.position = player.Position;
            mainPrimitive.transform.localScale = primitive.Scale;
            
            mainPrimitive.gameObject.AddComponent<MovingForward>().Initialize(primitive, ev.Direction);
            
            var collider = mainPrimitive.gameObject.GetComponent<BoxCollider>();
            collider.size = primitive.Scale;
            collider.isTrigger = true;

            NetworkServer.Spawn(mainPrimitive);
        }
    }
}
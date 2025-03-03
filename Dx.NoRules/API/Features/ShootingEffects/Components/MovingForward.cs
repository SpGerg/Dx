using System;
using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dx.NoRules.API.Features.ShootingEffects.Components
{
    public class MovingForward : MonoBehaviour
    {
        private Primitive _primitive;
        
        private Vector3 _direction;

        private bool _isWithExplode;

        private readonly LayerMask _wallLayer = LayerMask.GetMask("Default");
        
        public void Initialize(Primitive primitive, Vector3 direction, bool isWithExplode = true)
        {
            _primitive = primitive;
            _direction = direction;
            _isWithExplode = isWithExplode;
        }

        public void FixedUpdate()
        {
            if (_isWithExplode)
            {
                transform.position += _direction * (Time.deltaTime * 20);
            }
            
            _primitive.Position = transform.position;

            var ray = new Ray(transform.position, _direction);

            if (!Physics.Raycast(ray, out var hitInfo, 1, _wallLayer))
            {
                return;
            }
            
            if (_isWithExplode)
            {
                Explode();
            }

            _primitive.Destroy();
            NetworkServer.Destroy(gameObject);
        }

        public void Explode()
        {
            for (var i = 0; i < 50; i++)
            {
                var primitive = Primitive.Create(PrimitiveType.Cube);

                primitive.Flags -= PrimitiveFlags.Collidable;
                primitive.Scale = Vector3.one * 0.2f;

                primitive.Spawn();
            
                var mainPrimitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                mainPrimitive.transform.position = transform.position;
                mainPrimitive.transform.localScale = primitive.Scale;

                mainPrimitive.gameObject.AddComponent<MovingForward>().Initialize(primitive, Vector3.zero, false);
                
                var collider = mainPrimitive.gameObject.GetComponent<BoxCollider>();
                collider.size = primitive.Scale;
                collider.isTrigger = true;

                var rigidBody = mainPrimitive.gameObject.AddComponent<Rigidbody>();
                rigidBody.isKinematic = false;
                rigidBody.mass = 5;
                rigidBody.velocity = Random.rotation.eulerAngles * 0.1f;
            }
        }
    }
}
﻿using UnityEngine;

namespace RaycastEngine2D
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxController2D : RayCastController
    {        

        [HideInInspector] public CollisionInfo Collisions;
        [HideInInspector] public RaycastOrigins RayCastOrigins;

        public void Move(Vector3 velocity)
        {
            UpdateRaycastOrigins();
            Collisions.Reset();

            if (Mathf.Abs(velocity.x) > float.Epsilon)
            {
                CheckHorizontalCollisions(ref velocity);
            }

            if (Mathf.Abs(velocity.y) > float.Epsilon)
            {
                CheckVerticalCollisions(ref velocity);
            }
            
            if (float.IsNaN(velocity.x) || float.IsNaN(velocity.y) || float.IsNaN(velocity.z))
            {
                Debug.LogWarning("Velocity of " + gameObject.name + " is NaN, will not translate");
                return;
            }
            

            transform.Translate(velocity);
            Physics2D.SyncTransforms();
        }

        internal override void CheckHorizontalCollisions(ref Vector3 velocity)
        {
            var rayOrigin = RayCastOrigins.Center;
            var rayLength = Mathf.Abs(velocity.x) + SKINWIDTH;
            var directionX = Mathf.Sign(velocity.x);

            var hit = Physics2D.BoxCast(rayOrigin, _collider.bounds.size, 0, Vector2.right * directionX, rayLength, CollisionMask);

            if (hit)
            {
                velocity.x = (hit.distance - SKINWIDTH) * directionX;
                Collisions.Left = Mathf.Abs(directionX - (-1)) < float.Epsilon;
                Collisions.Right = Mathf.Abs(directionX - 1) < float.Epsilon;
            }


            Debug.DrawRay(rayOrigin, Vector2.right * directionX * 2,
                Collisions.Left || Collisions.Right ? Color.blue : Color.red);
        }

        internal override void CheckVerticalCollisions(ref Vector3 velocity)
        {
            var rayOrigin = RayCastOrigins.Center;
            var rayLength = Mathf.Abs(velocity.y) + SKINWIDTH;
            var directionY = Mathf.Sign(velocity.y);

            var hit = Physics2D.BoxCast(rayOrigin, _collider.bounds.size, 0, Vector2.up * directionY, rayLength, CollisionMask);
            if (hit)
            {
                velocity.y = (hit.distance - SKINWIDTH) * directionY;
                Collisions.Below = Mathf.Abs(directionY - (-1)) < float.Epsilon;
                Collisions.Above = Mathf.Abs(directionY - 1) < float.Epsilon;
            }

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * 2,
                Collisions.Below || Collisions.Above ? Color.blue : Color.red);
        }     
        
        internal override void CheckDiagonalCollisions(ref Vector3 velocity)
        {
            var rayOrigin = RayCastOrigins.Center;
            var rayLength = Mathf.Abs(velocity.magnitude) + SKINWIDTH;
            var directionXY = Mathf.Sign(velocity.y) * Mathf.Sign(velocity.x);

            var hit = Physics2D.BoxCast(rayOrigin, _collider.bounds.size, Vector3.SignedAngle(Vector3.right, velocity, Vector3.forward), Vector2.right, rayLength, CollisionMask);
            if (hit)
            {
                velocity.y = (hit.distance - SKINWIDTH) * directionXY;
                Collisions.Below = Mathf.Abs(directionXY - (-1)) < float.Epsilon;
                Collisions.Above = Mathf.Abs(directionXY - 1) < float.Epsilon;
            }

            Debug.DrawRay(rayOrigin, new Vector3(velocity.x, velocity.y, 0),
                Collisions.Below || Collisions.Above ? Color.white : Color.black);
        }

        internal override void UpdateRaycastOrigins()
        {
            var bounds = _collider.bounds;
            bounds.Expand(SKINWIDTH * -2);

            RayCastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            RayCastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
            RayCastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
            RayCastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);

            RayCastOrigins.Center = bounds.center;
        }



        #region structs
        public struct CollisionInfo
        {
            public bool Above, Below, Left, Right;

            public int CollisionSum() => (Above ? 1 : 0) + (Below ? 1 : 0) + (Left ? 1 : 0) + (Right ? 1 : 0);
            public void Reset()
            {
                Above = Below = Left = Right = default;
            }

            public bool IsColliding => Above || Below || Left || Right;
            
            public override string ToString()
            {
                return $"{nameof(Above)}: {Above}, {nameof(Below)}: {Below}, {nameof(Left)}: {Left}, {nameof(Right)}: {Right}";
            }
        }

        public struct RaycastOrigins
        {
            public Vector2 TopLeft, TopRight, BottomLeft, BottomRight, Center;
        }
        
        #endregion
    }
}
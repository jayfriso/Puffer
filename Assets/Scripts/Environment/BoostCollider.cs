﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class BoostCollider : MonoBehaviour
    {
        /******* Variables & Properties*******/

        [SerializeField] private MaxVelocityBoost _maxVelocityBoost;
        [SerializeField] private Vector3 _boostForce;

        /******* Monobehavior Methods *******/

        public void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision.collider);
        }

        public void OnTriggerEnter(Collider other)
        {
            HandleCollision(other);
        }

        /******* Methods *******/

        private void HandleCollision(Collider other)
        {
            if (!other.CompareTag(PuffConstants.TAG_BALL)) return;

            BallTouchController ball = PuffSingletonManager.gameManager.ballController;
            ball.rigidBody.AddForce(_boostForce, ForceMode.Impulse);
            ball.maxVelocityBoosts.Add(_maxVelocityBoost.Copy());
        }
    }
}

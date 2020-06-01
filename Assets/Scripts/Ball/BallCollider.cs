using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class BallCollider : AbstractBallController
    {
        public bool isCollidingWithFloor { private set; get; }

        private int _floorLayer;

        /******* Variables & Properties*******/
        public Ball ball { get => _ball; }

        /******* Monobehavior Methods *******/

        public void Awake()
        {
            _floorLayer = LayerMask.NameToLayer(PuffConstants.LAYER_FLOOR);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == _floorLayer)
                isCollidingWithFloor = true;
        }

        public void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.layer == _floorLayer)
                isCollidingWithFloor = true;
        }

        public void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == _floorLayer)
                isCollidingWithFloor = false;
        }

        /******* Methods *******/
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class LevelPart : MonoBehaviour
    {
        /******* Variables & Properties*******/
        [SerializeField] private Transform _endPositionTransform;
        public Vector3 endPosition { get { return _endPositionTransform.position; } }
        public Vector3 startPosition { get { return transform.position; } }

        [SerializeField] private CheckPointCollider _checkPointCollider;
        public CheckPointCollider checkPointCollider { get { return _checkPointCollider; } }

        /******* Monobehavior Methods *******/

        /******* Methods *******/
    }
}

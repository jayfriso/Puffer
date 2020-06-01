using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class Island : MonoBehaviour
    {
        /******* Events *******/

        /******* Variables & Properties*******/
        [SerializeField] private Transform _startPosTransform;
        public Vector3 startPos => _startPosTransform.position;
        [SerializeField] private Transform _endPosTransform;
        public Vector3 endPos => _endPosTransform.position;

        [SerializeField] private float _surroundingDetectionSphereCastRadius;
        public bool isThereACollidingIsland 
        {
            get
            {
                int layerMask = LayerMask.GetMask(PuffConstants.LAYER_FLOOR);
                Collider[] colliders = Physics.OverlapSphere(transform.position, _surroundingDetectionSphereCastRadius, layerMask, QueryTriggerInteraction.Ignore);
                return colliders.Length > 1; // If more than 1 collider, then there is another overlapping collider
            }
        }

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        /// <summary>
        /// Move the island where the start position will match the given position
        /// </summary>
        /// <param name="positionToSetStartPos"></param>
        public void PositionStartPos(Vector3 positionToSetStartPos)
        {
            Vector3 diffInPosition = positionToSetStartPos - startPos;
            transform.position = transform.position + diffInPosition;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class CameraBehavior : MonoBehaviour
    {
        /******* Variables & Properties*******/
        [SerializeField]
        private GameObject _ballObject;

        private Vector3 _lastBallPosition;

        /******* Monobehavior Methods *******/
        private void Awake()
        {
            _lastBallPosition = _ballObject.transform.localPosition;
        }

        private void FixedUpdate()
        {
            if (_lastBallPosition != null)
            {
                Vector3 ballPosDiff = _ballObject.transform.localPosition - _lastBallPosition;
                transform.localPosition += ballPosDiff;
            }
            _lastBallPosition = _ballObject.transform.localPosition;
        }

        /******* Methods *******/
    }
}

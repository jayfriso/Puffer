using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

namespace JFrisoGames.PuffMan
{
    public class BallTouchInput : MonoBehaviour, IRollInput
    {
        /******* Events *******/

        public event OnRollInput onRollInput;

        /******* Variables & Properties*******/

        [Header("Roll Settings")]
        [SerializeField] private float _minSwipeDistance = 0.1f;

        private SwipeGestureRecognizer _swipeGesture;


        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public void InitRollInput()
        {
            _swipeGesture = new SwipeGestureRecognizer();
            _swipeGesture.MinimumDistanceUnits = _minSwipeDistance;
            _swipeGesture.StateUpdated += HandleSwipe;
            FingersScript.Instance.AddGesture(_swipeGesture);
        }

        private void HandleSwipe(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                Vector3 swipeDirection = (new Vector3(gesture.FocusX - gesture.StartFocusX, 0, gesture.FocusY - gesture.StartFocusY)).normalized;

                onRollInput?.Invoke(swipeDirection);
            }
        }
    }
}

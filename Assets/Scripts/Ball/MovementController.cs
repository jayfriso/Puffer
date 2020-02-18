using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class MovementController : AbstractBallController
    {
        /******* Variables & Properties*******/
        [Header("Movement Variables")]
        [SerializeField] private Vector3 _movementSpeed;
        [SerializeField] private Vector3 _constantForwardForce; 

        private IMovementInput _movementInput;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public override void Init(Ball ball)
        {
            base.Init(ball);

            _movementInput = GetComponent<IMovementInput>();
            _movementInput.InitMovementInput();
            _movementInput.onMovementInput += HandleMovementInput;
        }

        public override void ExecuteFixedUpdate()
        {
            base.ExecuteFixedUpdate();
            rigidBody.AddForce(_constantForwardForce, ForceMode.Force);
        }

        private void HandleMovementInput(float movementDelta)
        {
            rigidBody.AddForce(_movementSpeed * movementDelta, ForceMode.Force);
        }
    }
}

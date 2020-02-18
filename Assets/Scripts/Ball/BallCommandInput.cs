using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    /// <summary>
    /// TODO : THIS CLASS IS NOT WORKING ANYMORE AND IS DEPRECATED
    /// </summary>
    #region DEPRECATED
    public enum BallCommand
    {
        // Movement Commands
        Forward,
        Backward,
        Left,
        Right,
        ForwardRight,
        ForwardLeft,
        BackRight,
        BackLeft
    }

    public class BallCommandInput : MonoBehaviour, IMovementInput
    {
        /******* Events *******/

        public event OnMovementInput onMovementInput;

        /******* Variables & Properties*******/

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public void TriggerCommand(BallCommand command)
        {
            switch (command)
            {
                case BallCommand.Forward:
                case BallCommand.ForwardLeft:
                case BallCommand.ForwardRight:
                case BallCommand.Backward:
                case BallCommand.BackLeft:
                case BallCommand.BackRight:
                case BallCommand.Left:
                case BallCommand.Right:
                    TriggerMovement(command);
                    break;
            }
        }

        public void TriggerMovement(BallCommand command)
        {
            Vector3 direction = Vector3.zero;
            switch (command)
            {
                case BallCommand.Forward:
                    direction = new Vector3(0, 0, 1);
                    break;
                case BallCommand.ForwardLeft:
                    direction = new Vector3(-1, 0, 1);
                    break;
                case BallCommand.ForwardRight:
                    direction = new Vector3(1, 0, 1);
                    break;
                case BallCommand.Backward:
                    direction = new Vector3(0, 0, -1);
                    break;
                case BallCommand.BackLeft:
                    direction = new Vector3(-1, 0, -1);
                    break;
                case BallCommand.BackRight:
                    direction = new Vector3(1, 0, -1);
                    break;
                case BallCommand.Left:
                    direction = new Vector3(-1, 0, 0);
                    break;
                case BallCommand.Right:
                    direction = new Vector3(1, 0, 0);
                    break;
            }
            onMovementInput?.Invoke(direction.x);
        }
        public void InitMovementInput() { }
    }
    #endregion

}

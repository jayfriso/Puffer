using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public delegate void OnMovementInput(float movementDelta);

    public interface IMovementInput
    {
        event OnMovementInput onMovementInput;

        void InitMovementInput();
    }
}

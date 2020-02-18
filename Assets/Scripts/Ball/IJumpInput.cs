using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public delegate void OnJumpInput(float jumpAmount); //Jump amount from 0 to 1. 

    public interface IJumpInput
    {
        void InitJumpInput();

        event OnJumpInput onJumpInput;
    }
}

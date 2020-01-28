using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public delegate void OnRollInput(Vector3 direction);

    public interface IRollInput
    {
        event OnRollInput onRollInput;

        void InitRollInput();
    }
}

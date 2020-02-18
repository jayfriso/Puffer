using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public delegate void OnFlightStartInput();
    public delegate void OnFlightEndInput();

    public interface IFlightInput
    {
        void InitFlightInput();

        event OnFlightStartInput onFlightStartInput;
        event OnFlightEndInput onFlightEndInput;
    }

    public delegate void OnFlightVerticalInput(float verticalDelta);

    public interface IFlightVerticalInput
    {
        void InitFlightVerticalInput();

        event OnFlightVerticalInput onFlightVerticalInput;
    }
}

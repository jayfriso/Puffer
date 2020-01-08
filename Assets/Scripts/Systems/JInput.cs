using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames
{
    public class JInput : MonoBehaviour
    {    
        /******* Variables & Properties*******/
        
        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public static Touch[] GetTouches()
        {
#if UNITY_EDITOR
            Touch simulatedTouch = new Touch();
            if (Input.GetMouseButtonDown(0))
                simulatedTouch.phase = TouchPhase.Began;
            else if (Input.GetMouseButton(0))
                simulatedTouch.phase = TouchPhase.Moved;
            else if (Input.GetMouseButtonUp(0))
                simulatedTouch.phase = TouchPhase.Ended;
            else
                return new Touch[] { };

            simulatedTouch.position = Input.mousePosition;
            return new Touch[] { simulatedTouch };
#else
            return Input.touches;
#endif
        }
    }
}

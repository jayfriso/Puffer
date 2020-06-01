using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JFrisoGames.PuffMan
{
    [System.Serializable]
    public struct StaminaColor
    {
        [SerializeField] private Color _color;
        public Color color => _color;

        [SerializeField] private float minStaminaValue;
        [SerializeField] private float maxStaminaValue;

        public bool IsStaminaColorApplicable(float staminaValue)
        {
            return staminaValue >= minStaminaValue && staminaValue < maxStaminaValue;
        }
    }

    public class StaminaCircleElement : MonoBehaviour
    {
        /******* Events *******/

        /******* Variables & Properties*******/
        [Header("Elements")]
        [SerializeField] private Image _fillImage;

        [Header("Colors")]
        [SerializeField] private List<StaminaColor> staminaColors;


        /******* Monobehavior Methods *******/

        /******* Methods *******/
        
        public void HandleStaminaValueChanged(float newStamina)
        {
            _fillImage.fillAmount = newStamina;
            foreach (var staminaColor in staminaColors)
            {
                if (staminaColor.IsStaminaColorApplicable(newStamina))
                {
                    _fillImage.color = staminaColor.color;
                    break;
                }
            }
        }
    }
}

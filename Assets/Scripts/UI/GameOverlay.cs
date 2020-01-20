using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JFrisoGames.Engine;
using UnityEngine.UI;
using TMPro;

namespace JFrisoGames.PuffMan
{
    [System.Serializable]
    public struct VelocityTextMilestone
    {
        public int minVelocity;
        public Color textColor;
        public int textSize;
    }

    public class GameOverlay : AbstractScreen
    {
        /******* Variables & Properties*******/
        [SerializeField] private TextMeshProUGUI _tmpVelocity;
        [SerializeField] private List<VelocityTextMilestone> _velocityTextMilestones;
        [SerializeField] private RectTransform _ballPositionElement;

        private Vector3 _startPosition;
        private float _totalLevelDistance;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public void Init(Vector3 startPosition, float totalLevelDistance)
        {
            _startPosition = startPosition;
            _totalLevelDistance = totalLevelDistance;
        }

        public void HandleBallVelocityChange(int newVelocity)
        {
            VelocityTextMilestone velocityTextMilestone = _velocityTextMilestones[0];
            for (int i = _velocityTextMilestones.Count - 1; i >= 0; i--)
            {
                if (newVelocity > _velocityTextMilestones[i].minVelocity)
                {
                    velocityTextMilestone = _velocityTextMilestones[i];
                    break;
                }
            }
            
            _tmpVelocity.text = newVelocity.ToString();
            _tmpVelocity.fontSize = velocityTextMilestone.textSize;
            _tmpVelocity.color = velocityTextMilestone.textColor;
        }

        public void HandleBallPositionChange(Vector3 newPosition)
        {
            float distanceTraveled = (newPosition - _startPosition).z;
            float percentageTraveled = distanceTraveled / _totalLevelDistance;
            SetBallPositionHud(percentageTraveled);
        }

        private void SetBallPositionHud(float amount)
        {
            _ballPositionElement.anchorMin = new Vector2(amount, 0.5f);
            _ballPositionElement.anchorMax = new Vector2(amount, 0.5f);
            _ballPositionElement.anchoredPosition = Vector2.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class GameManager : MonoBehaviour
    {
        /******* Variables & Properties*******/

        [Header("Game Components")]
        [SerializeField] private LevelSpawnSystem _levelSpawnSystem;
        [SerializeField] private BallTouchController _ballController;
        public BallTouchController ballController { get { return _ballController; } }

        [Header("Start Level Properties")]
        [SerializeField] private Vector3 _levelStartPosition;
        [SerializeField] private Vector3 _ballSpawnDiffFromGround;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public void Init()
        {
            _ballController.Init();

            ClearGame();
            StartGame();
            
            GameOverlay gameOverlay = PuffSingletonManager.uiManager.CreateScreen<GameOverlay>(PuffScreenID.GameOverlay, Engine.ScreenCreateBehavior.ShowOnCreate);
            gameOverlay.Init(_levelSpawnSystem.currentLevelPart.startPosition, _levelSpawnSystem.totalLevelDistance);
            _ballController.onBallVelocityChange += gameOverlay.HandleBallVelocityChange;
            _ballController.onBallPositionChange += gameOverlay.HandleBallPositionChange;
        }
        
        private void ClearGame()
        {
            _levelSpawnSystem.ClearLevel();
        }
        private void StartGame()
        {
            _levelSpawnSystem.CreateLevel(_levelStartPosition);
            SetBallToCheckPoint();
        }

        public void EndGame()
        {
            ClearGame();
            StartGame();
        }

        public void SetBallToCheckPoint()
        {
            _ballController.Reset(_levelSpawnSystem.currentLevelPart.startPosition + _ballSpawnDiffFromGround);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class GameManager : MonoBehaviour
    {
        /******* Variables & Properties*******/

        [Header("Game Components")]
        [SerializeField] private Ball _ballController;
        public Ball ballController { get { return _ballController; } }

        [SerializeField] private CameraBehavior _cameraController;

        [Header("Start Level Properties")]
        [SerializeField] private Vector3 _levelStartPosition;
        [SerializeField] private Vector3 _ballSpawnDiffFromGround;


        [Header("Level Testing")]
        [SerializeField] private bool _usePartLevel = false;

        [Header("Part Levels")]
        [SerializeField] private PartLevelFactory _partLevelFactory;
        [SerializeField] private PartLevelData _partLevelData;

        [Header("Island Levels")]
        [SerializeField] private IslandLevelFactory _islandLevelFactory;
        [SerializeField] private IslandLevelData _islandLevelData;

        private ILevelFactory _levelFactory;
        private ILevel _currentLevel;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public void Init()
        {
            _ballController.Init();
            _levelFactory = _usePartLevel ? (ILevelFactory)_partLevelFactory: (ILevelFactory)_islandLevelFactory;
            _cameraController.Init(_ballController.ballInfo);
           
            ClearGame();
            StartGame();

            GameOverlay gameOverlay = PuffSingletonManager.uiManager.CreateScreen<GameOverlay>(PuffScreenID.GameOverlay, Engine.ScreenCreateBehavior.ShowOnCreate);
            gameOverlay.Init(_currentLevel.startPosition, _currentLevel.totalLevelDistance);
            _ballController.ballInfo.onBallVelocityChange += gameOverlay.HandleBallVelocityChange;

            //_ballController.ballInfo.onBallPositionChange += gameOverlay.HandleBallPositionChange; // disabling the ball position hud element for now

            _ballController.ballInfo.onBallVelocityChange += _cameraController.HandleBallVelocityChanged;
        }
        
        private void ClearGame()
        {
            if (_currentLevel != null)
                _currentLevel.Clear();
        }
        private void StartGame()
        {
            ILevelData levelData = _usePartLevel ? (ILevelData)_partLevelData : (ILevelData)_islandLevelData;
            _currentLevel = _levelFactory.CreateLevel(levelData, _levelStartPosition);
            SetBallToCheckPoint();
        }

        public void EndGame()
        {
            ClearGame();
            StartGame();
        }

        public void SetBallToCheckPoint()
        {
            _ballController.Reset(_currentLevel.startPosition + _ballSpawnDiffFromGround);

        }
    }
}

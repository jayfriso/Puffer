using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace JFrisoGames.PuffMan
{
    public class PartLevel : MonoBehaviour, ILevel
    {
        /******* Events *******/

        /******* Variables & Properties*******/

        private List<LevelPart> _spawnedLevelParts = new List<LevelPart>();
        private LevelPart _lastSpawnedPart { get { return _spawnedLevelParts.Count > 0 ? _spawnedLevelParts.Last() : null; } }

        private int _currentLevelPartIndex;
        public LevelPart currentLevelPart { get { return _spawnedLevelParts[_currentLevelPartIndex]; } }

        public float totalLevelDistance { get { return _spawnedLevelParts.Count > 0 ? (_spawnedLevelParts.Last().endPosition - _spawnedLevelParts.First().startPosition).z : 0f; } }

        public GameObject gameobject => this.gameObject;

        public Vector3 startPosition => _spawnedLevelParts[0].startPosition;

        private PartLevelData _levelData;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public void CreateLevel(PartLevelData partLevelData, Vector3 startPosition)
        {
            _levelData = partLevelData;

            for (int i = 0; i < _levelData.levelParts.Count; i++)
            {
                Vector3 positionToSpawnAt = _lastSpawnedPart == null ? startPosition : _lastSpawnedPart.endPosition;
                LevelPart newPart = _levelData.levelParts[i].InstantiateLevelPart(transform, positionToSpawnAt);
                newPart.checkPointCollider.onCheckPointPasssed += HandleCheckPointPassed;

                _spawnedLevelParts.Add(newPart);
            }
        }

        public void HandleCheckPointPassed()
        {
            if (_currentLevelPartIndex < _spawnedLevelParts.Count - 1)
                _currentLevelPartIndex++;
        }

        public void Clear()
        {
            for (int i = 0; i < _spawnedLevelParts.Count; i++)
            {
                LevelPart levelPart = _spawnedLevelParts[i];
                Destroy(levelPart.gameObject);
                levelPart.checkPointCollider.onCheckPointPasssed -= HandleCheckPointPassed;
            }
            _spawnedLevelParts.Clear();
            _currentLevelPartIndex = 0;
        }
    }
}

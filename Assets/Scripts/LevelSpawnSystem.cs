using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace JFrisoGames.PuffMan
{
    public class LevelSpawnSystem : MonoBehaviour
    {
        /******* Variables & Properties*******/
        [SerializeField] private Transform _partParent;
        [SerializeField] private List<GameObject> _levelPartPrefabs;
        [SerializeField] private int _numberOfPartsToSpawn;

        private List<LevelPart> _spawnedLevelParts = new List<LevelPart>();
        private LevelPart _lastSpawnedPart { get { return _spawnedLevelParts.Count > 0 ? _spawnedLevelParts.Last() : null; } }

        private int _currentLevelPartIndex;
        public LevelPart currentLevelPart { get { return _spawnedLevelParts[_currentLevelPartIndex]; } }

        public float totalLevelDistance { get { return _spawnedLevelParts.Count > 0 ? (_spawnedLevelParts.Last().endPosition - _spawnedLevelParts.First().startPosition).z : 0f; } }

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public void CreateLevel(Vector3 startPosition)
        {
            SpawnLevelParts(startPosition);
        }

        public void ClearLevel()
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

        public void SpawnLevelParts(Vector3 startPosition)
        {
            for (int i = 0; i < _numberOfPartsToSpawn; i++)
            {
                Vector3 positionToSpawnAt = _lastSpawnedPart == null ? startPosition : _lastSpawnedPart.endPosition;
                LevelPart newPart = Instantiate(_levelPartPrefabs.RandomElement(), _partParent).GetComponent<LevelPart>();
                newPart.transform.position = positionToSpawnAt;
                newPart.checkPointCollider.onCheckPointPasssed += HandleCheckPointPassed;
                _spawnedLevelParts.Add(newPart);
            }
        }

        public void HandleCheckPointPassed()
        {
            if (_currentLevelPartIndex < _spawnedLevelParts.Count - 1)
                _currentLevelPartIndex++;
        }
    }
}

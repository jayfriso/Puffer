using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class LevelPart : MonoBehaviour
    {
        /******* Variables & Properties*******/
        [SerializeField] private Transform _endPositionTransform;
        public Vector3 endPosition { get { return _endPositionTransform.position; } }
        public Vector3 startPosition { get { return transform.position; } }

        [SerializeField] private CheckPointCollider _checkPointCollider;
        public CheckPointCollider checkPointCollider { get { return _checkPointCollider; } }

        [SerializeField] private GameObject _componentsObject;

        [SerializeField] private bool _isMirrored;
        public bool isMirrored { get => _isMirrored; }

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public LevelPart InstantiateLevelPart(Transform parentTransform, Vector3 posToInstantiate)
        {
            LevelPart levelPartCopy = Instantiate(gameObject, parentTransform).GetComponent<LevelPart>();
            levelPartCopy.transform.position = posToInstantiate;
            if (isMirrored)
            {
                GameObject mirroredComponents = Instantiate(_componentsObject, levelPartCopy.gameObject.transform);
                mirroredComponents.transform.localScale = new Vector3(-1, 1, 1);
            }
            return levelPartCopy;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace JFrisoGames.PuffMan
{
    public class PartLevelFactory : MonoBehaviour, ILevelFactory
    {
        /******* Variables & Properties*******/
        [SerializeField] private GameObject _partLevelObjectRef;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public ILevel CreateLevel(ILevelData levelData, Vector3 startPosition)
        {
            PartLevel newPartLevel = Instantiate(_partLevelObjectRef).GetComponent<PartLevel>();
            newPartLevel.gameobject.SetActive(true);
            newPartLevel.transform.parent = transform;
            newPartLevel.CreateLevel(levelData as PartLevelData, startPosition);
            return newPartLevel;
        }

    }
}

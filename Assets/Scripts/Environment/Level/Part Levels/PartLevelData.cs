using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    [CreateAssetMenu(menuName = "PuffMan/Data/PartLevelData")]
    public class PartLevelData: ScriptableObject, ILevelData
    {
        /******* Variables & Properties*******/
        [Header("Level Parts")]
        public List<LevelPart> levelParts;
        
        /******* Monobehavior Methods *******/

        /******* Methods *******/
    }
}

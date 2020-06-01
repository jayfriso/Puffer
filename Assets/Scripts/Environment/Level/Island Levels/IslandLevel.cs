using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JFrisoGames.Engine;

namespace JFrisoGames.PuffMan
{
    public class IslandLevel : MonoBehaviour, ILevel
    {
        /******* Events *******/

        /******* Variables & Properties*******/
        public List<Island> islands { get; private set; }

        public GameObject gameobject => this.gameObject;

        public Vector3 startPosition => islands[0].startPos;
        public float totalLevelDistance => (islands[0].startPos - islands[islands.Count - 1].endPos).magnitude;

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public void Init()
        {
            islands = new List<Island>();
        }
    }
}

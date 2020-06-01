using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JFrisoGames.Engine;

namespace JFrisoGames.PuffMan
{
    public class IslandLevelFactory : MonoBehaviour, ILevelFactory
    {
        /******* Events *******/

        /******* Variables & Properties*******/
        [SerializeField] private GameObject _islandLevelObjRef;

        public ILevel CreateLevel(ILevelData levelData, Vector3 startPos)
        {
            IslandLevel newIslandLevel = Instantiate(_islandLevelObjRef, transform).GetComponent<IslandLevel>();
            newIslandLevel.gameObject.SetActive(true);
            newIslandLevel.Init();

            SeededRandom random = new SeededRandom();

            IslandLevelData islandLevelData = levelData as IslandLevelData;

            Island firstIsland = Instantiate(islandLevelData.firstIsland, newIslandLevel.transform);
            firstIsland.PositionStartPos(startPos);
            newIslandLevel.islands.Add(firstIsland);

            for (int r = 0; r < islandLevelData.islandsPerHop; r++)
            {

                Island previousIsland = firstIsland;

                for (int i = 0; i < islandLevelData.islandHopCount; i++)
                {
                    Island newIsland = Instantiate(random.ChooseRandom<Island>(islandLevelData.possibleIslands), newIslandLevel.transform);

                    bool isColliding = true;
                    while(isColliding)
                    {
                        Vector3 position = previousIsland.endPos + islandLevelData.GetRandomDistance();
                        newIsland.PositionStartPos(position);
                        isColliding = newIsland.isThereACollidingIsland;
                    }

                    newIslandLevel.islands.Add(newIsland);
                    previousIsland = newIsland;
                }
            }

            newIslandLevel.transform.localRotation = Quaternion.Euler(islandLevelData.levelRotation, 0, 0);
            return newIslandLevel;
        }

        /******* Monobehavior Methods *******/

        /******* Methods *******/
    }
}

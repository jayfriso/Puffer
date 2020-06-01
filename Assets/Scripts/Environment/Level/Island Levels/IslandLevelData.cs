using System.Collections.Generic;
using UnityEngine;
using JFrisoGames.Engine;

namespace JFrisoGames.PuffMan
{
    [CreateAssetMenu(menuName = "PuffMan/Data/IslandLevelData")]
    public class IslandLevelData : ScriptableObject, ILevelData
    {
        /******* Events *******/

        /******* Variables & Properties*******/
        [SerializeField] private Island _firstIsland;
        public Island firstIsland => _firstIsland;
        [SerializeField] private List<Island> _possibleIslands;
        [SerializeField] private int _islandHopCount;
        [SerializeField] private int _islandsPerHop;
        public int islandHopCount => _islandHopCount;
        public int islandsPerHop => _islandsPerHop;

        public List<Island> possibleIslands => _possibleIslands;

        [SerializeField] private Vector3 _minYZDistanceBetweenIslands;
        [SerializeField] private Vector3 _maxYZDistanceBetweenIslands;

        [SerializeField] private float _maxXDistance; // The x distance to set from the current island
        [SerializeField] private float _maxYCompensation; // The y distance to subtract, the more x distance is set

        [SerializeField] private float _levelRotation; // The amount to tilt the entire level by
        public float levelRotation => _levelRotation;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public Vector3 GetRandomDistance ()
        {
            SeededRandom random = new SeededRandom(Random.Range(0, 1000));
            float yzTValue = random.RandFloat();
            // Start by getting the yz values
            Vector3 result = Vector3.Lerp(_minYZDistanceBetweenIslands, _maxYZDistanceBetweenIslands, yzTValue);
            // Generate the random variables for the x calculation
            float xTValue = random.RandFloat();
            bool isNegative = random.Bool();
            // The x value and the y compensation is based off the same tvalue
            float xValue = Mathf.Lerp(0f, _maxXDistance, xTValue) * (isNegative ? -1 : 1);
            float yCompensation = Mathf.Lerp(0f, _maxYCompensation, xTValue);
            result = result + new Vector3(xValue, yCompensation, 0);
            return result;
        }
    }
}

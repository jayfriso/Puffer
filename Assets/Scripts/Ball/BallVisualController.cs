using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public enum BallVisualState
    {
        Expanded = 0,
        Compressed = 1
    }

    public class BallVisualController : MonoBehaviour
    {
        /******* Variables & Properties*******/
        [SerializeField] private Material _ballExpandedMaterial;
        [SerializeField] private Material _ballCompressedMaterial;
        [SerializeField] private int _materialToSet;

        private MeshRenderer _meshRenderer;

        private BallVisualState _ballVisualState;

        private Material[] _ballMaterials;
        public BallVisualState ballVisualState 
        { 
            get { return _ballVisualState; }
            set
            {
                _ballVisualState = value;
                switch (value)
                {
                    case BallVisualState.Expanded:
                        _ballMaterials[2] = _ballExpandedMaterial;
                        break;
                    case BallVisualState.Compressed:
                        _ballMaterials[2] = _ballCompressedMaterial;
                        _meshRenderer.materials[_materialToSet] = _ballCompressedMaterial;
                        break;
                }
                _meshRenderer.materials = _ballMaterials;
            }
        }

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public void Init()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _ballMaterials = _meshRenderer.materials;
            ballVisualState = BallVisualState.Expanded;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public enum BallVisualState
    {
        Regular = 0,
        Flying = 1
    }

    public class BallVisualController : AbstractBallController
    {
        /******* Variables & Properties*******/
        [SerializeField] private ParticleSystem _particleSystem;
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
                var emissionModule = _particleSystem.emission;
                switch (value)
                {
                    case BallVisualState.Regular:
                        _ballMaterials[2] = _ballExpandedMaterial;
                        emissionModule.enabled = false;
                        break;
                    case BallVisualState.Flying:
                        _ballMaterials[2] = _ballCompressedMaterial;
                        _meshRenderer.materials[_materialToSet] = _ballCompressedMaterial;
                        emissionModule.enabled = true;
                        break;
                }
                _meshRenderer.materials = _ballMaterials;
            }
        }

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public override void Init(Ball ball)
        {
            base.Init(ball);

            _meshRenderer = GetComponent<MeshRenderer>();
            _ballMaterials = _meshRenderer.materials;
            ballVisualState = BallVisualState.Regular;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class SpringPlatformBehavior : MonoBehaviour
    {
        /******* Variables & Properties*******/

        [SerializeField] private float _compressionTime;
        private float _currentTime = 0f;
        private float _currentCompressionAmount 
        { 
            get 
            {
                if (_isCompressing)
                    return _currentTime / _compressionTime;
                else if (_isReleasing)
                    return 1f - (_currentTime / _releaseTime);
                else
                    return 0f;
            } 
        }

        private Vector3 _startPosition;
        [SerializeField] private Vector3 _springCompressionDistance;
        private Vector3 _compressedPosition { get { return _startPosition + _springCompressionDistance; } }
        private Vector3 _currentCompressionPosition { get { return Vector3.Lerp(_startPosition, _compressedPosition, _currentCompressionAmount); } }

        [SerializeField] float _releaseTime;
        [SerializeField] Vector3 _releaseForce;

        private bool _isCompressing;
        private bool _isReleasing;

        /******* Monobehavior Methods *******/

        private void Awake()
        {
            _startPosition = transform.localPosition;
        }

        private void FixedUpdate()
        {
            if (_isCompressing)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    if (_currentTime < _compressionTime)
                        _currentTime += Time.fixedDeltaTime;
                }
                else
                {
                    _isReleasing = true;
                    _isCompressing = false;
                    _currentTime = 0f;
                }
            }
            else if (_isReleasing)
            {
                _currentTime += Time.fixedDeltaTime;
                if (_currentTime > _releaseTime)
                {
                    _isReleasing = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _currentTime = 0f;
                    _isCompressing = true;
                }
            }

            transform.localPosition = _currentCompressionPosition;
        }

        /******* Methods *******/
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class BallCommandSequence : MonoBehaviour
    {
        /******* Variables & Properties*******/
        [SerializeField] private float _timeBetweenCommands;
        [SerializeField] private List<BallCommand> _commands;

        private BallCommandInput _ballCommandInput;
        
        /******* Monobehavior Methods *******/

        private void Start()
        {
            _ballCommandInput = GetComponent<BallCommandInput>();
            StartCoroutine(BallCommandSequenceEnumerator());
        }

        /******* Methods *******/

        private IEnumerator BallCommandSequenceEnumerator()
        {
            for (int i = 0; i < _commands.Count; i++)
            {
                yield return new WaitForSeconds(_timeBetweenCommands);
                _ballCommandInput.TriggerCommand(_commands[i]);
            }
        }
    }
}

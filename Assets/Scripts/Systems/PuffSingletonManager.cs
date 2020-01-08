using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class PuffSingletonManager : MonoBehaviour
    {
        /******* Variables & Properties*******/

        protected static PuffSingletonManager instance { get; private set; }

        [SerializeField]
        protected GameManager _gameManager;
        public static GameManager gameManager { get => instance._gameManager; }

        [SerializeField]
        protected PuffUIManager _uiManager;
        public static PuffUIManager  uiManager { get => instance._uiManager; }

        /******* Monobehavior Methods *******/

        private void Start()
        {
            Init();
        }

        /******* Methods *******/

        public void Init()
        {
            if (instance == null)
                InitInstance();

            _uiManager.Init();
            _gameManager.Init();
        }

        private void InitInstance()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

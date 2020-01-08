using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JFrisoGames.Engine;
using System.Linq;

namespace JFrisoGames.PuffMan
{
    public enum PuffScreenID : int
    {
        GameOverlay
    }

    [System.Serializable]
    public struct PuffScreenPointer : IScreenPointer
    {
        [SerializeField] private PuffScreenID _screenId;
        public int screenId => (int)_screenId;

        [SerializeField] private string _resourcePath;
        public string resourcePath => _resourcePath;
    }

    public class PuffUIManager : AbstractUIManager
    {
        /******* Variables & Properties*******/
        [SerializeField] private List<PuffScreenPointer> _puffScreenPointers;
        private List<IScreenPointer> _screenPointers;
        protected override List<IScreenPointer> screenPointers
        {
            get
            {
                if (_screenPointers == null)
                {
                    _screenPointers = _puffScreenPointers.ConvertAll<IScreenPointer>((PuffScreenPointer puffPointer) => puffPointer as IScreenPointer);
                }
                return _screenPointers;
            }
        }

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public TScreenType CreateScreen<TScreenType>(PuffScreenID screenID, ScreenCreateBehavior createBehavior) where TScreenType : AbstractScreen
        {
            return CreateScreen<TScreenType>((int)screenID, createBehavior);
        }

        public TScreenType GetScreen<TScreenType>(PuffScreenID screenID) where TScreenType : AbstractScreen
        {
            return GetScreen<TScreenType>((int)screenID);
        }

        public void DestroyScreen(PuffScreenID screenID)
        {
            DestroyScreen((int)screenID);
        }

        public void HideScreen(PuffScreenID screenID)
        {
            HideScreen((int)screenID);
        }
    }
}

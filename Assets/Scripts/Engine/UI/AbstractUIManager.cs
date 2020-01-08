using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.Engine
{
    public enum ScreenCreateBehavior
    {
        ShowOnCreate,
        HideOnCreate
    }
    
    public interface IScreenPointer
    {
        int screenId { get; }
        string resourcePath { get; }
    }

    public abstract class AbstractUIManager : MonoBehaviour
    {
        /******* Variables & Properties*******/

        [SerializeField]
        private RectTransform _screenContainer;

        protected abstract List<IScreenPointer> screenPointers { get; }

        private Dictionary<int, string> _screenResourceDict;
        private Dictionary<int, AbstractScreen> _createdScreensDict;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public void Init()
        {
            _screenResourceDict = new Dictionary<int, string>();
            _createdScreensDict = new Dictionary<int, AbstractScreen>();
            for (int i = 0; i < screenPointers.Count; i++)
            {
                IScreenPointer screenPointer = screenPointers[i];
                _screenResourceDict.Add(screenPointer.screenId, screenPointer.resourcePath);
            }
        }

        public TScreenType GetScreen<TScreenType>(int screenID) where TScreenType : AbstractScreen
        {
            if (!_createdScreensDict.ContainsKey(screenID))
                Debug.LogErrorFormat("SCREEN ERROR : Screen {0} has been attempted to be gotten but has not been created yet.", screenID.ToString());
            return _createdScreensDict[screenID] as TScreenType;
        }

        public TScreenType CreateScreen<TScreenType>(int screenID, ScreenCreateBehavior createBehavior) where TScreenType : AbstractScreen
        {
            GameObject newScreenPrefab = Resources.Load(_screenResourceDict[screenID]) as GameObject;
            TScreenType newScreen = Instantiate(newScreenPrefab, _screenContainer.transform).GetComponent<TScreenType>();
            newScreen.OnCreate();
            _createdScreensDict.Add(screenID, newScreen);

            switch(createBehavior)
            {
                case ScreenCreateBehavior.ShowOnCreate:
                    ShowScreen(screenID);
                    break;
                case ScreenCreateBehavior.HideOnCreate:
                    HideScreen(screenID);
                    break;
            }

            return newScreen;
        }

        public void DestroyScreen(int screenID)
        {
            if (!_createdScreensDict.ContainsKey(screenID))
                Debug.LogErrorFormat("SCREEN ERROR : Screen {0} has been attempted to be destroyed but has not been created yet.", screenID.ToString());

            Destroy(_createdScreensDict[screenID].gameObject);
            _createdScreensDict.Remove(screenID);
        }

        public void ShowScreen(int screenID)
        {
            if (!_createdScreensDict.ContainsKey(screenID))
                Debug.LogErrorFormat("SCREEN ERROR : Screen {0} has been attempted to be shown but has not been created yet.", screenID.ToString());
            AbstractScreen screen = _createdScreensDict[screenID];
            screen.gameObject.SetActive(true);
            screen.OnShow();
        }

        public void HideScreen(int screenID)
        {
            if (!_createdScreensDict.ContainsKey(screenID))
                Debug.LogErrorFormat("SCREEN ERROR : Screen {0} has been attempted to be hidden but has not been created yet.", screenID.ToString());
            AbstractScreen screen = _createdScreensDict[screenID];
            screen.gameObject.SetActive(false);
            screen.OnHide();
        }
    }
}

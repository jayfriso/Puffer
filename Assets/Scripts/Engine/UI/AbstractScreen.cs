using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.Engine
{
    public abstract class AbstractScreen : MonoBehaviour
    {
        public virtual void OnCreate() { }
        public virtual void OnShow() { }
        public virtual void OnHide() { }
    }
}

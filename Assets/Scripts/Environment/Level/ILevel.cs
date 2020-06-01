using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public interface ILevel
    {
        GameObject gameobject { get; }
        void Clear();
        Vector3 startPosition { get; }
        float totalLevelDistance { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

// Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace GUPS.EasyParallelScene.Helper
{
    /// <summary>
    /// Helper class for Unity EventSystems.
    /// </summary>
    public static class EventSystemHelper
    {
        /// <summary>
        /// Returns all EventSystems in all loaded Scenes.
        /// </summary>
        /// <returns>A list of EventSystems in the loaded Scenes.</returns>
        public static List<EventSystem> GetAllEventSystems()
        {
            List<EventSystem> var_EventSystems = new List<EventSystem>();

            for(int s = 0; s < SceneManager.sceneCount; s++)
            {
                Scene var_Scene = SceneManager.GetSceneAt(s);
                
                if (var_Scene.isLoaded)
                {
                    var_EventSystems.AddRange(GetAllEventSystems(var_Scene));
                }
            }

            return var_EventSystems;
        }

        /// <summary>
        /// Returns all EventSystems in the given Scene.
        /// </summary>
        /// <param name="_Scene"></param>
        /// <returns>A list of EventSystems in the passed _Scene.</returns>
        public static List<EventSystem> GetAllEventSystems(Scene _Scene)
        {
            return _Scene.GetRootGameObjects().SelectMany((x) => x.GetComponentsInChildren<EventSystem>(true)).ToList();
        }
    }
}

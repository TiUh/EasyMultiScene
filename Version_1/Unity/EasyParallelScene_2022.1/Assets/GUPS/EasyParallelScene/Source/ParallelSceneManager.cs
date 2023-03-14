using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// GUPS
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene
{
    /// <summary>
    /// The ParallelSceneManager allows to load and run Scenes in parallel. This means, you can have multiple Scenes running independent from each other,
    /// while it is possible to switch between these Scenes. To do so, the Scene has to be registered as ParallelScene.
    /// Each ParallelScene can have its own settings, for example a custom time scale, applied only to the linked Scene.
    /// Also it is possible to instantiate GameObject in those Scenes.
    /// </summary>
    public class ParallelSceneManager : Singleton.PersistentSingleton<ParallelSceneManager>
    {
        /// <summary>
        /// Stores a list of available ParallelScenes. To use a ParallelScene in GameMode, add it over the Unity-Inspector or use the RegisterParallelScene(...) method.
        /// </summary>
        [SerializeReference]
        private List<ParallelScene> parallelScenes = new List<ParallelScene>();

        /// <summary>
        /// Register a ParallelScene while runtime, to make it available in GameMode.
        /// </summary>
        /// <param name="_ParallelScene">Pass a ParallelScene you want to register and make it available in game.</param>
        public void RegisterParallelScene(ParallelScene _ParallelScene)
        {
            // Check if the ParallelScene was already added.
            if (this.parallelScenes.Contains(_ParallelScene))
            {
                return;
            }

            // Else, add it to the list.
            this.parallelScenes.Add(_ParallelScene);
        }

        /// <summary>
        /// Returns true, if a _ParallelScene is registered.
        /// </summary>
        /// <param name="_ParallelScene">The ParallelScene to check.</param>
        /// <returns>Returns if the ParallelScene is registered.</returns>
        public bool ContainsParallelScene(ParallelScene _ParallelScene)
        {
            return this.ContainsParallelScene(_ParallelScene.ScenePath);
        }

        /// <summary>
        /// Returns true, if a ParallelScene for _Scene is registered.
        /// </summary>
        /// <param name="_Scene">The Scene to check.</param>
        /// <returns>Returns if the ParallelScene is registered.</returns>
        public bool ContainsParallelScene(Scene _Scene)
        {
            return this.ContainsParallelScene(_Scene.path);
        }

        /// <summary>
        /// Returns true, if a ParallelScene with _ScenePath is registered.
        /// </summary>
        /// <param name="_ScenePath">Pass a relative scene path 'Assets/[...]/MyUnityScene.unity' or just the scene name 'MyUnityScene'.</param>
        /// <returns>Returns if the ParallelScene is registered.</returns>
        public bool ContainsParallelScene(String _ScenePath)
        {
            return this.parallelScenes.FindAll(p => p.ScenePath == _ScenePath || System.IO.Path.GetFileNameWithoutExtension(p.ScenePath) == _ScenePath).Count > 0;
        }

        /// <summary>
        /// Returns a registered ParallelScene with _Scene. If there is no ParallelScene returns null.
        /// </summary>
        /// <param name="_Scene">Pass a Scene.</param>
        /// <returns>Returns a registered ParallelScene. If there is none, returns null.</returns>
        public ParallelScene FindParallelScene(Scene _Scene)
        {
            return this.FindParallelScene(_Scene.path);
        }

        /// <summary>
        /// Returns a registered ParallelScene with _ScenePath. If there is no ParallelScene returns null.
        /// </summary>
        /// <param name="_ScenePath">Pass a relative scene path 'Assets/[...]/MyUnityScene.unity' or just the scene name 'MyUnityScene'.</param>
        /// <returns>Returns a registered ParallelScene. If there is none, returns null.</returns>
        public ParallelScene FindParallelScene(String _ScenePath)
        {
            return this.parallelScenes.Find(p => p.ScenePath == _ScenePath || System.IO.Path.GetFileNameWithoutExtension(p.ScenePath) == _ScenePath);
        }

        /// <summary>
        /// Returns true, if the _ParallelScene is already loaded.
        /// </summary>
        /// <param name="_ParallelScene"></param>
        /// <returns></returns>
        public bool IsLoaded(ParallelScene _ParallelScene)
        {
            // Return if the Scene exists and is loaded.
            return this.IsLoaded(_ParallelScene.ScenePath);
        }

        /// <summary>
        /// Returns true, if the _Scene is already loaded.
        /// </summary>
        /// <param name="_ScenePath"></param>
        /// <returns></returns>
        public bool IsLoaded(Scene _Scene)
        {
            // Return if the Scene exists and is loaded.
            return this.IsLoaded(_Scene.path);
        }

        /// <summary>
        /// Returns true, if the Scene at _ScenePath is already loaded.
        /// </summary>
        /// <param name="_ScenePath"></param>
        /// <returns></returns>
        public bool IsLoaded(String _ScenePath)
        {
            // Return if the Scene exists and is loaded.
            return SceneManager.GetSceneByPath(_ScenePath) != null && SceneManager.GetSceneByPath(_ScenePath).isLoaded;
        }

        /// <summary>
        /// Load a _ParallelScene either Single or Additive (Parallel) to the current Scenes.
        /// Note that this function behaves the same as SceneManager.LoadSceneAsync meaning that the load does not happen immediately.
        /// This behavior also means that the Scene that is returned has its state set to Loading.
        /// </summary>
        /// <param name="_ParallelScene">The Scene to load.</param>
        /// <param name="_LoadSceneMode">The loading mode. Either Single or Additive (Parallel).</param>
        /// <param name="_LoadGameObjectsFlags">Decide how to load the GameObjects in the loading Scene.</param>
        /// <returns>Returns the loading Scene async operation.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public AsyncOperation Load(ParallelScene _ParallelScene, LoadSceneMode _LoadSceneMode, ELoadGameObjectFlag _LoadGameObjectsFlags)
        {
            // If there is no parallel Scene passed, throw an exception.
            if (_ParallelScene == null)
            {
                throw new ArgumentNullException("_ParallelScene");
            }

            // Check if it is Single or Additive loading.
            if (_LoadSceneMode == LoadSceneMode.Additive)
            {
                // Is Additive.

                // If the Scene is alread loading/loaded, throw an exception. A ParallelScene can only be loaded once!
                if (SceneManager.GetSceneByPath(_ParallelScene.ScenePath) != null && SceneManager.GetSceneByPath(_ParallelScene.ScenePath).isLoaded)
                {
                    throw new Exception("The Scene at path: " + _ParallelScene.ScenePath + " got already loaded!");
                }
            }

            // Aysnc loading operation.
            AsyncOperation var_LoadingOperation = null;

            // Load the Scene, either via the Editor or Runtime SceneManager. NOTE: A Scene can be loaded more than once, but ParallelSceneManager supports only loading Scenes once! So the same Scene cannot be run parallel.
#if UNITY_EDITOR
            if (_LoadSceneMode == LoadSceneMode.Single)
            {
                var_LoadingOperation = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(_ParallelScene.ScenePath, new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));
            }
            else
            {
                var_LoadingOperation = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(_ParallelScene.ScenePath, new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.None));

            }
#else
            if (_LoadSceneMode == LoadSceneMode.Single)
            {
                var_LoadingOperation = SceneManager.LoadSceneAsync(_ParallelScene.ScenePath, new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));
            }
            else
            {
                var_LoadingOperation = SceneManager.LoadSceneAsync(_ParallelScene.ScenePath, new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.None));
            }
#endif

            // Wait until Scene is loaded.
            var_LoadingOperation.completed += (_) =>
            {
                // The loaded Scene.
                Scene var_LoadedScene = SceneManager.GetSceneByPath(_ParallelScene.ScenePath);

                // Deactivate all Cameras of the loaded Scene.
                foreach (Camera var_Camera in CameraHelper.GetAllCameras(var_LoadedScene))
                {
                    var_Camera.enabled = false;
                }

                // Deactivate all AudioListeners of the loaded Scene.
                foreach (AudioListener var_AudioListener in AudioListenerHelper.GetAllAudioListeners(var_LoadedScene))
                {
                    var_AudioListener.enabled = false;
                }

                // Deactivate all EventSystems of the loaded Scene.
                foreach (EventSystem var_EventSystem in EventSystemHelper.GetAllEventSystems(var_LoadedScene))
                {
                    var_EventSystem.enabled = false;
                }

                // Mute all AudioSources of the loaded Scene.
                foreach (AudioSource var_AudioSource in AudioSourceHelper.GetAllAudioSources(var_LoadedScene))
                {
                    var_AudioSource.mute = true;
                }

                // Apply layer and merge to the active Scene.
                if (_LoadGameObjectsFlags.HasFlag(ELoadGameObjectFlag.APPLY_LAYER | ELoadGameObjectFlag.MERGE))
                {
                    // Get active Scene.
                    Scene var_ActiveScene = SceneManager.GetActiveScene();

                    // Get the ParallelScene for the active Scene.
                    ParallelScene var_ActiveParallelScene = this.FindParallelScene(var_ActiveScene.path);

                    // If there is no ParallelScene for the active scene, use as Layer the Default (0);
                    int var_Layer = 0;

                    if (var_ActiveParallelScene)
                    {
                        var_Layer = var_ActiveParallelScene.Layer;
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning("There is no ParallelScene registered at path: " + var_ActiveScene.path + ". Using the Layer Default (0) instead.");
                    }

                    // Apply layer.
                    this.ApplyLayerToScene(var_LoadedScene, var_Layer);

                    // Merge loaded Scene GameObjects to active Scene.
                    SceneManager.MergeScenes(var_LoadedScene, var_ActiveScene);
                }
                else if(_LoadGameObjectsFlags.HasFlag(ELoadGameObjectFlag.APPLY_LAYER))
                {
                    // Apply layer.
                    this.ApplyLayerToScene(var_LoadedScene, _ParallelScene.Layer);
                }
                else if (_LoadGameObjectsFlags.HasFlag(ELoadGameObjectFlag.MERGE))
                {
                    // Get active Scene.
                    Scene var_ActiveScene = SceneManager.GetActiveScene();

                    // Merge loaded Scene GameObjects to active Scene.
                    SceneManager.MergeScenes(var_LoadedScene, var_ActiveScene);
                }

                // If is a single Scene. Switch to the Camera and Audio.
                if (_LoadSceneMode == LoadSceneMode.Single)
                {
                    this.SwitchTo(_ParallelScene, ESwitchSceneFlag.DEFAULT, false);
                }
            };

            return var_LoadingOperation;
        }

        /// <summary>
        /// Unload the Scene using the default Unity SceneManager unload methods.
        /// </summary>
        /// <param name="_ParallelScene">The scene to unload.</param>
        /// <returns>Returns the unloading scene async operation.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public AsyncOperation Unload(ParallelScene _ParallelScene)
        {
            // If there is no parallel scene passed, throw an exception.
            if (_ParallelScene == null)
            {
                throw new ArgumentNullException("_ParallelScene");
            }

            // Aysnc loading operation.
            AsyncOperation var_LoadingOperation = null;

            // Unload the Scene using the default Unity SceneManager.
#if UNITY_EDITOR

            var_LoadingOperation = UnityEditor.SceneManagement.EditorSceneManager.UnloadSceneAsync(_ParallelScene.ScenePath);
#else
            
            var_LoadingOperation = SceneManager.UnloadSceneAsync(_ParallelScene.ScenePath);
#endif

            return var_LoadingOperation;
        }

        /// <summary>
        /// Apply _Layer to all GameObjects and their children, Cameras and Lights in _Scene.
        /// </summary>
        /// <param name="_Scene"></param>
        /// <param name="_Layer"></param>
        private void ApplyLayerToScene(Scene _Scene, int _Layer)
        {
            // Get root GameObjects of loaded scene.
            GameObject[] var_RootGameObjects = _Scene.GetRootGameObjects();

            // Apply layer to them and their children.
            this.ApplyLayerToGameObjectsAndChildren(var_RootGameObjects, _Layer);

            // Get all Cameras of the loaded Scene.
            Camera[] var_Cameras = CameraHelper.GetAllCameras(_Scene).ToArray();

            // Apply layer to the Cameras.
            this.ApplyLayerToCameras(var_Cameras, _Layer);

            // Get all Lights of the loaded Scene.
            Light[] var_Lights = LightHelper.GetAllLights(_Scene).ToArray();

            // Apply layer to the Lights.
            this.ApplyLayerToLights(var_Lights, _Layer);

            // Get all ReflectionProbes of the loaded Scene.
            ReflectionProbe[] var_ReflectionProbes = LightHelper.GetAllReflectionProbes(_Scene).ToArray();

            // Apply layer to the ReflectionProbess.
            this.ApplyLayerToReflectionProbes(var_ReflectionProbes, _Layer);
        }

        /// <summary>
        /// Apply _Layer to all _GameObjects and their children.
        /// </summary>
        /// <param name="_GameObjects"></param>
        /// <param name="_Layer"></param>
        private void ApplyLayerToGameObjectsAndChildren(GameObject[] _GameObjects, int _Layer)
        {
            // Iterate all passed.
            for (int g = 0; g < _GameObjects.Length; g++)
            {
                // If the passed GameObject is null, skip it.
                if (_GameObjects[g] == null)
                {
                    continue;
                }

                // Find all GameObjects, recursively search also through all children and their children.
                IEnumerable<GameObject> var_AllGameObjects = _GameObjects[g].GetComponentsInChildren(typeof(Transform), true).Select(c => c.gameObject);

                // Iterate them all and apply layer.
                foreach(GameObject var_GameObject in var_AllGameObjects)
                {
                    // Apply layer.
                    var_GameObject.layer = _Layer;
                }
            }
        }

        /// <summary>
        /// Apply _Layer to the rendering and event (mouse) triggers of all _Cameras.
        /// </summary>
        /// <param name="_Cameras"></param>
        /// <param name="_Layer"></param>
        private void ApplyLayerToCameras(Camera[] _Cameras, int _Layer)
        {
            // Iterate all passed.
            for (int c = 0; c < _Cameras.Length; c++)
            {
                // If the passed Camera is null, skip it.
                if (_Cameras[c] == null)
                {
                    continue;
                }

                // Find the Layer Name for the passed _Layer.
                String var_LayerName = LayerMask.LayerToName(_Layer);

                // Find the Layer Mask for the passed _Layer.
                int var_LayerMask = LayerMask.GetMask(var_LayerName);

                // Apply Layer Mask to the rendering and event (mouse) triggers.
                _Cameras[c].cullingMask = var_LayerMask;
                _Cameras[c].eventMask = var_LayerMask;
            }
        }

        /// <summary>
        /// Apply _Layer to the culling mask of all _Lights.
        /// </summary>
        /// <param name="_Lights"></param>
        /// <param name="_Layer"></param>
        private void ApplyLayerToLights(Light[] _Lights, int _Layer)
        {
            // Iterate all passed.
            for (int l = 0; l < _Lights.Length; l++)
            {
                // If the passed Light is null, skip it.
                if (_Lights[l] == null)
                {
                    continue;
                }

                // Find the Layer Name for the passed _Layer.
                String var_LayerName = LayerMask.LayerToName(_Layer);

                // Find the Layer Mask for the passed _Layer.
                int var_LayerMask = LayerMask.GetMask(var_LayerName);

                // Apply Layer Mask.
                _Lights[l].cullingMask = var_LayerMask;
            }
        }

        /// <summary>
        /// Apply _Layer to the culling mask of all _ReflectionProbes.
        /// </summary>
        /// <param name="_ReflectionProbes"></param>
        /// <param name="_Layer"></param>
        private void ApplyLayerToReflectionProbes(ReflectionProbe[] _ReflectionProbes, int _Layer)
        {
            // Iterate all passed.
            for (int l = 0; l < _ReflectionProbes.Length; l++)
            {
                // If the passed ReflectionProbe is null, skip it.
                if (_ReflectionProbes[l] == null)
                {
                    continue;
                }

                // Find the Layer Name for the passed _Layer.
                String var_LayerName = LayerMask.LayerToName(_Layer);

                // Find the Layer Mask for the passed _Layer.
                int var_LayerMask = LayerMask.GetMask(var_LayerName);

                // Apply Layer Mask.
                _ReflectionProbes[l].cullingMask = var_LayerMask;
            }
        }

        /// <summary>
        /// Stores in a mapping the Scene path to its ParallelTime.
        /// </summary>
        private Dictionary<String, ParallelTime> scenePathToParallelTime = new Dictionary<string, ParallelTime>();

        /// <summary>
        /// Return the ParallelTime for the passed _Scene.
        /// If the _Scene is not registered as ParallelScene, a default ParallelTime using 'Time.timeScale' is returned.
        /// </summary>
        /// <param name="_Scene">Pass a Scene you want the ParallelTime for.</param>
        /// <returns></returns>
        private ParallelTime GetParallelTime(Scene _Scene)
        {
            return this.GetParallelTime(_Scene.path);
        }

        /// <summary>
        /// Return the ParallelTime for the passed _ScenePath.
        /// If the Scene at _ScenePath is not registered as ParallelScene, a default ParallelTime using 'Time.timeScale' is returned.
        /// </summary>
        /// <param name="_ScenePath">Pass a relative Scene path 'Assets/[...]/MyUnityScene.unity' or just the Scene name 'MyUnityScene' you want the ParallelTime for.</param>
        /// <returns></returns>
        private ParallelTime GetParallelTime(String _ScenePath)
        {
            /* If the mapping does not contain the scene path, create a new ParallelTime and add it to the mapping.
             * If the ParallelScene is not registered yet, returns the Unity default TimeScale. */
            if (!this.scenePathToParallelTime.TryGetValue(_ScenePath, out ParallelTime var_ParallelTime))
            {
                // Find the ParallelScene with the passed scene path. Might be null.
                ParallelScene var_ParallelScene = this.FindParallelScene(_ScenePath);

                // If there is no ParallelScene, return a default ParallelTime using the Unity TimeScale.
                if (var_ParallelScene == null)
                {
                    // Log a warning!
                    UnityEngine.Debug.LogWarning("There is no registered ParallelScene at: " + _ScenePath + ". So the default 'Time.timeScale' will be returned.");

                    // Return a default ParallelTime using the Unity TimeScale.
                    return new ParallelTime(Time.timeScale, false);
                }

                // Get the ParallelTime
                var_ParallelTime = this.GetParallelTime(var_ParallelScene);
            }

            // Return the ParallelTime.
            return var_ParallelTime;
        }

        /// <summary>
        /// Return the ParallelTime for the passed _ParallelScene.
        /// </summary>
        /// <param name="_ParallelScene"></param>
        /// <returns></returns>
        private ParallelTime GetParallelTime(ParallelScene _ParallelScene)
        {
            // If there is no parallel scene passed, throw an exception.
            if (_ParallelScene == null)
            {
                throw new ArgumentNullException("_ParallelScene");
            }

            // If the mapping does not contain the scene path, create a new ParallelTime and add it to the mapping.
            if (!this.scenePathToParallelTime.TryGetValue(_ParallelScene.ScenePath, out ParallelTime var_ParallelTime))
            {
                // Create a new ParallelTime using the ParallelScene.
                var_ParallelTime = new ParallelTime(_ParallelScene.TimeScale, false);

                // Add the ParallelTime to the mapping.
                this.scenePathToParallelTime.Add(_ParallelScene.ScenePath, var_ParallelTime);
            }

            // Return the ParallelTime.
            return var_ParallelTime;
        }

        /// <summary>
        /// Switch to a loaded ParallelScene and set it as active Scene. If the scene is not loaded yet, returns false.
        /// </summary>
        /// <param name="_ScenePath">Pass a relative Scene path like 'Assets/[...]/MyUnityScene.unity' or just the scene name 'MyUnityScene' to switch to.</param>
        /// <param name="_SwitchSceneFlag">Manage the Scene switching.</param>
        /// <param name="_PauseCurrentScene">Pause the active scene.</param>
        /// <returns>Returns if the switching was successful.</returns>
        /// <exception cref="Exception"></exception>
        public bool SwitchTo(String _ScenePath, ESwitchSceneFlag _SwitchSceneFlag, bool _PauseCurrentScene)
        {
            // Get the ParallelScene by the _ScenePath.
            ParallelScene var_ParallelScene = this.FindParallelScene(_ScenePath);

            if(var_ParallelScene == null)
            {
                throw new Exception("There is no registered ParallelScene at: " + _ScenePath);
            }

            // Switch to.
            return this.SwitchTo(var_ParallelScene, _SwitchSceneFlag, _PauseCurrentScene);
        }

        /// <summary>
        /// Switch to a loaded ParallelScene and set it as active Scene. If the scene is not loaded yet, returns false.
        /// </summary>
        /// <param name="_ParallelScene">Pass a ParallelScene to switch to.</param>
        /// <param name="_SwitchSceneFlag">Manage the Scene switching.</param>
        /// <param name="_PauseCurrentScene">Pause the active scene.</param>
        /// <returns>Returns if the switching was successful.</returns>
        public bool SwitchTo(ParallelScene _ParallelScene, ESwitchSceneFlag _SwitchSceneFlag, bool _PauseCurrentScene)
        {
            // If there is no parallel scene passed, throw an exception.
            if (_ParallelScene == null)
            {
                throw new ArgumentNullException("_ParallelScene");
            }

            // Check if the scene got loaded.
            Scene var_SwitchToScene = SceneManager.GetSceneByPath(_ParallelScene.ScenePath);

            if(!var_SwitchToScene.isLoaded)
            {
                return false;
            }

            // Get active scene.
            Scene var_ActiveScene = SceneManager.GetActiveScene();

            // Pause the active Scene.
            if(_PauseCurrentScene)
            {
                this.SetIsPaused(var_ActiveScene, true);
            }

            // Set the switched to Scene as active.
            SceneManager.SetActiveScene(var_SwitchToScene);

            // Deactivate all Cameras and activate the Main Camera in the switched to Scene.
            if (_SwitchSceneFlag.HasFlag(ESwitchSceneFlag.ACTIVATE_MAIN_CAMERA))
            {
                // Deactivate all Cameras.
                List<Camera> var_AllCameras = CameraHelper.GetAllCameras();

                for (int c = 0; c < var_AllCameras.Count; c++)
                {
                    var_AllCameras[c].enabled = false;
                }

                // Activate main Camera in switched to Scene.
                Camera var_MainCamera = CameraHelper.GetMainCamera(var_SwitchToScene);

                if (var_MainCamera == null)
                {
                    throw new Exception("There is no main Camera in the scene you want to switch to: " + var_SwitchToScene.path);
                }
                else
                {
                    var_MainCamera.enabled = true;
                }
            }

            // Deactivate all AudioListeners and activate the Main AudioListener in the switched to Scene.
            if (_SwitchSceneFlag.HasFlag(ESwitchSceneFlag.ACTIVATE_MAIN_AUDIOLISTENER))
            {
                // Deactivate all AudioListeners.
                List<AudioListener> var_AllAudioListeners = AudioListenerHelper.GetAllAudioListeners();

                for (int c = 0; c < var_AllAudioListeners.Count; c++)
                {
                    var_AllAudioListeners[c].enabled = false;
                }

                // Activate AudioListeners in switched to Scene.
                AudioListener var_AudioListener = AudioListenerHelper.GetMainAudioListener(var_SwitchToScene);

                if (var_AudioListener == null)
                {
                    throw new Exception("There is no AudioListener in the scene you want to switch to: " + var_SwitchToScene.path);
                }
                else
                {
                    var_AudioListener.enabled = true;
                }
            }

            // Deactivate all EventSystems and activate the Main EventSystem in the switched to Scene.
            if (_SwitchSceneFlag.HasFlag(ESwitchSceneFlag.ACTIVATE_MAIN_EVENTSYSTEM))
            {
                // Deactivate all EventSystems.
                List<EventSystem> var_AllEventSystems = EventSystemHelper.GetAllEventSystems();

                for (int c = 0; c < var_AllEventSystems.Count; c++)
                {
                    var_AllEventSystems[c].enabled = false;
                }

                // Activate EventSystems in switched to Scene.
                EventSystem var_EventSystem = EventSystemHelper.GetAllEventSystems(var_SwitchToScene).FirstOrDefault();

                if (var_EventSystem == null)
                {
                    throw new Exception("There is no EventSystem in the scene you want to switch to: " + var_SwitchToScene.path);
                }
                else
                {
                    var_EventSystem.enabled = true;
                }
            }

            // Mute all AudioSources and umute those in the switched to Scene.
            if (_SwitchSceneFlag.HasFlag(ESwitchSceneFlag.ACTIVATE_AUDIOSOURCE))
            {
                // Mute all AudioSources.
                List<AudioSource> var_AllAudioSources = AudioSourceHelper.GetAllAudioSources();

                for (int a = 0; a < var_AllAudioSources.Count; a++)
                {
                    var_AllAudioSources[a].mute = true;
                }

                // Unmute the Scene AudioSources.
                List<AudioSource> var_SceneAudioSources = AudioSourceHelper.GetAllAudioSources(var_SwitchToScene);

                for (int a = 0; a < var_SceneAudioSources.Count; a++)
                {
                    var_SceneAudioSources[a].mute = false;
                }
            }

            // Unpause the switched Scene.
            this.SetIsPaused(var_SwitchToScene, false);

            return true;
        }

        /// <summary>
        /// Returns if the _ParallelScene is active (in front).
        /// </summary>
        /// <param name="_ParallelScene">ParallelScene to check.</param>
        /// <returns></returns>
        public bool IsSceneActive(ParallelScene _ParallelScene)
        {
            return this.IsSceneActive(_ParallelScene.ScenePath);
        }

        /// <summary>
        /// Returns if the _Scene is active (in front).
        /// </summary>
        /// <param name="_Scene">Scene to check.</param>
        /// <returns></returns>
        public bool IsSceneActive(Scene _Scene)
        {
            return this.IsSceneActive(_Scene.path);
        }

        /// <summary>
        /// Returns if the Scene at _ScenePath is active (in front).
        /// </summary>
        /// <param name="_ScenePath">Scene at path to check.</param>
        /// <returns></returns>
        public bool IsSceneActive(String _ScenePath)
        {
            return SceneManager.GetActiveScene().path == _ScenePath;
        }

        /// <summary>
        /// Returns the layer of the ParallelScene of _Scene.
        /// If the _Scene is not registered as ParallelScene, returns the Default Layer (0).
        /// </summary>
        /// <param name="_Scene">Get the layer for the passed Scene.</param>
        /// <returns></returns>
        public int GetLayer(Scene _Scene)
        {
            // Return the layer.
            return this.GetLayer(_Scene.path);
        }

        /// <summary>
        /// Returns the layer of the ParallelScene of the Scene at _ScenePath.
        /// If the Scene at _ScenePath is not registered as ParallelScene, returns the Default Layer (0).
        /// </summary>
        /// <param name="_ScenePath">Get the layer for the Scene at passed path.</param>
        /// <returns></returns>
        public int GetLayer(String _ScenePath)
        {
            // Get the ParallelScene by the _ScenePath.
            ParallelScene var_ParallelScene = this.FindParallelScene(_ScenePath);

            // There is no parallel scene at the passed _ScenePath. So return the Default Layer (0).
            if (var_ParallelScene == null)
            {
                // Log a warning!
                UnityEngine.Debug.LogWarning("There is no registered ParallelScene at: " + _ScenePath + ". So the default layer (0) will be returned.");

                // Return the Default Layer (0).
                return 0;
            }

            // Return the layer.
            return this.GetLayer(var_ParallelScene);
        }

        /// <summary>
        /// Returns the layer of the _ParallelScene.
        /// </summary>        
        /// <param name="_ParallelScene">Get the layer for the passed ParallelScene.</param>
        /// <returns></returns>
        public int GetLayer(ParallelScene _ParallelScene)
        {
            // If there is no parallel scene passed, throw an exception.
            if (_ParallelScene == null)
            {
                throw new ArgumentNullException("_ParallelScene");
            }

            // Return the layer.
            return _ParallelScene.Layer;
        }

        /// <summary>
        /// Returns the current TimeScale for a ParallelScene with _Scene.
        /// </summary>
        /// <param name="_Scene">Pass as Scene you want to get the TimeScale for.</param>
        /// <returns>TimeScale in float for this Scene.</returns>
        public float GetTimeScale(Scene _Scene)
        {
            return this.GetTimeScale(_Scene.path);
        }

        /// <summary>
        /// Returns the current TimeScale for a ParallelScene with _ScenePath.
        /// </summary>
        /// <param name="_ScenePath">Pass a relative scene path 'Assets/[...]/MyUnityScene.unity' or just the scene name 'MyUnityScene' you want to get the TimeScale for.</param>
        /// <returns>TimeScale in float for this Scene.</returns>
        public float GetTimeScale(String _ScenePath)
        {
            return this.GetParallelTime(_ScenePath).TimeScale;
        }

        /// <summary>
        /// Returns the current TimeScale for a _ParallelScene.
        /// </summary>
        /// <param name="_ParallelScene">Pass a ParallelScene you want to get the TimeScale for.</param>
        /// <returns>TimeScale in float for this Scene.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public float GetTimeScale(ParallelScene _ParallelScene)
        {
            return this.GetParallelTime(_ParallelScene).TimeScale;
        }

        /// <summary>
        /// Set the _TimeScale for a ParallelScene with _Scene.
        /// </summary>
        /// <param name="_Scene">Pass a Scene you want to set the TimeScale for.</param>
        /// <param name="_TimeScale">The TimeScale for the ParallelScene.</param>
        public void SetTimeScale(Scene _Scene, float _TimeScale)
        {
            // Set the _TimeScale for the found ParallelScene for _Scene.
            this.SetTimeScale(_Scene.path, _TimeScale);
        }

        /// <summary>
        /// Set the _TimeScale for a ParallelScene with _ScenePath.
        /// </summary>
        /// <param name="_ScenePath">Pass a relative scene path 'Assets/[...]/MyUnityScene.unity' or just the scene name 'MyUnityScene' you want to set the TimeScale for.</param>
        /// <param name="_TimeScale">The TimeScale for the ParallelScene.</param>
        public void SetTimeScale(String _ScenePath, float _TimeScale)
        {
            // Find the ParallelScene with the passed scene path. Might be null.
            ParallelScene var_ParallelScene = this.FindParallelScene(_ScenePath);

            // Set the _TimeScale for the found ParallelScene.
            this.SetTimeScale(var_ParallelScene, _TimeScale);
        }

        /// <summary>
        /// Set the _TimeScale for a _ParallelScene.
        /// </summary>
        /// <param name="_ParallelScene">Pass a ParallelScene you want to set the TimeScale.</param>
        /// <param name="_TimeScale">The TimeScale for the ParallelScene.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetTimeScale(ParallelScene _ParallelScene, float _TimeScale)
        {
            // If there is no parallel scene passed, throw an exception.
            if (_ParallelScene == null)
            {
                throw new ArgumentNullException("_ParallelScene");
            }

            // Set the TimeScale of the ParallelScene.
            if (this.scenePathToParallelTime.TryGetValue(_ParallelScene.ScenePath, out ParallelTime var_ParallelTime))
            {
                this.scenePathToParallelTime[_ParallelScene.ScenePath] = new ParallelTime(_TimeScale, var_ParallelTime.IsPaused);
            }
            else
            {
                this.scenePathToParallelTime[_ParallelScene.ScenePath] = new ParallelTime(_TimeScale, false);
            }
        }

        /// <summary>
        /// Returns if the ParallelScene is paused.
        /// </summary>
        /// <param name="_ScenePath">Pass a Scene you want to know if it is paused.</param>
        /// <returns>If the Scene is paused.</returns>
        public bool GetIsPaused(Scene _Scene)
        {
            return this.GetIsPaused(_Scene.path);
        }

        /// <summary>
        /// Returns if the ParallelScene is paused.
        /// </summary>
        /// <param name="_ScenePath">Pass a relative scene path 'Assets/[...]/MyUnityScene.unity' or just the scene name 'MyUnityScene' you want to know if it is paused.</param>
        /// <returns>If the Scene is paused.</returns>
        public bool GetIsPaused(String _ScenePath)
        {
            return this.GetParallelTime(_ScenePath).IsPaused;
        }

        /// <summary>
        /// Returns if the ParallelScene is paused.
        /// </summary>
        /// <param name="_ParallelScene">Pass a ParallelScene you want to know if it is paused.</param>
        /// <returns>If the Scene is paused.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool GetIsPaused(ParallelScene _ParallelScene)
        {
            return this.GetParallelTime(_ParallelScene).IsPaused;
        }

        /// <summary>
        /// Pause or unpause a ParallelScene with _Scene.
        /// </summary>
        /// <param name="_Scene">Pass a Scene you want to pause or unpause.</param>
        /// <param name="_IsPaused">True for pausing.</param>
        public void SetIsPaused(Scene _Scene, bool _IsPaused)
        {
            // Set _IsPaused for the found ParallelScene for _Scene.
            this.SetIsPaused(_Scene.path, _IsPaused);
        }

        /// <summary>
        /// Pause or unpause a ParallelScene with _ScenePath.
        /// </summary>
        /// <param name="_ScenePath">Pass a relative scene path 'Assets/[...]/MyUnityScene.unity' or just the scene name 'MyUnityScene' you want to pause or unpause.</param>
        /// <param name="_IsPaused">True for pausing.</param>
        public void SetIsPaused(String _ScenePath, bool _IsPaused)
        {
            // Find the ParallelScene with the passed scene path. Might be null.
            ParallelScene var_ParallelScene = this.FindParallelScene(_ScenePath);

            // Set _IsPaused for the found ParallelScene.
            this.SetIsPaused(var_ParallelScene, _IsPaused);
        }

        /// <summary>
        /// Pause or unpause a _ParallelScene.
        /// </summary>
        /// <param name="_ParallelScene">Pass a ParallelScene you want to pause or unpause.</param>
        /// <param name="_IsPaused">True for pausing.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetIsPaused(ParallelScene _ParallelScene, bool _IsPaused)
        {
            // If there is no parallel scene passed, throw an exception.
            if (_ParallelScene == null)
            {
                throw new ArgumentNullException("_ParallelScene");
            }

            // Set the ParallelScene to paused or unpaused.
            if (this.scenePathToParallelTime.TryGetValue(_ParallelScene.ScenePath, out ParallelTime var_ParallelTime))
            {
                this.scenePathToParallelTime[_ParallelScene.ScenePath] = new ParallelTime(var_ParallelTime.TimeScale, _IsPaused);
            }
            else
            {
                this.scenePathToParallelTime[_ParallelScene.ScenePath] = new ParallelTime(Time.timeScale, _IsPaused);
            }
        }

        /// <summary>
        /// Returns the deltaTime for a _ParallelScene.
        /// The deltaTime is the interval in seconds from the last frame to the current one.
        /// If the _ParallelScene is paused, the deltaTime is 0.0f.
        /// </summary>
        /// <param name="_Scene">Pass a Scene you want the deltaTime for.</param>
        /// <returns></returns>
        public float GetDeltaTime(Scene _Scene)
        {
            return this.GetDeltaTime(_Scene.path);
        }

        /// <summary>
        /// Returns the deltaTime for a ParallelScene with _ScenePath.
        /// The deltaTime is the interval in seconds from the last frame to the current one.
        /// If the ParallelScene is paused, the deltaTime is 0.0f.
        /// </summary>
        /// <param name="_ScenePath">Pass a relative scene path 'Assets/[...]/MyUnityScene.unity' or just the scene name 'MyUnityScene' you want the deltaTime for.</param>
        /// <returns></returns>
        public float GetDeltaTime(String _ScenePath)
        {
            // Get the ParallelTime for the passed Scene path.
            ParallelTime var_ParallelTime = this.GetParallelTime(_ScenePath);

            // If is paused, return 0.0f;
            if (var_ParallelTime.IsPaused)
            {
                return 0.0f;
            }
            
            // Else calculate and return the deltaTime.
            return Time.unscaledDeltaTime * var_ParallelTime.TimeScale;
        }

        /// <summary>
        /// Returns the deltaTime for a _ParallelScene.
        /// The deltaTime is the interval in seconds from the last frame to the current one.
        /// If the _ParallelScene is paused, the deltaTime is 0.0f.
        /// </summary>
        /// <param name="_ParallelScene">Pass a ParallelScene you want the deltaTime for.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public float GetDeltaTime(ParallelScene _ParallelScene)
        {
            // Get the ParallelTime for the passed Scene path.
            ParallelTime var_ParallelTime = this.GetParallelTime(_ParallelScene);

            // If is paused, return 0.0f;
            if (var_ParallelTime.IsPaused)
            {
                return 0.0f;
            }

            // Else calculate and return the deltaTime.
            return Time.unscaledDeltaTime * var_ParallelTime.TimeScale;
        }

        /// <summary>
        /// Clones the GameObject _Original and returns the clone.
        /// When you clone a GameObject or Component, all child objects and components are also cloned with their properties set like those of the original object.
        /// This cloned GameObject will then be moved to the _ParallelScene.
        /// </summary>
        /// <param name="_Original">GameObject to clone.</param>
        /// <param name="_ParallelScene">Move cloned object to.</param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject _Original, ParallelScene _ParallelScene)
        {
            // If there is no parallel scene passed, throw an exception.
            if (_ParallelScene == null)
            {
                throw new ArgumentNullException("_ParallelScene");
            }
            
            // Instantiate the GameObject.
            GameObject var_GameObject = GameObject.Instantiate(_Original);
            
            // Get the layer for the choosen _ParallelScene.
            int var_Layer = _ParallelScene.Layer;

            // Set the Layer for the new instantiated GameObject and Children.
            this.ApplyLayerToGameObjectsAndChildren(new GameObject[] { var_GameObject }, var_Layer);

            // Check if the active scene is already the choosen _ParallelScene.
            if (SceneManager.GetActiveScene().path == _ParallelScene.ScenePath)
            {
                // If so, just return the instantiated GameObject.
                return var_GameObject;
            }

            // Else, move the instantiated GameObject to the choosen _ParallelScene.
            Scene var_TargetScene = SceneManager.GetSceneByPath(_ParallelScene.ScenePath);
            
            SceneManager.MoveGameObjectToScene(var_GameObject, var_TargetScene);

            return var_GameObject;
        }

        /// <summary>
        /// Clones the GameObject _Original and returns the clone.
        /// When you clone a GameObject or Component, all child objects and components are also cloned with their properties set like those of the original object.
        /// This cloned GameObject will then be moved to the _ParallelScene.
        /// </summary>
        /// <param name="_Original">GameObject to clone.</param>
        /// <param name="_Position">Position for the cloned GameObject.</param>
        /// <param name="_Rotation">Rotation for the cloned GameObject.</param>
        /// <param name="_ParallelScene">Move cloned object to.</param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject _Original, Vector3 _Position, Quaternion _Rotation, ParallelScene _ParallelScene)
        {
            // If there is no parallel scene passed, throw an exception.
            if (_ParallelScene == null)
            {
                throw new ArgumentNullException("_ParallelScene");
            }
            
            // Instantiate the GameObject.
            GameObject var_GameObject = GameObject.Instantiate(_Original, _Position, _Rotation);

            // Get the layer for the choosen _ParallelScene.
            int var_Layer = _ParallelScene.Layer;

            // Set the Layer for the new instantiated GameObject and Children.
            this.ApplyLayerToGameObjectsAndChildren(new GameObject[] { var_GameObject }, var_Layer);

            // Check if the active scene is already the choosen _ParallelScene.
            if (SceneManager.GetActiveScene().path == _ParallelScene.ScenePath)
            {
                // If so, just return the instantiated GameObject.
                return var_GameObject;
            }

            // Else, move the instantiated GameObject to the choosen _ParallelScene.
            Scene var_TargetScene = SceneManager.GetSceneByPath(_ParallelScene.ScenePath);

            SceneManager.MoveGameObjectToScene(var_GameObject, var_TargetScene);

            return var_GameObject;
        }

        /// <summary>
        /// Move the _GameObject to the _ParallelScene and applying the _ParallelScene Layer to _GameObject and Children.
        /// </summary>
        /// <param name="_GameObject">GameObject to move.</param>
        /// <param name="_ParallelScene">Target scene.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void MoveGameObjectToScene(GameObject _GameObject, ParallelScene _ParallelScene)
        {
            // If there is no GameObject passed, throw an exception.
            if (_GameObject == null)
            {
                throw new ArgumentNullException("_GameObject");
            }

            // If there is no ParallelScene passed, throw an exception.
            if (_ParallelScene == null)
            {
                throw new ArgumentNullException("_ParallelScene");
            }

            // Get the target Scene.
            Scene var_TargetScene = SceneManager.GetSceneByPath(_ParallelScene.ScenePath);

            // Move the GameObject to the target Scene.
            this.MoveGameObjectToScene(_GameObject, var_TargetScene);
        }

        /// <summary>
        /// Move the _GameObject to the ParallelScene at _ScenePath and applying the ParallelScene Layer to _GameObject and Children.
        /// </summary>
        /// <param name="_GameObject">GameObject to move.</param>
        /// <param name="_ScenePath">Target scene.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void MoveGameObjectToScene(GameObject _GameObject, String _ScenePath)
        {
            // If there is no GameObject passed, throw an exception.
            if (_GameObject == null)
            {
                throw new ArgumentNullException("_GameObject");
            }

            // If there is no Scene path passed, throw an exception.
            if (_ScenePath == null)
            {
                throw new ArgumentNullException("_ScenePath");
            }

            // Get the target Scene.
            Scene var_TargetScene = SceneManager.GetSceneByPath(_ScenePath);

            // Move the GameObject to the target Scene.
            this.MoveGameObjectToScene(_GameObject, var_TargetScene);
        }

        /// <summary>
        /// Move the _GameObject to the ParallelScene _Scene and applying the ParallelScene Layer to _GameObject and Children.
        /// </summary>
        /// <param name="_GameObject">GameObject to move.</param>
        /// <param name="_Scene">Target scene.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void MoveGameObjectToScene(GameObject _GameObject, Scene _Scene)
        {
            // If there is no GameObject passed, throw an exception.
            if (_GameObject == null)
            {
                throw new ArgumentNullException("_GameObject");
            }

            // If there is no Scene passed, throw an exception.
            if (_Scene == null)
            {
                throw new ArgumentNullException("_Scene");
            }

            // Get the layer of the target Scene.
            int var_Layer = this.GetLayer(_Scene);

            // Set the Layer for the GameObject and Children.
            this.ApplyLayerToGameObjectsAndChildren(new GameObject[] { _GameObject }, var_Layer);

            // Move using the default Unity SceneManager.
            SceneManager.MoveGameObjectToScene(_GameObject, _Scene);
        }
    }
}
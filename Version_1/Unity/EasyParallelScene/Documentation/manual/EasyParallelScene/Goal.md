Having multiple unity scenes parallel running without them merging or interacting.
This should be easy to setup and also multiplayer supportable.
So you can have a world game scene and a editor scene for example.
Or like oyu know from skyrim, entering a cave and the outside world is still active.

Importance is the rendering for the camera and collision. And maybe each scene has a custom lighting. Or needs custom timescale.

GameObject based scene.

Layer based scene.

A Scene can have a custom camera, lighting, id, timescale/deltatime.

ParallelSceneManager : Singleton
ParallelScene is a TopGameObject.

ScriptAbleObject - ParallelScene
- Scene (Name): String
- Layer: int ( -1 = null and 0 to 31 for uniy)
- Lightning: Lightning
- TimeScale: float
- HashOverAll (https://forum.unity.com/threads/is-there-a-way-to-get-guid-on-runtime-in-build.892837/)

- Singleton

ParallelSceneManager
-> UnloadParallelScene
-> LoadParallelScene(ParallelScene / Name, Mode)
-> SwitchToParallelScene(ParallelScene / Name, PauseOtherScene):
Check if scene is laoded. Switch to camera of scene. Switch to ligtning if available.
-> GetDeltaTime(GameObject / ParallelScene / Name):
In a dicitonary there is scenename to deltatime * ParallelScene.TimeScale.
-> Instantiate(ParallelScene / Name, GameObject): Spawn the object in the scene and set the layer.
-> IsSceneActive(GameObject / ParallelScene / Name)
-> UnityEvents OnSceneActivated / OnSceneDeactivted and Messages
-> Transfer: Move one GameObject from current scene to the selected.

PRallelSceneHlerp /Editor
-> GetAllAvailableParallelScenes

->LoadAndMerge(SceneTo, SceneNew, ApplyLayerFromTo)
->Load(SceneNew, ApplyLayerFromNew)

->SwitchToParallelScene(PauseOtherScenes, MoveGameObjects[], UseCamera: Camera (enable = true))
Scene.GetRootGameObjects

TagHelper
->GetAllRootGameobjectWIthTag(Scene, searchChildren): Search CHildren too, if some child is Tag x, move specific root parent too with all children.


SceneManager.GetActiveScene / SetActiveScene : Decides which lightning is used and instantian the objects in is used.

Mode:
-> MergeScene: Additive adds scene loaded to current.
AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_Scene, LoadSceneMode.Additive);
Scene loadedScene = SceneManager.GetSceneByName(m_Scene)
Scene currentScene = SceneManager.GetActiveScene();
MergeScenes(loadedScene, currentScene); 
-> MergeSceneWithLayer: MergeScene and then set each Gameobjects Layer from loaded Scene to current ParallelScene Layer.
-> ApplyLayer: Apply the scenes layer to all gameojbects.


Nice to know:
https://docs.unity3d.com/Manual/LayerBasedCollision.html
https://docs.unity3d.com/Manual/class-TagManager.html
https://docs.unity3d.com/ScriptReference/LayerMask.NameToLayer.html
Recommend tags to differ objects: https://docs.unity3d.com/Manual/Tags.html

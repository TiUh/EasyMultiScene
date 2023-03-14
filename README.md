# Background

Occasionally, you need to run multiple scenes in parallel. For example, you may have a store scene 
or an indoor scene that runs alongside your main game scene, and you want to be able to switch 
between these different scenes without unloading the other scene and without having any physical 
interactions (like collisons, lightning, sound, ...) between those scenes.

Unfortunately, Unity currently lacks a built-in feature for running multiple scenes in parallel 
as described above. While it is possible to replicate this functionality by using certain 
techniques and workarounds, it is important to note that there will still be some limitations and 
drawbacks to doing so. These limitations may include issues such as increased complexity in the codebase, 
potential performance bottlenecks, and difficulty in maintaining and troubleshooting the implementation. 

Let's have a look at the common solutions:

* Deactivate the gameobjects of the first scene and active the ones from the second scene. And vice versa.

One option for switching between multiple scenes in Unity is to deactivate the gameobjects of the 
first scene and activate the ones from the second scene, and then switch back the other way when 
returning to the first scene. However, this method has the drawback that the gameobjects in the 
deactivated scene will not continue to run or update while they are inactive. This can cause issues 
such as maintaining the state of the gameobjects, preserving the continuity of gameplay, and ensuring 
that the gameobjects are ready to be used when they are reactivated. Additionally, deactivating 
and reactivating gameobjects can also introduce performance issues, especially if done frequently 
or with a large number of gameobjects. It can be a solution, but it should be used with caution 
and consideration of potential drawbacks and limitations.

* Have the gameobjects of the two scenes far away from each other in 3D space.

Another option for managing multiple scenes in Unity is to have the gameobjects of the different 
scenes placed far away from each other in 3D space. This allows the gameobjects of each scene to 
exist independently of one another, without interfering with each other's functionality. However, 
this method does require some additional considerations, such as finding a safe location to place 
the gameobjects of the second scene that is far enough away from the first scene so as not to cause 
conflicts or issues. It's also a solution that will require more attention to the design of the 
game, as the player's movement between the scenes should be smooth and logical.

Both of the solutions I described above are not suitable for a game that I am developing that 
requires a free open world scene and a separate scene that allows an editor to interact with that 
world. The first solution of deactivating gameobjects and switching between scenes would not work 
well in this case, as it would interrupt the continuity of the open world gameplay, create lacks 
because of deactivation/activation of many gameobjects and also would not allow the editor to seamlessly 
interact with the game world. The second solution of having the gameobjects of the different scenes 
placed far away from each other in 3D space would also not be suitable, as the open world is procedural 
and so you have to find a place, the player never reached.

So, to overcome the limitations of the previously mentioned solutions, I developed my own ways 
using Unity's layer techniques. By assigning different layers to the gameobjects in each of the 
scenes, I was able to use separate cameras to render them independently and keep their colliders and 
physics isolated from one another. This allowed me to have a free open world scene, as well as a 
separate scene for the editor to interact with the world, without any interference or conflicts between
the two. This approach also allowed me to maintain the continuity of the open world gameplay and 
allowed the editor to make real-time changes to the game world without interrupting the player's 
experience. Additionally, this solution is efficient, as it avoids unnecessary rendering and 
physics calculation, thus improving performance. Overall, this solution proved to be a more suitable 
and effective approach for my game, as it addresses the specific requirements and challenges of having 
both open world and editor scenes in the game, activate all the time.

# Description

EasyParallelScene is a simple but powerful asset/tool that enables developers to seamlessly manage multiple 
scenes running in parallel to each other. With this tool, you can run multiple scenes in the same world 
space without any interactions between them. It also allows you to switch between scenes easily, bringing 
a selected scene in front while keeping the other scenes running in the background (in parallel) and 
invisible to the player.

In addition to this, EasyParallelScene also allows you to pause different scenes individually, providing 
you with more control over the game's flow and pacing. You can also set custom time (such as delta time) 
for each scene, which can be used to create unique and immersive gameplay experiences. Overall, 
EasyParallelScene is a versatile tool that can help you take your game development to the next level 
by providing you with more control and flexibility over your multi-scene game or application.

**Key features**

The key features of EasyParallelScene are the following:

1. Ability to run multiple scenes in parallel.

2. Keep those parallel scenes within the same world space, without interacting with each other 
  (lightning/collisions/...).

3. Switch between parallel scenes and bring them in front while others run in the background.

4. Ability to pause different parallel scenes individually or give them a custom time scale.

**Advantages**

The advantages of EasyParallelScene are the following:

1. Allows for separate rendering of different scenes in the same world space, while keeping their 
  colliders and physics isolated from each other.

2. Maintains the continuity of the multiple scenes.

3. Flexibility, as you can use different cameras and settings for each scene.

4. Allows custom time scales for different scenes.

**Drawbacks**

But there are also some drawbacks of EasyParallelScene:

1. Layer limitations due to Unity only allowing 32 layers. However, you can still run 32 scenes parallel.

2. Only one layer can be assigned per parallel scene.

3. Custom time feature may not work for certain components such as animator or pathfinding.

4. Finding gameobjects and their interactions may require additional custom layer validation.

However, for all of these drawbacks, EasyParallelScene provides solutions to help you overcome them 
and make the most out of it.

# Is it suitable for you

EasyParallelScene is a powerful solution for those looking to create a more dynamic and engaging game or 
application. By allowing multiple scenes to run in parallel or within the same world space, without 
interactions between them, players can experience a more immersive and interactive game world.

However, it's important to keep in mind that this solution is best implemented at the beginning of your 
game development process. This is because setting up the necessary rendering, event, and collider layers 
are crucial for this solution to work, which can be a time-consuming task, depending on the state of development. 
Furthermore, it's important to note that there may be limitations on the number of layers Unity allows. 
Additionally, utilizing this solution may have certain limitations, such as the restriction of using only one layer 
per parallel scene.

Also if you already have a developed game, implementing EasyParallelScene may be challenging as it could require 
significant changes to the existing codebase, and it may take a lot of effort and resources to make it work 
seamlessly. It's important to carefully weigh the benefits and drawbacks before making a decision to implement 
this solution.

# Code example

The heart of EasyParallelScene is the ParallelSceneManager. Similar to the Unity SceneManager it allows to load
and unload scenes, but additionally allows to switch between those loaded scenes. To load a Scene in parallel,
you will use the following method:

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

It allows to load a Scene in parallel. You notice that you do not pass directly a Scene, instead you pass a 
ParallelScene object. A ParallelScene wraps around a Unity Scene and makes it parallelizable. This allows 
unique rendering, collision, lightning and time. 

    /// <summary>
    /// A ParallelScene wraps around a Unity Scene and makes it parallelizable.
    /// This allows unique rendering, collision and light.
    /// To create a ParallelScene, just right click anywhere in your project view and click Create->GUPS->EasyParallelScene->ParallelScene.
    /// Assign to this ParallelScene a Scene and apply your custom settings.
    /// </summary>
    [CreateAssetMenu(fileName = "NewParallelScene", menuName = "GUPS/EasyParallelScene/ParallelScene", order = Int32.MaxValue)]
    public class ParallelScene : ScriptableObject
	
The ParallelScene itself is serializeable. You can create it inside the Unity Editor project view. Here you 
assign a Scene that should be parallelizable, the rendering/collision layer and a custom time scale if needed.

To make this ParallelScene available it need to be registered to the ParallelSceneManager. You can either do
this via the Unity Editor Inspector or via code using this method:

    /// <summary>
	/// Register a ParallelScene while runtime, to make it available in GameMode.
	/// </summary>
	/// <param name="_ParallelScene">Pass a ParallelScene you want to register and make it available in game.</param>
	public void RegisterParallelScene(ParallelScene _ParallelScene)

Now you can load this ParallelScene and interact with it. To receive for example the registered layer for a 
ParallelScene you can use the ParallelSceneManager and call:

    /// <summary>
	/// Returns the layer of the _ParallelScene.
	/// </summary>        
	/// <param name="_ParallelScene">Get the layer for the passed ParallelScene.</param>
	/// <returns></returns>
	public int GetLayer(ParallelScene _ParallelScene)
	
Checking if two GameObjects are in the same ParallelScene, you use the GameObjectHelper and call:

    /// <summary>
	/// Returns true, if both GameObjects are in the same Scene.
	/// </summary>
	/// <param name="_GameObject1"></param>
	/// <param name="_GameObject2"></param>
	/// <returns></returns>
	public static bool GameObjectsAreInSameScene(GameObject _GameObject1, GameObject _GameObject2)
	
Finally, to switch between the ParallelScenes, you need to use the ParallelSceneManager. Here you can
switch between the Scenes and also decide how to switch between them.
	
	/// <summary>
	/// Switch to a loaded ParallelScene and set it as active Scene. If the scene is not loaded yet, returns false.
	/// </summary>
	/// <param name="_ParallelScene">Pass a ParallelScene to switch to.</param>
	/// <param name="_SwitchSceneFlag">Manage the Scene switching.</param>
	/// <param name="_PauseCurrentScene">Pause the active scene.</param>
	/// <returns>Returns if the switching was successful.</returns>
	public bool SwitchTo(ParallelScene _ParallelScene, ESwitchSceneFlag _SwitchSceneFlag, bool _PauseCurrentScene)

That's mostly the code you will need to setup and use EasyParallelScene.
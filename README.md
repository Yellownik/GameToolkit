# Game Toolkit
This repository contains some game making tools that I would like to collect in one place.
But also it might help you to create Unity game much easier.
All mentioned things are placed relative to 'Assets' folder

### 3rdParty
- [Demigiant](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676 "AssetStore - DOTween") contains DOTween engine for animating almost everything in Unity via C# code - sprites, sound, physic. See also:
  - [Documentation](http://dotween.demigiant.com/documentation.php)
  - [Easing functions](https://easings.net/)
  - [My DOTween Cheat Sheet](https://gist.github.com/Yellownik/b454f1841aafd7f26852e97c32930d83)
- [NaughtyAttributes](https://assetstore.unity.com/packages/tools/utilities/naughtyattributes-129996 "AssetStore - NaughtyAttributes") has interesting attributes for Unity Editor - Button, ShowIf, Required, ect
- [Orbox](https://bitbucket.org/orbox/orbox/src/master/ "Orbox/master") contains useful programming tools like ResourceManager, SoundManager, Promises, Timers
- [PathCreator](https://assetstore.unity.com/packages/tools/utilities/b-zier-path-creator-136082 "AssetStore - Bézier Path Creator") allows quick creating of smooth paths in the editor
- [TextMesh Pro](http://digitalnativestudios.com/textmeshpro/docs/ "Documentation") - no comments)

### Data
- **Textures** - contains Dissolve, Distortion, Gradient and Noise texture samples
- **Audio** - contains music and sound samples

### Resources
This folder contains loadable objects in certain hierarchy - path to the object have to match some enum for **IResourceManager** (for instance, "Resources/{namespace}/{enum type}/{enum value}.prefab").

Currently it has simple samples of music, click sounds, fly text, main/pause/titre menus, and objects for some services. 

### Manager scripts
**Root** script implements the idea of Composition Root design pattern (relatively similar to ServiceLocator, but with rigid servicing). All services/managers in Root have to be created at the start of the game.

**IResourceManager** - can load or create instance of object from Resource folder. Also can use pool for created objects (for returning to pool - just deactivate object).<br/>
Path to object associates with enum - "Resources/{namespace}/{enum type}/{enum value}.prefab".<br/>For instance, enum value ESounds.Click in namespace 'AudioSources' associates with "Resources/AudioSources/ESounds/Click.prefab"

- AudioManager - plays sound and music themes, with fading
- CameraManager (now - empty)
- FadeManager - fades screen with image material from UIRoot, allows setting of fade center
- InputManager - hub for intractable objects (should be blocked during fade and so on)
- TimerService - provides subscriptions to Update, 0.5s update, wait for second and for condition
- FlyTextManager
- SaveManager - now saves only music Volume, but can be extended
- MenuManager - manages Start, Pause and Titre menus
- UIRoot - has access to Main and Menu canvases, as well as FadeImage
- ViewFactory - common creator for UI objects 
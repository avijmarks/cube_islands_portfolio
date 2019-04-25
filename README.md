# cube_islands_portfolio
Portfolio Project for GoogleCardboard with a menu scene and different activities through portals on islands. Currently  consists of menu and one island, CubeRacer which is an endless runner style game for cardboard. Currently working on a tutorial/menu system and dynamic collision for cuberacer 

This project was loosely centered around the idea of exploring locomotion based experiences that show off the immersive value of virtual reality even on a system with incredibly limited user input, such as Google Cardboard. Currently, the entire project is usable and the games are playable, even on headsets _without_ the standard one button input functionality that comes with cardboard. 

## Menu Scene

![](readmeScreenshots/menu_scene1.png)

The menu scene consists of several islands around a large cube. The goal is to plan a variety of experiences all accessible through portals on different islands around the cube. Currently, there is only the starting island, which will have a tutorial feature soon, and an island with a portal to Cube Racer (LINK HERE) (endless runner style flying game). 

### Menu Scene Locomotion
<img src="readmeScreenshots/menu_scene_ui.png" width="500" >

Gif/image
The locomotion system used in the menu is based around allowing users to "fly" around in 3D space  without button input. I wanted users to be able to both start/stop, and fully retain the ability to look in any direction without vision being obstructed by button UI. The menu scene achievese this through a start/stop button disabling system that detects when a users gaze towards a button has significantly altered in velocity, as it might if they were naturally looking around, and disabling the start/stop button. 
* There are currently two buttons that both possess both start and stop functionality. 
* The buttons are at the top and bottom of the screen
* There is a (editable size) deadzone in the middle of the screen around where the users gaze would be horizontal.
* The algorithm that detects changes in acceleration and disables buttons _only_ activates when users gaze is _outside_ of this deadzone. 
* A small indicator ring will appear and snap to the cardboard reticle pointer to indicate to user when they are moving.
* Can easily be transported for use in another project (for instance a showcase of 3D models or art)

#### Relevant Scripts
* `MoveControlManager.cs` on PlayerControls empty under PlayerParent empty -- script that handles disabling of controllers
* `PlayerControlsPosition.cs on PlayerControls -- keeps controls in front of user
* `ControlFade.cs` on PlayerControls empty -- fades controllers based on position in vision
* `PlayerMove.cs` on PlayerParent -- Moves the entire stack
* `IndicatorManager.cs` on MoveIndicator object under MainCamera -- controls move indicator snapping



### Game Text
<img src="readmeScreenshots/gametext_portal.png" width="500" >

Uses 3D alphabet object prefabs to construct string messages passed to it through a function on a (static) singlet of the class `GameTextController`.
* currently instantiates from references to all children of an empty in the scene, going to be changed to an array with just a reference to prefabs in assets folder
* returns an empty that is the **parent** of the character prefabs of the message passed to it, this empty can then be moved etc
* can easily customize material, prefab size, and spacing between characters in `GameTextController.cs` inspector
* adaptable rotation and location offsets to parent on `GameController.gameText.CreateTextNode` function
* can easily be adapted to another project, all that is need is any alphabet asset set, `GameTextController.cs`, and `GameTextCharacter.cs` -- TextNode reference can be any empty and is just the "anchor" for the characters instantiated.

#### Future Plans for Game Text
* Going to be developed into a full light weight/portable (hopefully) menu system where text nodes can function as buttons with an on pointer enter "animation" and an on pointer click effect (starting the game etc)
* Create types of `GameText` you can create with preset materials and sizes from the arrays in `GameTextController` to simply function calls
* Full menus could be created just with button/nonbutton types of textnodes and position offsets from a singlescript very easily


### Future Plans for Menu Scene
* Tutorial for locomotion system using new GameText feature
* Removing the top  start/stop buttons from the menu locomotion scheme (after testing and feedback it doesn't seem like its necessary to have both). 
* Disabling algorithm for locomotion buttons will become sensitive to the user's vision turning, disabling the button if the user's gaze is not maintaining an approximately an vertical course towards the button, making the disable more natural and responsive.
* Potentially a cool interactive 3D perlin cube structure spawning within the center cube
* Menu's allowing the user to change colors in the environment



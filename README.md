# Paper Cat

## Required Toolset
* Unity 2021.2.0f1
* Visual Studio 2019 [Optional]

## Game Jam 2022 - Getting Started
* Find and open the main game jam scene under JamReady2022/Scemes/PrototypeScene01
* To edit the level geometry, simply select LevelGeometry_Fabgrid
	* You can then interact with the level editor using the inspector window
	* Under the "Paint" tab, you can pick the type of tool you'd like to use. The primary ones are Brush, Rectangle Tool and the eraser.
	* To make use of the eraser efficiently, you can hold "Left Shift" to enable it temporarily.
	* Additionally, there are 4 types of snap options that you can find under the level editor tool settings.
		* Adaptive - attempts to snap to meshes and if none is found, snaps to the grid.
		* Mesh - will directly snap to meshes in the level.
		* Floor - will snap to the floor of the level.
			* This is a lot like a grid snapping but you can change the elevation using the "Floor" textbox under the "Paint" tab.
		* Grid - this is fairly standard, the tiles will match themselves to the grid.
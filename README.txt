Procedural Square Grid Tilemap Generation in Unity
Ryan Roden-Corrent

This is an early proof-of-concept.

How to use:
1. Create a top-level empty object and attach the Tilemap script (or drag the Tilemap prefab into the scene)
2. Set NumRows and NumCols of the attached Tilemap script (on the object, not the prefab)
	- 30x30 is a good size. much larger is very slow to generate
3. Create some Terraformers
	a) create an empty GameObject
	b) attach a script from Scripts/Terraforming
	c) set the properties on the attached terraforming script
		- Row, Col sets the center point of this terraforming action on the Tilemap.
		  These values should be within the bounds set by NumRows, NumCols, of course
		- Range determines how far this terraformer reaches out from its start point
		- Density determines how agressively it fills in its target area. 
		  select a value 0 to 1.0
	d) repeat for each terraformer

4. Make the Tilemap aware of the Terraformers
	- the Tilemap script has a section called Terraformers. expand it.
	- set the Size value to the number of terraformers you have
	- drag each terraformer object you created into an Element slot of the Terraformers section
5. Select the Tilemap and click the Generate button

Notes:
The test scene contains an example of how this can be set up.
The terraformer objects are nested under a parent object called Terraformers
Terraformers are applied in order. Changes made by Terraformer 1 can overpower those of Terraformer 0.
There are currently 2 terraforming scripts:
CreateElevation: creates mountains within a specified range. Higher density means taller and more frequent elevation.
TextureTerrain: applies a material to tiles. This material must be defined in your assets and attached to the script.

Improvements:

These same concepts should be applicable to creating buildings, bodies of water, and other terrain

Having to create the terraformer objects and drag each one into a slot in the tilemap script is annoying.
I may be able to clean this up as I learn more about unity.

Should CreateElevation and TextureTerrain be combined into one script? Often elevation and texture go together.
For example, deserts are yellow-brown and flat, mountains are elevated and gray, hills are green...

Sometimes tiles are left blank if a TextureTerrain terraformer does not hit them.
This could be solved with a final pass that finds blank terrain and fills it with a nearby texture.

How to avoid creating terrain that creates an "impossible map"

Generate game-mechanic data along with visual terrain (e.g movement cost, defense bonus).

Generation gets slow quickly as map size increases. Do I just have an old machine?

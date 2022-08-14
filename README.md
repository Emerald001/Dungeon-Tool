<h1>Dungeon Drawer</h1>
<p>This is a simple yet fast tool to make floorplans for your dungeons</p>
</br>
<h2>Using the Tool</h2>
<p>1. Download and open the most recent build in the builds folder, this will show you the tool.</br>
2. Once you submitted the width and height of your grid, press the "Create Grid" button to start drawing.</br>
3. Click on any of the tiles on the grid to toggle wether it is walkable or not.</br>
4. Once you are content with your map, give it a name and press "Export Map" to save it in a specific spot on your pc.</br>
</br>
It gets saved as a JSON file and is basically just a list of Vector2Int's, you can use the generator provided or make a generator yourself that uses a list of gridpositions as it's input</p>
</br>
<h2>Importing via the Generator</h2>
<p>1. Once you have a map, you can use it in your unity project, add the Generator and MapContainer scripts from the Assets/Scripts folder to your project's Asset folder (or anywhere you want it) and add some walls and floors for the generator.</br>
2. Then add your Json file into your asset folder if you hadn't already.</br>
3. Once you have added the Generator script to any object, drag in the floor and wall prefabs and the JSON file.</br>
4. Press play and you will see the dungeon get generated!</br>
</br>

</p>

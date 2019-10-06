### Quick Cheatsheet for devs [WIP]

## Controls

### - Move the camera with W A S D, and rotate the camera using the RMB and moving it left or right, you can also zoom in and out with the mouse scroll wheel . In the editor you can invert the camera rotation in the "playerCamera" GameObject.

### - Spawn Enemies with the Numpads 0 in a desired waypoint (Configurable through the editor).

### - Click anywhere on the planet to throw a meteor at your enemies! (Explosion Blast still damages enemies).

### - Click the Gear Icon on the top left of the screen to start spawning turrets! In the correct zones, you will be able to preview the position of your turret with a green color. To place it, click the Middle Mouse Button. It will detect near enemies and shoot at them! You can also adjust its fire rate in the editor. To cancel the turret preview, click the gear icon again.

### - Your objective: Protect all your faires at all costs! If all of your fairies die, you lose! You can retry the level after the game over screen.

## Things you can change in the editor :

### "GameManager" (GameManager) : Max Enemies, Max Fairies, Max Turrets, Score Settings.

### "Player" (Player) : Speed , Zoom and Camera Settings.

###  "Hada" (NPC) : Speed, Initial State, Outline Colors, Invincibility Frames, Minimum distance to choose another waypoint.

### "Enemigo" (Enemy) (NPC) : Speed, Initial State, Minimum distance to choose another waypoint.

### "Shoot" (Weapon) (This GameObject is located within "playerCamera") : Fire Rate.

### "EnemySpawner1 (1),(2),etc" (Spawner) : Spawn Key, Minimum Enemy speed, Maximum Enemy Speed (a range between these values will be applied to the enemy's speed) , minimum spawn time, maximum spawn time.

### "Waypoint" (Waypoints): All Possible Connections.

### "TurretSpawner1 (1),(2),etc" (Spawner) : Fire Rate (also in Turret). *Not Configurable Yet*

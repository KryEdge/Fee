### Quick Cheatsheet for devs [WIP]

## Controls

### - Move the camera with W A S D, and rotate the camera using the RMB and moving it left or right, you can also zoom in and out with the mouse scroll wheel . In the editor you can invert the camera rotation in the "playerCamera" GameObject.

### - Spawn Enemies with the Numpads 1 and 2 (Configurable through the editor).

### - Click anywhere on the planet to throw a meteor at your enemies! (Explosion Blast still damages enemies).

### - Click the middle mouse button to spawn Turrets ( WIP ), it will detect near enemies and shoot at them! You can also adjust its fire rate in the editor.

## Things you can change in the editor :

### Camera "Player" (Player) : Speed , Zoom and Camera Settings.

###  *Ally "Hada" (NPC) : Speed, Default State , Minimum Switch Direction time, Maximum Switch Direction time (a range between these values will be applied to the ally's time between each rotation) , Waiting time before going to Idle State after Escaping. (depecrated)*

### Enemy "Enemigo" (Enemigo (1),(2),etc) (NPC) : Speed .

### Meteor "Shoot" (Weapon) (This GameObject is located within "playerCamera") : Fire Rate.

### Enemy Spawner "EnemySpawner1 (1),(2),etc" (Spawner) : Spawn Key, Minimum Enemy speed, Maximum Enemy Speed (a range between these values will be applied to the enemy's speed) , minimum spawn time, maximum spawn time.

## Important:
 > You can set up the enemies' waypoints, but it's not fully organized yet.

### Turret Spawner "TurretSpawner1 (1),(2),etc" (Spawner) : Fire Rate (also in Turret).

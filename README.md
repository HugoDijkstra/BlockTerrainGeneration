# Block base terrain generation in unity3d
## Inspired by minecraft
Im using a 3D perlin noise at y axis 0 to generate the top of the terain.
After that I optimize the terrain by deleting unseen faces and setting all the blocks on the inside to inactive.
Then i generate caves by going over the terrain with another perlin noise and deleting all blocks under a certain value.
Im able to visualize all of it by using multi-threading.

---

## TODO
* Tree generation
* Inventory
* Ore Spawning

---

### credits

* perlin noise | https://gist.github.com/Flafla2
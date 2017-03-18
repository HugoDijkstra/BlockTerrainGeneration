using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDictionairy
{

    static BlockDictionairy instance;
    List<Vector2> blocks;

    BlockDictionairy()
    {
        blocks = new List<Vector2>();
        blocks.Add(new Vector2(2, 15));
        blocks.Add(new Vector2(3, 15));
        blocks.Add(new Vector2(5, 15));
        blocks.Add(new Vector2(1, 15));
        blocks.Add(new Vector2(1, 14));
    }
    public List<Vector2> getDictionairy() { return blocks; }
    public static Vector2 getUv(int id)
    {
        return getInstance().blocks[id];

    }
    public static BlockDictionairy getInstance()
    {
        if (instance == null)
            instance = new BlockDictionairy();
        return instance;
    }
}

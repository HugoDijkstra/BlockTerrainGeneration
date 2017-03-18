using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField]
    Vector2 worldSize;
    [SerializeField]
    GameObject chunk;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(genWorld());
    }
    IEnumerator genWorld()
    {
        for (int i = 0; i < worldSize.x; i++)
        {
            for(int j = 0;j < worldSize.y;j++)
            {
                Instantiate(chunk, new Vector3(i * 16, 0, j * 16), Quaternion.identity, transform);
            }
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

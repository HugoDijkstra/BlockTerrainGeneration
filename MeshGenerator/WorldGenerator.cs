using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class WorldGenerator : MonoBehaviour
{


    [SerializeField]
    [Header("Texture Values")]
    Texture2D spriteSheet;
    [SerializeField]
    [Tooltip("1 / spritesheet rows")]
    float uvSize = 1;
    [SerializeField]
    [Tooltip("Position on spritesheet")]
    Dictionary<string, Vector2> textureUv = new Dictionary<string, Vector2>();
    [SerializeField]
    [Header("World Values")]
    Vector3 terainSize = Vector3.zero;
    GameObject[,,] world;

    [SerializeField]
    GameObject block;

    GameObject player;
    GameObject cam;
    bool destroyed = false;
    bool done = false;




    private Coroutine worldGenThreat;

    [SerializeField]
    static float seed;

    int sqaure = 84;


    float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
    public static double OctavePerlin(double x, double y, double z, int octaves, double persistence)
    {
        double total = 0;
        double frequency = 1;
        double amplitude = 1;
        for (int i = 0; i < octaves; i++)
        {
            total += perlin(x * frequency, y * frequency, z * frequency) * amplitude;

            amplitude *= persistence;
            frequency *= 2;
        }

        return total;
    }

    private static readonly int[] permutation = { 151,160,137,91,90,15,					// Hash lookup table as defined by Ken Perlin.  This is a randomly
		131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,	// arranged array of all numbers from 0-255 inclusive.
		190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
        102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
        135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
    };

    private static int[] p;                                                    // Doubled permutation to avoid overflow

    public static double perlin(double x, double y, double z)
    {

        int xi = (int)x & 255;                              // Calculate the "unit cube" that the point asked will be located in
        int yi = (int)y & 255;                              // The left bound is ( |_x_|,|_y_|,|_z_| ) and the right bound is that
        int zi = (int)z & 255;                              // plus 1.  Next we calculate the location (from 0.0 to 1.0) in that cube.
        double xf = x - (int)x;                             // We also fade the location to smooth the result.
        double yf = y - (int)y;
        double zf = z - (int)z;
        double u = fade(xf);
        double v = fade(yf);
        double w = fade(zf);

        int a = p[xi] + yi;                             // This here is Perlin's hash function.  We take our x value (remember,
        int aa = p[a] + zi;                             // between 0 and 255) and get a random value (from our p[] array above) between
        int ab = p[a + 1] + zi;                             // 0 and 255.  We then add y to it and plug that into p[], and add z to that.
        int b = p[xi + 1] + yi;                             // Then, we get another random value by adding 1 to that and putting it into p[]
        int ba = p[b] + zi;                             // and add z to it.  We do the whole thing over again starting with x+1.  Later
        int bb = p[b + 1] + zi;                             // we plug aa, ab, ba, and bb back into p[] along with their +1's to get another set.
                                                            // in the end we have 8 values between 0 and 255 - one for each vertex on the unit cube.
                                                            // These are all interpolated together using u, v, and w below.

        double x1, x2, y1, y2;
        x1 = lerp(grad(p[aa], xf, yf, zf),          // This is where the "magic" happens.  We calculate a new set of p[] values and use that to get
                    grad(p[ba], xf - 1, yf, zf),            // our final gradient values.  Then, we interpolate between those gradients with the u value to get
                    u);                                     // 4 x-values.  Next, we interpolate between the 4 x-values with v to get 2 y-values.  Finally,
        x2 = lerp(grad(p[ab], xf, yf - 1, zf),          // we interpolate between the y-values to get a z-value.
                    grad(p[bb], xf - 1, yf - 1, zf),
                    u);                                     // When calculating the p[] values, remember that above, p[a+1] expands to p[xi]+yi+1 -- so you are
        y1 = lerp(x1, x2, v);                               // essentially adding 1 to yi.  Likewise, p[ab+1] expands to p[p[xi]+yi+1]+zi+1] -- so you are adding
                                                            // to zi.  The other 3 parameters are your possible return values (see grad()), which are actually
        x1 = lerp(grad(p[aa + 1], xf, yf, zf - 1),      // the vectors from the edges of the unit cube to the point in the unit cube itself.
                    grad(p[ba + 1], xf - 1, yf, zf - 1),
                    u);
        x2 = lerp(grad(p[ab + 1], xf, yf - 1, zf - 1),
                      grad(p[bb + 1], xf - 1, yf - 1, zf - 1),
                      u);
        y2 = lerp(x1, x2, v);

        return (lerp(y1, y2, w) + 1) / 2;                       // For convenience we bound it to 0 - 1 (theoretical min/max before is -1 - 1)
    }

    public static double grad(int hash, double x, double y, double z)
    {
        int h = hash & 15;                                  // Take the hashed value and take the first 4 bits of it (15 == 0b1111)
        double u = h < 8 /* 0b1000 */ ? x : y;              // If the most signifigant bit (MSB) of the hash is 0 then set u = x.  Otherwise y.

        double v;                                           // In Ken Perlin's original implementation this was another conditional operator (?:).  I
                                                            // expanded it for readability.

        if (h < 4 /* 0b0100 */)                             // If the first and second signifigant bits are 0 set v = y
            v = y;
        else if (h == 12 /* 0b1100 */ || h == 14 /* 0b1110*/)// If the first and second signifigant bits are 1 set v = x
            v = x;
        else                                                // If the first and second signifigant bits are not equal (0/1, 1/0) set v = z
            v = z;

        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v); // Use the last 2 bits to decide if u and v are positive or negative.  Then return their addition.
    }

    public static double fade(double t)
    {
        // Fade function as defined by Ken Perlin.  This eases coordinate values
        // so that they will "ease" towards integral values.  This ends up smoothing
        // the final output.
        return t * t * t * (t * (t * 6 - 15) + 10);         // 6t^5 - 15t^4 + 10t^3
    }

    public static double lerp(double a, double b, double x)
    {
        return a + x * (b - a);
    }
    // Use this for initialization
    void Start()
    {
        p = new int[512];
        for (int x = 0; x < 512; x++)
        {
            p[x] = permutation[x % 256];
        }

        player = GameObject.Find("Player");
        if (player != null)
            player.SetActive(false);
        cam = GameObject.Find("GenCam");
        GameObject.Find("GenCam").SetActive(true);
        GameObject.Find("GenCam").transform.position = terainSize;
        GameObject.Find("GenCam").transform.LookAt(Vector3.zero);

        world = new GameObject[(int)terainSize.x, (int)terainSize.y, (int)terainSize.z];


        worldGenThreat = StartCoroutine(startWorld());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && done && worldGenThreat == null)
        {
            done = false;
            if (destroyed == false)
            {
                worldGenThreat = StartCoroutine(destroyWorld());
            }
            else
            {
                worldGenThreat = StartCoroutine(startWorld());
            }
        }
    }



    IEnumerator destroyWorld()
    {
        activeGenCam();
        yield return destroyPerlin();
        done = true;
        destroyed = false;
        System.GC.Collect();
        System.GC.Collect();

        destroyed = false;
        worldGenThreat = null;
    }
    void activeGenCam()
    {
        if (player != null)
            player.SetActive(false);
        if (cam != null)
            cam.SetActive(true);
        destroyed = true;
        done = true;
    }

    IEnumerator destroyPerlin()
    {
        for (int i = 0; i < terainSize.x; i++)
        {
            for (int j = 0; j < terainSize.y; j++)
            {
                for (int p = 0; p < terainSize.z; p++)
                {
                    destroyBlockAt(i, j, p);
                }
            }
            yield return null;
        }
    }
    IEnumerator startWorld()
    {
        yield return genPerlin();
        yield return genWorld();
        yield return setVisibleActive();
        yield return genCaves();
        System.GC.Collect();
        System.GC.Collect();
        activatePlayer();
        done = true;
        worldGenThreat = null;
    }

    IEnumerator genWorld()
    {
        sqaure = 0;
        for (int i = 0; i < terainSize.x; i++)
        {
            for (int j = 0; j < terainSize.y; j++)
            {
                for (int p = 0; p < terainSize.z; p++)
                {
                    GameObject right, left, up, down, front, back;
                    if (world[i, j, p] != null)
                    {
                        int sides;
                        //check right
                        if (i == terainSize.x - 1)
                            right = null;
                        else
                            right = world[i + 1, j, p];
                        //check left
                        if (i == 0)
                            left = null;
                        else
                            left = world[i - 1, j, p];
                        //check up
                        if (j == terainSize.y - 1)
                            up = null;
                        else
                            up = world[i, j + 1, p];
                        //check down
                        if (j == 0)
                            down = null;
                        else
                            down = world[i, j - 1, p];
                        //check front
                        if (p == terainSize.z - 1)
                            front = null;
                        else
                            front = world[i, j, p + 1];
                        //check back
                        if (p == 0)
                            back = null;
                        else
                            back = world[i, j, p - 1];

                        world[i, j, p].GetComponent<Block>().setSides(right, left, up, down, front, back);
                        world[i, j, p].GetComponent<Block>().updateMesh();
                        if (right == null && left == null && up == null && down == null && front == null && back == null)
                            gameObject.SetActive(false);
                    }

                }
            }
            yield return null;
        }

    }
    void activatePlayer()
    {
        if (player != null)
            player.SetActive(true);
        if (cam != null)
            cam.SetActive(false);
        if (player != null)
            player.transform.position = new Vector3((int)terainSize.x / 2, findHighestBlock((int)terainSize.x / 2, (int)terainSize.z / 2) * 2, (int)terainSize.z / 2);

    }

    IEnumerator genCaves()
    {
        for (int z = 0; z < terainSize.z; z++)
        {
            for (int x = 0; x < terainSize.x; x++)
            {
                for (int y = 1; y < terainSize.y; y++)
                {
                    float p = (float)perlin((double)(seed + transform.position.x + x) / 10.0f, (double)(seed + y) / 10.0f, (double)(seed + transform.position.z + z) / 10.0f);
                    if (p > 0.6f)
                    {
                        destroyBlockAt(x, y, z);
                        cam.transform.position = new Vector3(x, y + 20, z);
                        cam.transform.LookAt(Vector3.zero);
                    }
                }
            }
            yield return null;
        }
    }
    IEnumerator genPerlin()
    {
        for (int z = 0; z < terainSize.z; z++)
        {
            for (int x = 0; x < terainSize.x; x++)
            {
                for (int y = 0; y < terainSize.y; y++)
                {
                    if (y == 0)
                    {
                        world[x, y, z] = Instantiate(block, new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z), Quaternion.identity , transform);
                        world[x, y, z].GetComponent<Block>().setBlock(4);
                    }
                    else if (y / terainSize.y < 0.6f)
                    {
                        world[x, y, z] = Instantiate(block, new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z), Quaternion.identity, transform);
                        world[x, y, z].GetComponent<Block>().setBlock(3);
                        cam.transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
                        cam.transform.LookAt(Vector3.zero);
                    }
                    else
                    {
                        float p = (float)perlin((double)(seed + transform.position.x + x) / 30.0f, 0.0f, (double)(seed + transform.position.z + z) / 30.0f);
                        p = map(p, 0.0f, 1.0f, terainSize.y * 0.6f, terainSize.y * 0.8f);
                        if (y < p)
                        {

                            world[x, y, z] = Instantiate(block, new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z), Quaternion.identity, transform);
                            cam.transform.position = new Vector3(x, y + 20, z);
                            cam.transform.LookAt(Vector3.zero);
                        }
                    }
                }
                /*A
                {
                    float p = (float)perlin((double)(seed + x) / 10.0f, (double)(seed + y) / 10.0f, (double)(seed + z) / 10.0f);
                    if (p < 0.5f)
                    {
                        world[x, y, z] = Instantiate(block, new Vector3(x, y, z), Quaternion.identity);
                        cam.transform.position = new Vector3(x, y + 20, z);
                        cam.transform.LookAt(Vector3.zero);
                    }
                    else
                    {
                    }
                }*/
            }
            yield return null;
        }
    }

    IEnumerator setVisibleActive()
    {
        for (int i = 0; i < terainSize.x; i++)
        {
            for (int j = 0; j < terainSize.y; j++)
            {
                for (int p = 0; p < terainSize.z; p++)
                {
                    GameObject right, left, up, down, front, back;

                    if (world[i, j, p] != null)
                    {
                        bool active = world[i, j, p].GetComponent<Block>().right != null && world[i, j, p].GetComponent<Block>().left != null && world[i, j, p].GetComponent<Block>().up != false && world[i, j, p].GetComponent<Block>().down != null && world[i, j, p].GetComponent<Block>().front != null && world[i, j, p].GetComponent<Block>().back != null;

                        world[i, j, p].SetActive(!active);

                    }
                }
            }
            yield return null;
        }
    }




    int findHighestBlock(int x, int z)
    {
        if (x < 0 || x > terainSize.x - 1 || z < 0 || z > terainSize.z - 1)
        {
            return -1;
        }
        for (int i = (int)terainSize.y - 2; i > 0; i--)
        {
            if (world[x, i, z] != null)
            {
                return i;
            }
        }
        return -1;
    }

    public void destroyBlockAt(int x, int y, int z)
    {
        Destroy(world[x, y, z]);
    }
    public void destroyBlockAt(Vector3 pos)
    {
        if (pos.y > 0)
            Destroy(world[(int)pos.x, (int)pos.y, (int)pos.z]);
    }
    public void addBlockAt(Vector3 pos, int type)
    {
        if (pos.x >= 0 && pos.x < terainSize.x && pos.y >= 0 && pos.y < terainSize.y && pos.z >= 0 && pos.z < terainSize.z)
        {
            GameObject right, left, up, down, front, back;

            GameObject b = (GameObject)Instantiate(block, new Vector3((int)pos.x, (int)pos.y, (int)pos.z), Quaternion.identity, transform);
            int i = (int)pos.x, j = (int)pos.y, p = (int)pos.z;
            //check right
            if (i == terainSize.x - 1)
                right = null;
            else
                right = world[i + 1, j, p];
            //check left
            if (i == 0)
                left = null;
            else
                left = world[i - 1, j, p];
            //check up
            if (j == terainSize.y - 1)
                up = null;
            else
                up = world[i, j + 1, p];
            //check down
            if (j == 0)
                down = null;
            else
                down = world[i, j - 1, p];
            //check front
            if (p == terainSize.z - 1)
                front = null;
            else
                front = world[i, j, p + 1];
            //check back
            if (p == 0)
                back = null;
            else
                back = world[i, j, p - 1];

            b.GetComponent<Block>().setSides(right, left, up, down, front, back);

            if (left != null)
            {
                left.GetComponent<Block>().right = b;
                left.GetComponent<Block>().updateMesh();
            }
            if (right != null)
            {
                right.GetComponent<Block>().left = b;
                right.GetComponent<Block>().updateMesh();
            }
            if (up != null)
            {
                up.GetComponent<Block>().down = b;
                up.GetComponent<Block>().updateMesh();
            }
            if (down != null)
            {
                down.GetComponent<Block>().up = b;
                down.GetComponent<Block>().updateMesh();
            }
            if (front != null)
            {
                front.GetComponent<Block>().back = b;
                front.GetComponent<Block>().updateMesh();

            }
            if (back != null)
            {
                back.GetComponent<Block>().front = b;
                back.GetComponent<Block>().updateMesh();
            }

            b.GetComponent<Block>().updateMesh();
            world[(int)pos.x, (int)pos.y, (int)pos.z] = b;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Block : MonoBehaviour
{

    List<Vector3> vertecis;
    List<int> tris;
    List<Vector2> uv;

    [SerializeField]
    float uvSize;
    Mesh mesh;

    [SerializeField]
    Texture2D texture;

    public bool justEnabled = false;

    int id;
    int x, y, z;

    int sqaure = 0;
    public GameObject right, left, up, down, front, back;
    bool updated = false;
    // Use this for initialization
    void Start()
    {
        uvSize = 1.0f / 16.0f;
        vertecis = new List<Vector3>();
        tris = new List<int>();
        uv = new List<Vector2>();

    }
    void Update()
    {
        if (!updated)
        {
            updateMesh();
            updated = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
            updated = true;
    }

    public Mesh getMesh()
    {
        return mesh;
    }
    public void setSides(GameObject r, GameObject l, GameObject u, GameObject d, GameObject f, GameObject b)
    {
        this.right = r;
        this.left = l;
        this.up = u;
        this.down = d;
        this.front = f;
        this.back = b;
    }
    public void setBlock(int id)
    {
        this.id = id;
    }
    public void updateMesh()
    {
        sqaure = 0;
        if (mesh != null)
            mesh.Clear();
        else { mesh = new Mesh(); }
        if (vertecis != null)
            vertecis.Clear();
        else vertecis = new List<Vector3>();
        if (uv != null)
            uv.Clear();
        else uv = new List<Vector2>();
        if (tris != null)
            tris.Clear();
        else tris = new List<int>();

        if (right == null)
        {
            genRightSide(0, 0, 0);
        }
        if (left == null)
        {
            genLeftSide(0, 0, 0);
        }
        if (front == null)
        {
            genFrontSide(0, 0, 0);
        }
        if (back == null)
        {
            genBackSide(0, 0, 0);
        }
        if (up == null)
        {
            genTop(0, 0, 0);
        }
        if (down == null)
        {
            genBottom(0, 0, 0);
        }
        mesh.vertices = vertecis.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();

        mesh.uv = uv.ToArray();
        mesh.name = "Vertcount: " + mesh.vertexCount.ToString();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }


    void genTop(int x, int y, int z)
    {
        vertecis.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        vertecis.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        vertecis.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        vertecis.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

        tris.Add(sqaure * 4);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 3);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 2);
        tris.Add((sqaure * 4) + 3);
        Vector2 texture = Vector2.zero;
        if (id == 0)
            texture = BlockDictionairy.getUv(2);
        else
            texture = BlockDictionairy.getUv(id);

        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y + uvSize));
        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y + uvSize));
        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y));
        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y));
        sqaure++;
    }
    void genBottom(int x, int y, int z)
    {
        vertecis.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        vertecis.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        vertecis.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        vertecis.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        tris.Add(sqaure * 4);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 3);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 2);
        tris.Add((sqaure * 4) + 3);
        Vector2 texture = Vector3.zero;
        if (id == 0)
            texture = BlockDictionairy.getUv(0);
        else
            texture = BlockDictionairy.getUv(id);
        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y + uvSize));
        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y + uvSize));
        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y));
        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y));
        sqaure++;
    }
    void genLeftSide(int x, int y, int z)
    {
        vertecis.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        vertecis.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        vertecis.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        vertecis.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        tris.Add(sqaure * 4);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 3);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 2);
        tris.Add((sqaure * 4) + 3);
        Vector2 texture = Vector2.zero;
        if (id == 0)
            texture = BlockDictionairy.getUv((up == null) ? 1 : 0);
        else
            texture = BlockDictionairy.getUv(id);


        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y));
        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y + uvSize));
        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y + uvSize));
        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y));

        sqaure++;
    }
    void genRightSide(int x, int y, int z)
    {
        vertecis.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        vertecis.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        vertecis.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        vertecis.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));


        tris.Add(sqaure * 4);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 3);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 2);
        tris.Add((sqaure * 4) + 3);
        Vector2 texture = Vector2.zero;
        if (id == 0)
            texture = BlockDictionairy.getUv((up == null) ? 1 : 0);
        else
            texture = BlockDictionairy.getUv(id);
        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y));
        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y));
        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y + uvSize));
        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y + uvSize));
        sqaure++;
    }
    void genBackSide(int x, int y, int z)
    {
        vertecis.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        vertecis.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        vertecis.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        vertecis.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

        tris.Add(sqaure * 4);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 3);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 2);
        tris.Add((sqaure * 4) + 3);
        Vector2 texture = Vector2.zero;
        if (id == 0)
            texture = BlockDictionairy.getUv((up == null) ? 1 : 0);
        else
            texture = BlockDictionairy.getUv(id);

        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y + uvSize));
        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y));
        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y));
        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y + uvSize));
        sqaure++;
    }
    void genFrontSide(int x, int y, int z)
    {
        vertecis.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        vertecis.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        vertecis.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        vertecis.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));


        tris.Add(sqaure * 4);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 3);
        tris.Add((sqaure * 4) + 1);
        tris.Add((sqaure * 4) + 2);
        tris.Add((sqaure * 4) + 3);

        Vector2 texture = Vector2.zero;
        if (id == 0)
            texture = BlockDictionairy.getUv((up == null) ? 1 : 0);
        else
            texture = BlockDictionairy.getUv(id);

        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y + uvSize));
        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y + uvSize));
        uv.Add(new Vector2(uvSize * texture.x, uvSize * texture.y));
        uv.Add(new Vector2(uvSize * texture.x + uvSize, uvSize * texture.y));


        sqaure++;
    }


    void OnDestroy()
    {
        if (left != null)
        {
            left.SetActive(true);
            left.GetComponent<Block>().right = null;
            left.GetComponent<Block>().updateMesh();
        }
        if (right != null)
        {
            right.SetActive(true);
            right.GetComponent<Block>().left = null;
            right.GetComponent<Block>().updateMesh();
        }
        if (up != null)
        {
            up.SetActive(true);
            up.GetComponent<Block>().down = null;
            up.GetComponent<Block>().updateMesh();
        }
        if (down != null)
        {
            down.SetActive(true);
            down.GetComponent<Block>().up = null;
            down.GetComponent<Block>().updateMesh();
        }
        if (front != null)
        {
            front.SetActive(true);
            front.GetComponent<Block>().back = null;
            front.GetComponent<Block>().updateMesh();

        }
        if (back != null)
        {
            back.SetActive(true);
            back.GetComponent<Block>().front = null;
            back.GetComponent<Block>().updateMesh();
        }
        //
        if (left != null)
        {
            left.GetComponent<Block>().updateMesh();
        }
        if (right != null)
        {
            right.GetComponent<Block>().updateMesh();
        }
        if (up != null)
        {
            up.GetComponent<Block>().updateMesh();
        }
        if (down != null)
        {
            down.GetComponent<Block>().updateMesh();
        }
        if (front != null)
        {
            front.GetComponent<Block>().updateMesh();

        }
        if (back != null)
        {
            back.GetComponent<Block>().updateMesh();
        }
    }

}

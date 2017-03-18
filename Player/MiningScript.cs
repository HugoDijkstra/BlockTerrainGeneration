using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class MiningScript : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, -2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10))
            {
                if (hit.transform.GetComponent<Block>() != null) { }
                hit.transform.parent.GetComponent<WorldGenerator>().destroyBlockAt(hit.transform.localPosition);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, -2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10))
            {
                if (hit.transform.GetComponent<Block>() != null)
                {
                    Vector3 at = hit.transform.localPosition + hit.normal;
                    print(at);
                    print(hit.transform.parent);
                    hit.transform.parent.GetComponent<WorldGenerator>().addBlockAt(at, 0);
                }
            }
        }
    }
}

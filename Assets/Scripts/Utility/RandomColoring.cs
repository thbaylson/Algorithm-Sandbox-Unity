using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColoring : MonoBehaviour
{
    void Start()
    {
        var mat = GetComponent<MeshRenderer>().material;
        mat.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }
}

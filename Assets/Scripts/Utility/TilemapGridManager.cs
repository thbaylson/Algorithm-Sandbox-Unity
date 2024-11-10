using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGridManager : MonoBehaviour
{
    [SerializeField]
    RuleTile[] ruleTiles;

    [SerializeField]
    int width;//35

    [SerializeField]
    int height;//18

    private Grid _grid;
    private Tilemap _tilemap;

    // Start is called before the first frame update
    void Start()
    {
        // Setup the Grid
        _grid = new GameObject("Grid").AddComponent<Grid>();
        _grid.transform.SetParent(transform);

        // Setup the Tilemap
        _tilemap = new GameObject("Tilemap").AddComponent<Tilemap>();
        _tilemap.AddComponent<TilemapRenderer>();
        _tilemap.transform.SetParent(_grid.transform);

        PopulateTilemap(_tilemap);
    }

    private void PopulateTilemap(Tilemap tilemap)
    {
        tilemap.ClearAllTiles();

        // Populate the Tilemap
        var tiles = WaveFunctionCollapse.GenerateTileArray(width, height, ruleTiles);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), tiles[i][j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PopulateTilemap(_tilemap);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGridManager : MonoBehaviour
{
    [SerializeField] Tilemap exampleTilemap;
    [SerializeField] TileBase defaultTile;
    [SerializeField] int width;
    [SerializeField] int height;

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
        //_tilemap = Instantiate(exampleTilemap, _grid.transform);

        exampleTilemap.CompressBounds();
        //exampleTilemap.origin = new Vector3Int(0, 0, 0);
        PopulateTilemap(_tilemap);
    }

    private void PopulateTilemap(Tilemap tilemap)
    {
        tilemap.ClearAllTiles();

        // Populate the Tilemap
        var tiles = WaveFunctionCollapse.GenerateSpriteArray(width, height, exampleTilemap);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                TileBase nextTile = tiles[i][j] ?? defaultTile;
                tilemap.SetTile(new Vector3Int(i, j, 0), nextTile);
            }
        }
    }

    public void GenerateNewMap()
    {
        PopulateTilemap(_tilemap);
    }
}

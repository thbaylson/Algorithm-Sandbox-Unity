using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/***
 * Given a tileset with adjacency constraints, this algorithm will generate tilemaps
 * that appropriately adhere to those adjacency contraints. This classes uses '2D Tilemap Extras'
 * to access Unity's RuleTile class.
 */
public static class WaveFunctionCollapse
{
    private class WaveCell
    {
        public WaveCell(int x, int y, TileBase[] possibleTiles)
        {
            this.possibleTiles = possibleTiles;
            this.x = x;
            this.y = y;
            collapsed = false;
        }

        public int GetEntropy()
        {
            if(possibleTiles == null) return 0;
            return possibleTiles.Length;
        }

        public TileBase tile;
        public TileBase[] possibleTiles;
        public int x;
        public int y;
        public bool collapsed;
    }

    /// <summary>
    /// Returns an n x m grid of procedurally chosen tiles. Uses WFC to determine logical tile arrangement.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="m"></param>
    /// <param name="ruleTiles"></param>
    public static TileBase[][] GenerateTileArray(int n, int m, RuleTile[] ruleTiles)
    {
        WaveCell[][] waveArray = GenerateWaveArray(n, m, ruleTiles);
        
        Stack<WaveCell> cells = new Stack<WaveCell>();
        var newLowest = GetLowestEntropyCell(n, m, waveArray);
        if (newLowest != null)
        {
            cells.Push(newLowest);
        }

        int cnt = 0; // Quick and dirty stopping condition
        int maxCnt = 10000;
        while (cells.Count > 0 && cnt < maxCnt)
        {
            WaveCell currentCell = cells.Pop();
            CollapseWaveCell(currentCell, ruleTiles, waveArray);

            // Push new lowest entropy onto the stack
            newLowest = GetLowestEntropyCell(n, m, waveArray);
            if (newLowest != null)
            {
                cells.Push(newLowest);
            }
            cnt++;
        }

        if(cnt >= maxCnt)
        {
            Debug.Log("Stopped due to count safeguard");
        }

        return waveArray.Select(cols => cols.Select(cell => cell.tile).ToArray()).ToArray();
    }

    private static WaveCell GetLowestEntropyCell(int n, int m, WaveCell[][] waveArray)
    {
        int lowestEntropy = int.MaxValue;
        List<WaveCell> lowestEntropyCells = new List<WaveCell>();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                WaveCell cell = waveArray[i][j];
                int currentEntropy = cell.GetEntropy();

                if (!cell.collapsed && currentEntropy > 0)
                {
                    if (currentEntropy < lowestEntropy)
                    {
                        lowestEntropy = currentEntropy;
                        lowestEntropyCells.Clear();
                        lowestEntropyCells.Add(cell);
                    }
                    else if (currentEntropy == lowestEntropy)
                    {
                        lowestEntropyCells.Add(cell);
                    }
                }
            }
        }

        if (lowestEntropyCells.Count > 0)
        {
            return lowestEntropyCells[UnityEngine.Random.Range(0, lowestEntropyCells.Count)];
        }

        return null;
    }

    private static void CollapseWaveCell(WaveCell currentCell, RuleTile[] ruleTiles, WaveCell[][] waveArray)
    {
        // Determine what this cell is allowed to be based on possibilities and weights
        currentCell.tile = currentCell.possibleTiles[UnityEngine.Random.Range(0, currentCell.possibleTiles.Length)];

        // The cell has been determined, there are no more possibilities
        currentCell.possibleTiles = null;

        // Collapse the cell
        currentCell.collapsed = true;

        // Update neighbors' entropies
        PropagateConstraints(currentCell, ruleTiles, waveArray);
    }

    // Generate wave array
    private static WaveCell[][] GenerateWaveArray(int n, int m, TileBase[] possibleTiles)
    {
        WaveCell[][] tileBases = new WaveCell[n][];
        for (int i = 0; i < n; i++)
        {
            tileBases[i] = new WaveCell[m];
            for (int j = 0; j < m; j++)
            {
                tileBases[i][j] = new WaveCell(i, j, possibleTiles);
            }
        }

        return tileBases;
    }

    // Adjacency constraint propagation
    private static void PropagateConstraints(WaveCell currentCell, RuleTile[] ruleTiles, WaveCell[][] waveArray)
    {
        // Check the rule's neighbors and update the entropies of neighboring cells
        for (int i = currentCell.x - 1; i <= currentCell.x + 1; i++)
        {
            for (int j = currentCell.y - 1; j <= currentCell.y + 1; j++)
            {
                if (i >= 0 && i < waveArray.Length && j >= 0 && j < waveArray[0].Length && !waveArray[i][j].collapsed)
                {
                    // Update the entropy of the neighboring cell based on the rule
                    UpdateEntropy(waveArray[i][j], waveArray, currentCell);
                }
            }
        }
    }

    private static void UpdateEntropy(WaveCell neighborCell, WaveCell[][] waveArray, WaveCell currentCell)
    {
        if (neighborCell.possibleTiles == null)
        {
            return;
        }

        // Collect constraints from all neighboring cells
        HashSet<TileType> allowedTileTypes = new HashSet<TileType>(Enum.GetValues(typeof(TileType)).Cast<TileType>());
        for (int i = neighborCell.x - 1; i <= neighborCell.x + 1; i++)
        {
            for (int j = neighborCell.y - 1; j <= neighborCell.y + 1; j++)
            {
                if (i >= 0 && i < waveArray.Length && j >= 0 && j < waveArray[0].Length && waveArray[i][j].collapsed)
                {
                    TileType neighborTileType = GetTileType(waveArray[i][j].tile);
                    Direction direction = GetDirection(neighborCell.x, neighborCell.y, i, j);
                    List<TileType> neighborConstraints = TileConstraints.AdjacencyConstraints[neighborTileType][direction];
                    allowedTileTypes.IntersectWith(neighborConstraints);
                }
            }
        }

        // Filter possible tiles based on the combined constraints
        List<TileBase> newPossibleTiles = new List<TileBase>();
        foreach (TileBase possibleTile in neighborCell.possibleTiles)
        {
            TileType possibleTileType = GetTileType(possibleTile);
            if (allowedTileTypes.Contains(possibleTileType))
            {
                newPossibleTiles.Add(possibleTile);
            }
        }

        neighborCell.possibleTiles = newPossibleTiles.ToArray();
    }

    private static TileType GetTileType(TileBase tile)
    {
        if (tile.name.Contains("Ground"))
        {
            return TileType.Ground;
        }
        else if (tile.name.Contains("Wall"))
        {
            return TileType.Wall;
        }
        else if (tile.name.Contains("Water"))
        {
            return TileType.Water;
        }

        return TileType.Ground; // Default type
    }

    private static Direction GetDirection(int x1, int y1, int x2, int y2)
    {
        if (x1 == x2 && y1 == y2 - 1) return Direction.Up;
        if (x1 == x2 && y1 == y2 + 1) return Direction.Down;
        if (x1 == x2 - 1 && y1 == y2) return Direction.Right;
        if (x1 == x2 + 1 && y1 == y2) return Direction.Left;
        if (x1 == x2 - 1 && y1 == y2 - 1) return Direction.TopRight;
        if (x1 == x2 + 1 && y1 == y2 - 1) return Direction.TopLeft;
        if (x1 == x2 - 1 && y1 == y2 + 1) return Direction.BottomRight;
        if (x1 == x2 + 1 && y1 == y2 + 1) return Direction.BottomLeft;

        throw new ArgumentException("Invalid direction");
    }
}

public enum TileType
{
    Empty,
    Ground,
    Wall,
    Water,
}
public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}

public static class TileConstraints
{
    public static Dictionary<TileType, Dictionary<Direction, List<TileType>>> AdjacencyConstraints = new Dictionary<TileType, Dictionary<Direction, List<TileType>>>()
    {
        { TileType.Empty, new Dictionary<Direction, List<TileType>> {
            { Direction.Up, new List<TileType> { TileType.Empty } },
            { Direction.Down, new List<TileType> { TileType.Empty } },
            { Direction.Left, new List<TileType> { TileType.Empty } },
            { Direction.Right, new List<TileType> { TileType.Empty } },
            { Direction.TopLeft, new List<TileType> { TileType.Empty } },
            { Direction.TopRight, new List<TileType> { TileType.Empty } },
            { Direction.BottomLeft, new List<TileType> { TileType.Empty } },
            { Direction.BottomRight, new List<TileType> { TileType.Empty } }
        }},
        { TileType.Ground, new Dictionary<Direction, List<TileType>> {
            { Direction.Up, new List<TileType> { TileType.Ground, TileType.Wall, TileType.Water } },
            { Direction.Down, new List<TileType> { TileType.Ground, TileType.Wall, TileType.Water } },
            { Direction.Left, new List<TileType> { TileType.Ground, TileType.Wall, TileType.Water } },
            { Direction.Right, new List<TileType> { TileType.Ground, TileType.Wall, TileType.Water } },
            { Direction.TopLeft, new List<TileType> { TileType.Ground, TileType.Wall, TileType.Water } },
            { Direction.TopRight, new List<TileType> { TileType.Ground, TileType.Wall, TileType.Water } },
            { Direction.BottomLeft, new List<TileType> { TileType.Ground, TileType.Wall, TileType.Water } },
            { Direction.BottomRight, new List<TileType> { TileType.Ground, TileType.Wall, TileType.Water } }
        }},
        { TileType.Wall, new Dictionary<Direction, List<TileType>> {
            { Direction.Up, new List<TileType> { TileType.Wall, TileType.Ground } },
            { Direction.Down, new List<TileType> { TileType.Wall, TileType.Ground } },
            { Direction.Left, new List<TileType> { TileType.Wall, TileType.Ground } },
            { Direction.Right, new List<TileType> { TileType.Wall, TileType.Ground } },
            { Direction.TopLeft, new List<TileType> { TileType.Wall, TileType.Ground } },
            { Direction.TopRight, new List<TileType> { TileType.Wall, TileType.Ground } },
            { Direction.BottomLeft, new List<TileType> { TileType.Wall, TileType.Ground } },
            { Direction.BottomRight, new List<TileType> { TileType.Wall, TileType.Ground } }
        }},
        { TileType.Water, new Dictionary<Direction, List<TileType>> {
            { Direction.Up, new List<TileType> { TileType.Water, TileType.Ground } },
            { Direction.Down, new List<TileType> { TileType.Water, TileType.Ground } },
            { Direction.Left, new List<TileType> { TileType.Water, TileType.Ground } },
            { Direction.Right, new List<TileType> { TileType.Water, TileType.Ground } },
            { Direction.TopLeft, new List<TileType> { TileType.Water, TileType.Ground } },
            { Direction.TopRight, new List<TileType> { TileType.Water, TileType.Ground } },
            { Direction.BottomLeft, new List<TileType> { TileType.Water, TileType.Ground } },
            { Direction.BottomRight, new List<TileType> { TileType.Water, TileType.Ground } }
        }}
    };
}

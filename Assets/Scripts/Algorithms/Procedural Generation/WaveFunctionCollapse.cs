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
    private static Dictionary<string, Dictionary<Direction, HashSet<string>>> Constraints;

    // Reusable directions array for 8 directions
    private static readonly int[][] directions = new int[][]
    {
        new int[] { 0, 1 },  // Up
        new int[] { 0, -1 }, // Down
        new int[] { -1, 0 }, // Left
        new int[] { 1, 0 },  // Right
    };

    /// <summary>
    /// Returns an n x m grid of procedurally chosen tiles. Uses WFC to determine logical tile arrangement.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="m"></param>
    /// <param name="ruleTiles"></param>
    public static TileBase[][] GenerateSpriteArray(int n, int m, Tilemap exampleTilemap)
    {
        Constraints = BuildAdjacencyConstraints(exampleTilemap);

        string[] possibleSprites = Constraints.Keys.ToArray();
        WaveCell[][] waveArray = GenerateWaveArray(n, m, possibleSprites);

        Stack<WaveCell> cells = new Stack<WaveCell>();
        var newLowest = GetLowestEntropyCell(n, m, waveArray);
        if (newLowest != null)
        {
            cells.Push(newLowest);
        }

        while (cells.Count > 0)
        {
            WaveCell currentCell = cells.Pop();
            CollapseWaveCell(currentCell, waveArray);

            // Push new lowest entropy onto the stack
            newLowest = GetLowestEntropyCell(n, m, waveArray);
            if (newLowest != null)
            {
                cells.Push(newLowest);
            }
        }

        // Create a new TileBase array to store the generated tile arrangement
        TileBase[][] result = new TileBase[n][];
        for (int i = 0; i < n; i++)
        {
            result[i] = new TileBase[m];
            for (int j = 0; j < m; j++)
            {
                string spriteName = waveArray[i][j].sprite;
                result[i][j] = spriteName == "NULL" ? null : GetTileByName(exampleTilemap, spriteName);
            }
        }

        return result;
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

    // Generate wave array
    private static WaveCell[][] GenerateWaveArray(int n, int m, string[] possibleSprites)
    {
        WaveCell[][] waveArray = new WaveCell[n][];
        for (int i = 0; i < n; i++)
        {
            waveArray[i] = new WaveCell[m];
            for (int j = 0; j < m; j++)
            {
                waveArray[i][j] = new WaveCell(i, j, possibleSprites.ToArray());
            }
        }

        return waveArray;
    }

    private static void CollapseWaveCell(WaveCell currentCell, WaveCell[][] waveArray)
    {
        // Filter possible sprites based on constraints from neighboring cells
        List<string> validSprites = new List<string>(currentCell.possibleSprites);

        foreach (var dir in directions)
        {
            int newX = currentCell.x + dir[0];
            int newY = currentCell.y + dir[1];

            if (newX >= 0 && newX < waveArray.Length && newY >= 0 && newY < waveArray[0].Length && waveArray[newX][newY].collapsed)
            {
                string neighborSprite = waveArray[newX][newY].sprite;
                Direction direction = GetDirection(currentCell.x, currentCell.y, newX, newY);
                validSprites = validSprites.Intersect(Constraints[neighborSprite][Opposite(direction)]).ToList();
            }
        }

        // Determine what this cell is allowed to be based on possibilities and weights
        currentCell.sprite = validSprites[UnityEngine.Random.Range(0, validSprites.Count)];

        // The cell has been determined, there are no more possibilities
        currentCell.possibleSprites = null;

        // Collapse the cell
        currentCell.collapsed = true;

        // Update neighbors' entropies
        PropagateConstraints(currentCell, waveArray);
    }

    // Adjacency constraint propagation
    private static void PropagateConstraints(WaveCell currentCell, WaveCell[][] waveArray)
    {
        foreach (var dir in directions)
        {
            int newX = currentCell.x + dir[0];
            int newY = currentCell.y + dir[1];

            if (newX >= 0 && newX < waveArray.Length && newY >= 0 && newY < waveArray[0].Length && !waveArray[newX][newY].collapsed)
            {
                UpdateEntropy(waveArray[newX][newY], waveArray, currentCell);
            }
        }
    }

    private static void UpdateEntropy(WaveCell neighborCell, WaveCell[][] waveArray, WaveCell currentCell)
    {
        if (neighborCell.possibleSprites == null)
        {
            return;
        }

        HashSet<string> allowedSprites = new HashSet<string>(neighborCell.possibleSprites);

        foreach (var dir in directions)
        {
            int newX = neighborCell.x + dir[0];
            int newY = neighborCell.y + dir[1];

            if (newX >= 0 && newX < waveArray.Length && newY >= 0 && newY < waveArray[0].Length && waveArray[newX][newY].collapsed)
            {
                string neighborSprite = waveArray[newX][newY].sprite;
                Direction direction = GetDirection(neighborCell.x, neighborCell.y, newX, newY);
                allowedSprites.IntersectWith(Constraints[neighborSprite][Opposite(direction)]);
            }
        }

        neighborCell.possibleSprites = allowedSprites.ToArray();
    }

    private static Dictionary<string, Dictionary<Direction, HashSet<string>>> BuildAdjacencyConstraints(Tilemap exampleTilemap)
    {
        var adjacencyConstraints = new Dictionary<string, Dictionary<Direction, HashSet<string>>>();
        BoundsInt bounds = exampleTilemap.cellBounds;
        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                TileBase centerTile = exampleTilemap.GetTile(new Vector3Int(x, y, 0));
                if (centerTile == null) continue;

                string centerTileName = centerTile.name;

                if (!adjacencyConstraints.ContainsKey(centerTileName))
                {
                    adjacencyConstraints[centerTileName] = new Dictionary<Direction, HashSet<string>>();
                    foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                    {
                        adjacencyConstraints[centerTileName][dir] = new HashSet<string>();
                    }
                }

                AddConstraint(adjacencyConstraints, centerTileName, Direction.Up, exampleTilemap.GetTile(new Vector3Int(x, y + 1, 0))?.name);
                AddConstraint(adjacencyConstraints, centerTileName, Direction.Down, exampleTilemap.GetTile(new Vector3Int(x, y - 1, 0))?.name);
                AddConstraint(adjacencyConstraints, centerTileName, Direction.Left, exampleTilemap.GetTile(new Vector3Int(x - 1, y, 0))?.name);
                AddConstraint(adjacencyConstraints, centerTileName, Direction.Right, exampleTilemap.GetTile(new Vector3Int(x + 1, y, 0))?.name);
            }
        }

        return adjacencyConstraints;
    }

    private static void AddConstraint(Dictionary<string, Dictionary<Direction, HashSet<string>>> constraints, string centerTileName, Direction direction, string adjacentTileName)
    {
        constraints[centerTileName][direction].Add(adjacentTileName ?? "NULL");
    }

    private static Direction GetDirection(int x1, int y1, int x2, int y2)
    {
        if (x1 == x2 && y1 == y2 - 1) return Direction.Up;
        if (x1 == x2 && y1 == y2 + 1) return Direction.Down;
        if (x1 == x2 + 1 && y1 == y2) return Direction.Left;
        if (x1 == x2 - 1 && y1 == y2) return Direction.Right;

        throw new ArgumentException("Invalid direction");
    }

    private static Direction Opposite(Direction dir) => dir switch
    {
        Direction.Up => Direction.Down,
        Direction.Down => Direction.Up,
        Direction.Left => Direction.Right,
        Direction.Right => Direction.Left,
        _ => dir
    };

    // Helper method to get a TileBase by its name from the example Tilemap
    private static TileBase GetTileByName(Tilemap tilemap, string name)
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        foreach (TileBase tile in allTiles)
        {
            if (tile != null && tile.name == name)
            {
                return tile;
            }
        }

        return null;
    }

    private class WaveCell
    {
        public WaveCell(int x, int y, string[] possibleSprites)
        {
            this.possibleSprites = possibleSprites;
            this.x = x;
            this.y = y;
            collapsed = false;
        }

        public int GetEntropy()
        {
            if (possibleSprites == null) return 0;
            return possibleSprites.Length;
        }

        public string sprite;
        public string[] possibleSprites;
        public int x;
        public int y;
        public bool collapsed;
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
}

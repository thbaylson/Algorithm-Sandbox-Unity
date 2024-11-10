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
        public WaveCell(int x, int y, int maxEntropy)
        {
            this.x = x;
            this.y = y;
            entropy = maxEntropy;
            collapsed = false;
        }

        public TileBase tile;

        public int x;
        public int y;

        public int entropy;
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
        int maxEntropy = ruleTiles.Length;
        WaveCell[][] waveArray = GenerateWaveArray(n, m, maxEntropy);
        
        Stack<WaveCell> cells = new Stack<WaveCell>();
        var newLowest = GetLowestEntropyCell(n, m, waveArray);
        if (newLowest != null)
        {
            cells.Push(newLowest);
        }

        int cnt = 0; // Quick and dirty stopping condition
        while (cells.Count > 0 && cnt < 10000)
        {
            WaveCell currentCell = cells.Pop();
            CollapseWaveCell(currentCell, ruleTiles);

            // Update neighbors' entropies
            for (int i = currentCell.x - 1; i <= currentCell.x + 1 && i != currentCell.x; i++)
            {
                for (int j = currentCell.y - 1; j <= currentCell.y + 1 && j != currentCell.y; j++)
                {
                    // Search the neightbors and propagate information
                }
            }

            // Push new lowest entropy onto the stack
            newLowest = GetLowestEntropyCell(n, m, waveArray);
            if (newLowest != null)
            {
                cells.Push(newLowest);
            }
            cnt++;
        }

        if(cnt >= 10000)
        {
            Debug.Log("Stopped due to count safeguard");
        }

        return waveArray.Select(cols => cols.Select(cell => cell.tile).ToArray()).ToArray();
    }

    private static WaveCell GetLowestEntropyCell(int n, int m, WaveCell[][] waveArray)
    {
        // Get lowest, non-zero entropy
        int lowestEntropy = waveArray[0][0].entropy;
        int currentEntropy;
        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                currentEntropy = waveArray[i][j].entropy;
                if (lowestEntropy*currentEntropy != 0)
                {
                    lowestEntropy = lowestEntropy < currentEntropy ? lowestEntropy : currentEntropy;
                }
            }
        }
        
        Debug.Log($"Lowest entropy: {lowestEntropy}.");
        List<WaveCell> lowestEntropyCells = new List<WaveCell>();
        if (lowestEntropy > 0)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (!waveArray[i][j].collapsed && waveArray[i][j].entropy == lowestEntropy)
                    {
                        Debug.Log($"Lowest (or tied for lowest) entropy: {waveArray[i][j].entropy}; Cell: {waveArray[i][j].x}, {waveArray[i][j].y}.");
                        lowestEntropyCells.Add(waveArray[i][j]);
                    }
                }
            }

            // Choose randomly among the lowest entropy cells
            return lowestEntropyCells[Random.Range(0, lowestEntropyCells.Count)];
        }
        return null;
    }

    private static void CollapseWaveCell(WaveCell currentCell, RuleTile[] ruleTiles)
    {
        // Determine what this cell is allowed to be
        currentCell.tile = ruleTiles[Random.Range(0, ruleTiles.Length)];

        // Set entropy to 0
        currentCell.entropy = 0;

        // Collapse the cell
        currentCell.collapsed = true;
    }

    // Generate wave array
    private static WaveCell[][] GenerateWaveArray(int n, int m, int maxEntropy)
    {
        WaveCell[][] tileBases = new WaveCell[n][];
        for (int i = 0; i < n; i++)
        {
            tileBases[i] = new WaveCell[m];
            for (int j = 0; j < m; j++)
            {
                tileBases[i][j] = new WaveCell(i, j, maxEntropy);
            }
        }

        return tileBases;
    }

    // Generate output

    // Adjacency constraint propagation
}

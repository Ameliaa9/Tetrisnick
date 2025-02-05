using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class tetrisGrid : MonoBehaviour
{
    public int width = 10;
    public int height = 20;

    public Transform[,] grid;
    public Transform[,] debugGrid;

    TetrisManager tetrisManager;
    // Start is called before the first frame update
    void Start()
    {
        tetrisManager = FindObjectOfType<TetrisManager>();


        grid = new Transform[width, height + 4];
        debugGrid = new Transform[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject cell = new GameObject($"Cell ({i}, {j})");
                cell.transform.position = new Vector3(i, j, 0);
                debugGrid[i, j] = cell.transform;
            }
        }
    }
    

    public void AddBlockToGrid(Transform block, Vector2Int position)
   
    {
       grid[position.x, position.y] = block;
    }
    
    public bool IsCellOccupied(Vector2Int position)
    {
        if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height + 4)
        {
            return true; //out of bounds
        }
        return grid[position.x, position.y] != null;
    }

    //checks cells in a line to seee if they are all occupied
    public bool IsLineFull(int rowNumber)
    {
        for(int x = 0; x < width; x++)
        {
            if (grid[x, rowNumber] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void ClearLine(int rowNumber)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, rowNumber].gameObject); //destroy gameobject in cell(s)
            grid[x, rowNumber] = null; //remove reference in grid
        }
    }

    public void ClearFullLines()
    {
        int linesCleared = 0;
        for (int y = 0; y < height; y++ )
        {
            if (IsLineFull(y))
            {
                ClearLine(y);
                ShiftRowsDown(y);
                y--; //recheck the current row after shifting
                linesCleared++;
            }
        }
        if (linesCleared > 0)
        {
            tetrisManager.CalculateScore(linesCleared);
        } 
    }

    //moves blocks that are aboce a line being cleared down 1
    public void ShiftRowsDown(int clearedRow) 
    {
        for(int y = clearedRow; y < height -1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = grid[x, y + 1];
                if (grid[x, y] != null)
                {
                    grid[x, y].position += Vector3.down;
                }
                grid[x, y + 1] = null;
            }
        }

    }

    void OnDrawGizmos()
    {

        //draw red sphere at the transform position
        Gizmos.color = Color.black;
        if (debugGrid != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Gizmos.DrawWireCube(debugGrid[i, j].position, Vector3.one);
                }
            }
        }

    }
}
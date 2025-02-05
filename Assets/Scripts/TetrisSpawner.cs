using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs;
    private tetrisGrid grid;
    private GameObject nextPiece;



    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<tetrisGrid>();
        if (grid == null)
        {
            //error out here
            return;
        }
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        //calculate top center of grid, and spawn there
        Vector3 spawnPosition = new Vector3(
            Mathf.Floor(grid.width / 2),//X position, horizontal center of the grid
            grid.height,              // Y position
            0);                           // Z position

        if (nextPiece != null)
        {
            nextPiece.SetActive(true);

            nextPiece.transform.position = spawnPosition;   
        }
        else
        {
            nextPiece = InstantiateRandomPiece();


            nextPiece.transform.position = spawnPosition;
        }
       nextPiece = InstantiateRandomPiece();
        nextPiece.SetActive(false); //deactiveate until its the next piece 


    }

    public GameObject InstantiateRandomPiece()
    {
        int index = Random.Range(0, tetrominoPrefabs.Length);
        return Instantiate(tetrominoPrefabs[index]);
    }
}

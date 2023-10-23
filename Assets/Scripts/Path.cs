using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    private List<GameObject> path = new List<GameObject>();
    private List<GameObject> topTiles = new List<GameObject>();
    private List<GameObject> bottomTiles = new List<GameObject>();
    [SerializeField] private GameObject pathPrefab;


    private int radius;
    private int currentTileIndex;

    private bool hasReachedX;
    private bool hasReachedY;

    private GameObject startingTile;
    private GameObject endingTile;

    public List<GameObject> GetPath()
    {
        return path;
    }

    public Path(int radius)
    {
        this.radius = radius;
    }

    public void showPath()
    {
        foreach (GameObject tile in path)
        {
            Instantiate(pathPrefab, tile.transform.position, Quaternion.identity);
        }
    }

    public void AssignTopAndBottomTiles(int x, GameObject tile)
    {
        if (x == 0)
        {
            topTiles.Add(tile);
        }
        if (x == radius - 1)
        {
            bottomTiles.Add(tile);
        }
    }

    private bool AssignAndCheckStartingAndEndingTile()
    {
        int xIndex = Random.Range(0, topTiles.Count - 1);
        int yIndex = Random.Range(0, bottomTiles.Count - 1);

        startingTile = topTiles[xIndex];
        endingTile = bottomTiles[yIndex];

        return startingTile != null && endingTile != null;
    }

    public void GeneratePath(GameObject pathPrefab)
    {
        this.pathPrefab = pathPrefab;
        if (AssignAndCheckStartingAndEndingTile())
        {
            GameObject currentTile = startingTile;

            for (int i = 0; i < 3; i++)
            {
                MoveLeft(ref currentTile);
            }

            var safityBreakX = 0;
            var safityBreakY = 0;

            while (!hasReachedX || !hasReachedY)
            {
                int rand = Random.Range(1, 3);
                Debug.Log(rand);

                if (safityBreakX > 100) break;
                if (currentTile.transform.position.y > 0)
                {
                    if (currentTile.transform.position.x > endingTile.transform.position.x)
                        MoveDown(ref currentTile);
                    else if (currentTile.transform.position.x < endingTile.transform.position.x)
                        MoveUp(ref currentTile);
                    else
                        hasReachedX = true;

                    if (safityBreakY > 100) break;

                }

                if (currentTile.transform.position.x > 0)
                {
                    if (currentTile.transform.position.y > endingTile.transform.position.y)
                        MoveRight(ref currentTile);
                    else if (currentTile.transform.position.y < endingTile.transform.position.y)
                        MoveLeft(ref currentTile);
                    else
                        hasReachedY = true;
                }

            }

            // while (!hasReachedY)
            // {

            // }
            path.Add(endingTile);
        }
    }

    private void MoveDown(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GenerateGrid.GeneratedTiles.IndexOf(currentTile);
        int n = currentTileIndex - radius;
        currentTile = GenerateGrid.GeneratedTiles[n];
    }

    private void MoveUp(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GenerateGrid.GeneratedTiles.IndexOf(currentTile);
        int n = currentTileIndex + radius;
        currentTile = GenerateGrid.GeneratedTiles[n];
    }

    private void MoveLeft(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GenerateGrid.GeneratedTiles.IndexOf(currentTile);
        int n = currentTileIndex + 1;
        currentTile = GenerateGrid.GeneratedTiles[n];
    }
    private void MoveRight(ref GameObject currentTile)
    {
        path.Add(currentTile);
        currentTileIndex = GenerateGrid.GeneratedTiles.IndexOf(currentTile);
        int n = currentTileIndex - 1;
        currentTile = GenerateGrid.GeneratedTiles[n];
    }

}

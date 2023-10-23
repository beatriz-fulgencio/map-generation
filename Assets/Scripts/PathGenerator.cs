using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{

    public int radius = 10;

    public static List<Node> GeneratedTiles = new List<Node>();
    private List<Node> path = new List<Node>();


    [SerializeField] private Sprite notpath, emptyTile, upLeft, upRight, downLeft, downRight;

    [SerializeField] public Node tileNode;
    [SerializeField] private int numberPaths;

    private Sprite spriteToUse;


    private enum CurrentDirection
    {
        LEFT,
        RIGHT,
        DOWN,
        UP
    };

    private CurrentDirection curDirection = CurrentDirection.DOWN;


    // Start is called before the first frame update
    void Awake()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < radius; x++)
        {
            for (int y = 0; y < radius; y++)
            {
                GameObject tile = Instantiate(tileNode.tilePrefab, new Vector2(x * 2, y * 2), Quaternion.identity);

                GeneratedTiles.Add(new Node(tile, y, x));
                tileNode.tilePrefab.GetComponent<SpriteRenderer>().sprite = notpath;

            }
        }

        ConstructGraph();

        tileNode.tilePrefab.GetComponent<SpriteRenderer>().sprite = emptyTile;
        GeneratePath();
        //     Node startingTile = null;
        //     Node endingTile = null;
        //     IsGeneratedBeginningAndEnd(ref startingTile, ref endingTile);
        //     GeneratePath(ref startingTile, ref endingTile, 2);
        //    // GeneratePath(ref startingTile, ref endingTile, 3);
        //     //GeneratePath(ref startingTile, ref endingTile, 4);
        //     showPath();

    }
    private void ConstructGraph()
    {
        for (int i = 0; i < GeneratedTiles.Count; i++)
        {
            if (GeneratedTiles[i].y != 0)
            {
                //Debug.Log("bottom");
                GeneratedTiles[i].adjecent.Add(GeneratedTiles[i - radius]);
            }
            //Debug.Log(GeneratedTiles[i].x);
            if (GeneratedTiles[i].x != 0)
            {
                //Debug.Log("left");
                GeneratedTiles[i].adjecent.Add(GeneratedTiles[i - 1]);
            }

            if (GeneratedTiles[i].y != radius - 1)
            {
                //Debug.Log("top");
                GeneratedTiles[i].adjecent.Add(GeneratedTiles[i + radius]);
            }

            if (GeneratedTiles[i].x != radius - 1)
            {
                //Debug.Log("right");
                GeneratedTiles[i].adjecent.Add(GeneratedTiles[i + 1]);
            }


        }
    }

    // public bool IsGeneratedBeginningAndEnd(ref Node startingTile, ref Node endingTile)
    // {
    //     int xIndex = Random.Range(0, radius - 1);
    //     int yIndex = Random.Range(radius * radius - radius, radius * radius - 1);

    //     startingTile = GeneratedTiles[yIndex];
    //     endingTile = GeneratedTiles[xIndex];

    //     return startingTile != null && endingTile != null;
    // }

    // public void GeneratePath(ref Node startingTile, ref Node endingTile, int iteration)
    // {
    //     // Node startingTile = null;
    //     // Node endingTile = null;


    //     int c = 0;
    //     Node currentTile = startingTile;
    //     Node prevtile = startingTile;
    //     while (currentTile.y > endingTile.y)
    //     {
    //         path.Add(currentTile);
    //         currentTile.isPath = true;
    //         Debug.Log("x = " + currentTile.x + " /  y= " + currentTile.y);
    //         c++;
    //         int move = Random.Range(0, currentTile.adjecent.Count);
    //         if (c % iteration == 0)
    //         {
    //             move = 0;
    //         }
    //         if (currentTile.adjecent[move].canRecieveTower == false)
    //         {
    //             currentTile = currentTile.adjecent[move];
    //             currentTile.isPath = true;
    //             foreach (var tile in prevtile.adjecent)
    //             {
    //                 if (tile.isPath == false)
    //                 {
    //                     tile.canRecieveTower = true;
    //                     Debug.Log("Canot be path: x = " + tile.x + " /  y= " + tile.y);
    //                 }
    //             }

    //         }
    //         //prevtile = currentTile;

    //     }
    //     while (currentTile != endingTile)
    //     {
    //         path.Add(currentTile);
    //         if (currentTile.x < endingTile.x)
    //         {
    //             if (currentTile.x >0) currentTile = currentTile.adjecent[2];
    //             else currentTile = currentTile.adjecent[1];
    //         }
    //         else
    //         {
    //             currentTile = currentTile.adjecent[0];
    //         }
    //     }

    //     path.Add(currentTile);


    //     // // Node currentTile = startingTile;
    //     // // path.Add(currentTile);
    //     // // int breakWhile = 0;
    //     // // while (currentTile != endingTile || breakWhile!= 10000)
    //     // // {
    //     // //     // currentTile.isPath = true;
    //     // //     // Debug.Log(path.Count);
    //     // //     // int rand = -1;
    //     // //     // do
    //     // //     // {
    //     // //     //     rand = Random.Range(beggin, end);
    //     // //     //     switch (rand)
    //     // //     //     {
    //     // //     //         case 0://top
    //     // //     //             if (currentTile.top != null && !currentTile.top.isPath)
    //     // //     //             {
    //     // //     //                 currentTile = currentTile.top;
    //     // //     //             }
    //     // //     //             else
    //     // //     //             {
    //     // //     //                 beggin=1;
    //     // //     //             }
    //     // //     //             break;
    //     // //     //         case 1://right
    //     // //     //             if (currentTile.right != null && !currentTile.right.isPath)
    //     // //     //             {
    //     // //     //                 currentTile = currentTile.right;
    //     // //     //             }
    //     // //     //             else
    //     // //     //             {
    //     // //     //                 rand = -1;
    //     // //     //             }
    //     // //     //             break;
    //     // //     //         case 2://bottom
    //     // //     //             if (currentTile.bottom != null && !currentTile.bottom.isPath)
    //     // //     //             {
    //     // //     //                 currentTile = currentTile.bottom;
    //     // //     //             }
    //     // //     //             else
    //     // //     //             {
    //     // //     //                 rand = -1;
    //     // //     //             }
    //     // //     //             break;
    //     // //     //         case 3://left
    //     // //     //             if (currentTile.left != null && !currentTile.left.isPath)
    //     // //     //             {
    //     // //     //                 currentTile = currentTile.left;
    //     // //     //             }
    //     // //     //             else
    //     // //     //             {
    //     // //     //                 rand = -1;
    //     // //     //             }
    //     // //     //             break;
    //     // //     //     }
    //     // //     // } while (rand == -1);
    //     // //     // path.Add(currentTile);
    //     // //     // breakWhile++;


    //     // // }
    //     // Debug.Log("starting at" + startingTile.x + " " + startingTile.y);

    //     // bool stop = false;
    //     // for (int i = 0; i < GeneratedTiles.Count; i++)
    //     // {
    //     //     if (stop) continue;
    //     //     //if (GeneratedTiles[i] == endingTile) continue;
    //     //     if (!GeneratedTiles[i].isPath)
    //     //     {
    //     //         stop = DFS(startingTile, endingTile, stop);
    //     //         //nonRecursiveDFS(graph.get(i), visited);
    //     //     }
    //     // }
    //     // Debug.Log("ending at" + endingTile.x + " " + endingTile.y);





    // }

    // // private bool DFS(Node node, Node node2, bool stop)
    // // {
    // //     if (stop) return true;
    // //     node.isPath = true;
    // //     path.Add(node);

    // //     Debug.Log(node.x + " " + node.y);

    // //     foreach (Node n in node.adjecent)
    // //     {

    // //         if (n && !n.isPath)
    // //         {
    // //             n.isPath = true;
    // //             if (!(n == node2))
    // //             {
    // //                 stop = DFS(n, node2, (n == node2));
    // //             }
    // //             else
    // //             {
    // //                 path.Add(n);
    // //                 return true;
    // //             }
    // //         }
    // //         if (stop) return true;
    // //     }
    // //     return false;
    // // }

    private void GeneratePath()
    {
        spriteToUse = emptyTile;
    }
    public void showPath()
    {

        foreach (Node tile in path)
        {
            Instantiate(tileNode.tilePrefab, new Vector2(tile.x * 2, tile.y * 2), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

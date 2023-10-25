using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite emptyTile, pathTile, entryTile;
    [SerializeField] public Node tileNode;

    [SerializeField] public GameObject tilePrefab;

    [SerializeField] private GameObject tree, flower, grass, grass2, tower;


    private int radius = 24;

    private MatrixGraph matrix;

    private List<Node> Q1, Q2, Q3, Q4;

    private Node source, destination;

    private List<Node> requiredVertices = new List<Node>();
    private List<List<Node>> path = new List<List<Node>>();

    public List<List<Transform>> path_transform = new List<List<Transform>>();


    // start is called before the first frame update
    void Awake()
    {
        tileNode.tile = emptyTile;

        matrix = new MatrixGraph(radius);

        Q1 = matrix.getQuadrant(1);
        Q2 = matrix.getQuadrant(2);
        Q3 = matrix.getQuadrant(3);
        Q4 = matrix.getQuadrant(4);

        sortSourceAndDestination();
        sortQVertex();

        //List<Node> path = BreadthFirstPaths.BFS(source, requiredVertices, destination);

        //path1.AddRange(BreadthFirstPaths.BFS(requiredVertices[0], requiredVertices, requiredVertices[1]));
        //path.AddRange(BreadthFirstPaths.BFS(requiredVertices[1], requiredVertices, destination));
        //path.AddRange(BreadthFirstPaths.BFS(requiredVertices[2], requiredVertices, requiredVertices[2]));
        //path.AddRange(BreadthFirstPaths.BFS(requiredVertices[3], requiredVertices, destination));

        List<Node> path1 = new List<Node>();
        move(source, requiredVertices[0],path1);
        //move(requiredVertices[0], requiredVertices[1]);
        move(requiredVertices[0], requiredVertices[3],path1);
        move(requiredVertices[3], destination,path1);

        path.Add(path1);
        requiredVertices = new List<Node>();

        sortQVertex();

        List<Node> path2 = new List<Node>();

        move2(source, requiredVertices[1], path2);
        move2(requiredVertices[1], requiredVertices[2], path2);
        move2(requiredVertices[2], destination, path2);

         path.Add(path2);

        findPossibleTowerPlaces();

        printMap();

        // foreach (var item in path)
        // {
        //     printPath(item);
        // }
        printPath(path[0]);
        printPath(path[1]);
    }

    public void printMap()
    {
        Node tileToPrint = matrix.head.bottom;
        for (int i = 0; i < radius; i++)
        {
            Node firstPosition = tileToPrint;

            for (int j = 0; j < radius; j++)
            {
                // if (tileToPrint.isPath)
                // {
                //     if (!tileToPrint.isEntry) tilePrefab.GetComponent<SpriteRenderer>().sprite = pathTile;
                //     else
                //     {
                //         tilePrefab.GetComponent<SpriteRenderer>().sprite = entryTile;
                //     }

                //     GameObject tile = Instantiate(tilePrefab, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2)), Quaternion.identity);
                //     path_transform.Add(tile.transform);
                // }
                // else
                if(!tileToPrint.isPath){
                    tilePrefab.GetComponent<SpriteRenderer>().sprite = emptyTile;

                    GameObject tile = Instantiate(tilePrefab, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2)), Quaternion.identity);
                    if (!tileToPrint.canRecieveTower)
                    {
                        int x = Random.Range(0, 10);
                        if (x % 2 == 0)
                        {
                            Instantiate(flower, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2) + (float)0.1), Quaternion.identity);
                        }
                        else if (x % 3 == 0)
                        {
                            Instantiate(grass, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2) + (float)0.1), Quaternion.identity);
                        }
                        else if (x % 5 == 0 || x % 7 == 0)
                        {
                            Instantiate(tree, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2) + (float)+1), Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(grass2, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2) + (float)0.1), Quaternion.identity);
                        }
                    }

                }



                // GameObject tile = Instantiate(tilePrefab, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2)), Quaternion.identity);
                tileToPrint = tileToPrint.right;
            }


            tileToPrint = firstPosition.bottom;
        }

        tilePrefab.GetComponent<SpriteRenderer>().sprite = emptyTile;
        for (int j = 0; j < radius; j++)
        {
            Instantiate(tilePrefab, new Vector2(-2, -(j * 2)), Quaternion.identity);
            Instantiate(tilePrefab, new Vector2(radius * 2, -(j * 2)), Quaternion.identity);
        }
    }

    public void printPath(List<Node> path)
    {
        // tilePrefab.GetComponent<SpriteRenderer>().sprite = pathTile;
         List<Transform> path1 = new List<Transform>();

        foreach (Node n in path)
        {
           
            if (!n.isEntry) tilePrefab.GetComponent<SpriteRenderer>().sprite = pathTile;
            else
            {
                tilePrefab.GetComponent<SpriteRenderer>().sprite = entryTile;
            }

            GameObject tile = Instantiate(tilePrefab, new Vector2(n.x * 2, -(n.y * 2)), Quaternion.identity);
            path1.Add(tile.transform);
        }

        path_transform.Add(path1);
    }

    public void sortSourceAndDestination()
    {
        int x;
        x = Random.Range(0, matrix.n);
        source = Q1[x];

        x = Random.Range(Q4.Count - matrix.n, Q4.Count - 1);
        destination = Q4[x];

        source.isEntry = destination.isEntry = true;
        source.isPath = destination.isPath = true;
    }

    public void sortQVertex()
    {
        int pos;
        pos = Random.Range(radius, Q1.Count - 1);
        this.requiredVertices.Add(Q1[pos]);

        pos = Random.Range(radius, Q2.Count - 1);
        this.requiredVertices.Add(Q2[pos]);

        pos = Random.Range(0, Q3.Count - 1);
        this.requiredVertices.Add(Q3[pos]);

        pos = Random.Range(0, Q4.Count - 1);
        this.requiredVertices.Add(Q4[pos]);

        // foreach (Node n in requiredVertices)
        // {
        //     n.isPath = true;
        // }
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<Node> findPossibleTowerPlaces()
    {
        List<Node> possibleTowerPlace = new List<Node>();
        Node tileToPrint = matrix.head.bottom;
        for (int i = 0; i < radius; i++)
        {
            Node firstPosition = tileToPrint;

            for (int j = 0; j < radius; j++)
            {
                if (tileToPrint.isPath == false && tileToPrint.left && tileToPrint.left.isPath)
                {
                    tileToPrint.canRecieveTower = true;
                }
                else if (tileToPrint.isPath == false && tileToPrint.right && tileToPrint.right.isPath)
                {
                    tileToPrint.canRecieveTower = true;
                }
                else if (tileToPrint.isPath == false && tileToPrint.top && tileToPrint.top.isPath)
                {
                    tileToPrint.canRecieveTower = true;
                }
                else if (tileToPrint.isPath == false && tileToPrint.bottom && tileToPrint.bottom.isPath)
                {
                    tileToPrint.canRecieveTower = true;
                }

                if (tileToPrint.canRecieveTower)
                {
                    possibleTowerPlace.Add(tileToPrint);
                }
                tileToPrint = tileToPrint.right;
            }
            tileToPrint = firstPosition.bottom;
        }
        //find all nodes adjecent to paths that are not paths
        return possibleTowerPlace;
    }

    // void ShowScenenary()
    // {
    //     //all nodes that are not paths and cant receive towers will have decorations
    // }

    void PutRandomTowers(List<Node> towerPlacements)
    {

        //put towers randomly in the placements available 

    }

    bool move(Node root, Node tail, List<Node> path1)
    {
        //List<Node> path1 = new List<Node>();
        Node current = root;

        bool reachedX = false, reachedY = false;

        while (current != tail)
        {
            Debug.Log(current.x + " -  " + current.y);
            Debug.Log("Tail:" + tail.x + " -  " + tail.y);
            if (!reachedY)
            {
                Debug.Log("Nao y");
                if (current.y < tail.y)
                {
                    current.isPath = true;
                    path1.Add(current);
                    current = current.bottom;
                }
                else if (current.y > tail.y)
                {
                    current.isPath = true;
                    path1.Add(current);

                    current = current.top;
                }
                if (current.y == tail.y)
                {
                    reachedY = true;
                }
            }
            else if (!reachedX)
            {
                Debug.Log("Nao x");
                if (current.x > tail.x)
                {
                    current.isPath = true;
                    path1.Add(current);

                    current = current.left;
                }
                else if (current.x < tail.x)
                {
                    current.isPath = true;
                    path1.Add(current);
                    current = current.right;
                }
                if (current.x == tail.x)
                {
                    reachedX = true;
                }
            }
            if (reachedX && reachedY)
            {
                Debug.Log("Chegou");
                current.isPath = true;
                current = tail;
                path1.Add(current);
                //path.Add(path1);
                return true;
            }

        }
        return false;
    }

    bool move2(Node root, Node tail,List<Node> path1)
    {
        //List<Node> path1 = new List<Node>();
        Node current = root;
        path1.Add(current);
        current.isPath = true;

        bool reachedX = false, reachedY = false;

        while (current != tail)
        {
            Debug.Log(current.x + " -  " + current.y);
            Debug.Log("Tail:" + tail.x + " -  " + tail.y);
            if (!reachedX)
            {
                Debug.Log("Nao x");
                if (current.x > tail.x)
                {
                    current.isPath = true;
                    path1.Add(current);

                    current = current.left;
                }
                else if (current.x < tail.x)
                {
                    current.isPath = true;
                    path1.Add(current);
                    current = current.right;
                }
                if (current.x == tail.x)
                {
                    reachedX = true;
                }
            }
            else if (!reachedY)
            {
                Debug.Log("Nao y");
                if (current.y < tail.y)
                {
                    current.isPath = true;
                    path1.Add(current);
                    current = current.bottom;
                }
                else if (current.y > tail.y)
                {
                    current.isPath = true;
                    path1.Add(current);

                    current = current.top;
                }
                if (current.y == tail.y)
                {
                    reachedY = true;
                }
            }

            if (reachedX && reachedY)
            {
                Debug.Log("Chegou");
                current.isPath = true;
                current = tail;
                path1.Add(current);
                //path.Add(path1);
                return true;
            }

        }
        return false;
    }

}


class MatrixGraph
{

    private int radius;
    public int n;
    private int x;
    public Node head;

    //public Node tail;


    public MatrixGraph(int radius)
    {
        this.radius = radius;

        this.n = (int)radius / 2;
        this.x = (int)n * n;

        //head = head Node 
        head = new Node();
        head.bottom = new Node(0, 0);

        Node columnCurrent = head.bottom, lineCurrent;

        // First line 
        for (int i = 0; i < radius - 1; i++)
        {
            columnCurrent.right = new Node(i + 1, 0);
            columnCurrent.right.left = columnCurrent;
            columnCurrent = columnCurrent.right;
        }

        for (int i = 1; i < radius; i++)
        {
            while (columnCurrent.left != null)
            {
                columnCurrent = columnCurrent.left;
            }

            // next line
            columnCurrent.bottom = new Node(0, i);
            lineCurrent = columnCurrent.bottom;
            lineCurrent.top = columnCurrent;

            // building line`s column
            for (int j = 0; j < radius - 1; j++)
            {
                lineCurrent.right = new Node(j + 1, i);
                lineCurrent.right.left = lineCurrent;

                lineCurrent = lineCurrent.right;
                columnCurrent = columnCurrent.right;

                lineCurrent.top = columnCurrent;
                columnCurrent.bottom = lineCurrent;
            }

            // point to last node in line
            columnCurrent = lineCurrent;
        }

    }

    public Node getQuadrantStart(int q)
    {
        Node ret = head.bottom;
        if (q == 1)
        {
            return ret;
        }
        else if (q == 2)
        {
            while (ret.x != n)
            {
                ret = ret.right;
            }
            return ret;
        }
        else if (q == 3)
        {
            while (ret.y != (n))
            {
                ret = ret.bottom;
            }
            return ret;
        }
        else if (q == 4)
        {
            while (ret.x != n)
            {
                ret = ret.right;
            }
            while (ret.y != (n))
            {
                ret = ret.bottom;
            }
            return ret;
        }

        return null;

    }

    public List<Node> getQuadrant(int q)
    {
        List<Node> list = new List<Node>();
        Node curr = getQuadrantStart(q);
        Node first = curr;

        int count = 0;
        if (curr != null)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    list.Add(curr);
                    curr = curr.right;
                }
                curr = first.bottom;
                first = curr;

            }
        }

        return list;
    }


    // public void fill()
    // {

    // }
}
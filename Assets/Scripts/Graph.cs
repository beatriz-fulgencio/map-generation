using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite emptyTile, pathTile;
    [SerializeField] public Node tileNode;

    [SerializeField] public GameObject tilePrefab;


    private int radius = 12;

    private MatrixGraph matrix;

    private List<Node> Q1, Q2, Q3, Q4;



    // start is called before the first frame update
    void Start()
    {
        tileNode.tile = emptyTile;

        matrix = new MatrixGraph(radius);

        printMap();


    }

    public void printMap()
    {
        Node tileToPrint = matrix.head.bottom;
        for (int i = 0; i < radius; i++)
        {
            Node firstPosition = tileToPrint;

            for (int j = 0; j < radius; j++)
            {
                if (tileToPrint.isPath)
                {
                    tilePrefab.GetComponent<SpriteRenderer>().sprite = pathTile;
                }
                else
                {
                    tilePrefab.GetComponent<SpriteRenderer>().sprite = emptyTile;
                }

                GameObject tile = Instantiate(tilePrefab, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2)), Quaternion.identity);
                tileToPrint = tileToPrint.right;
            }

            tileToPrint = firstPosition.bottom;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

class MatrixGraph
{

    private int radius;
    private int n;
    private int x;
    public Node head;

    //public Node tail;


    public MatrixGraph(int radius)
    {
        this.radius = radius;

        this.n = (int)radius / 2;
        this.x = (int)System.Math.Sqrt(n);

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

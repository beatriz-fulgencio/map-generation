using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Node : ScriptableObject
{
    [SerializeField] public GameObject tilePrefab;

    
    // public Node top = null;
    // public Node bottom = null;
    // public Node left = null;
    // public Node right = null;

    //public Node[] adjecent = new Node[4]; 
    public List<Node> adjecent = new List<Node>();

    public bool canRecieveTower = false;
    public bool canRecieveDecoration;
    public bool isPath = false;

    public int x;
    public int y;

    public Node(GameObject tile, int x, int y){
        this.tilePrefab = tile;
        this.x = x;
        this.y = y;
    }



}

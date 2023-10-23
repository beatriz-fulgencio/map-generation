using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public static List<GameObject> GeneratedTiles = new List<GameObject>();

    [SerializeField] private GameObject tilePrefab;
    //[SerializeField] private GameObject pathPrefab;

    [SerializeField] private Sprite emptyTile, pathTile;
    [SerializeField] public Node tileNode;


    private int radius = 20;

    // Start is called before the first frame update
    void Start()
    {

        Path pathGenerator = new Path(radius);

        tileNode.tilePrefab.GetComponent<SpriteRenderer>().sprite = emptyTile;

        for (int x = 0; x < radius; x++)
        {
            for (int y = 0; y < radius; y++)
            {
                GameObject tile = Instantiate(tileNode.tilePrefab, new Vector2(x*3, y*3), Quaternion.identity);

                GeneratedTiles.Add(tile);
                pathGenerator.AssignTopAndBottomTiles(y, tile);

            }
        }
        tileNode.tilePrefab.GetComponent<SpriteRenderer>().sprite = pathTile;
        pathGenerator.GeneratePath(tileNode.tilePrefab);
        // foreach (var pObject in pathGenerator.GetPath())
        // {
        //     pObject.SetActive(false);

        //      GameObject tile = Instantiate(tileNode.tilePrefab, pObject.transform.position, Quaternion.identity);

        // }
        pathGenerator.showPath();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

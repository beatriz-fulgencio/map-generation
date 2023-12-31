using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int mapWidth, mapHeight;

    [SerializeField] private GameObject tileReference;
    [SerializeField] private Sprite emptyTile, downPath, leftRight, leftDown, rightDown, downLeft, downRight;

    public int[,] path;

    private int curX;
    private int saveX;
    private int curY;
    private int saveY;

    private int prevX;
    private int prevY;
    private Sprite spriteToUse;
    private bool forceDirectionChange = false;

    private bool continueLeft = false;
    private bool continueRight = false;
    private int currentCount = 0;

    private enum CurrentDirection
    {
        LEFT,
        RIGHT,
        DOWN,
        UP
    };
    private CurrentDirection curDirection = CurrentDirection.DOWN;

    public struct TileData
    {
        public Transform transform;
        public SpriteRenderer spriteRenderer;
        public int tileID;
    }

    TileData[,] tileData;

    void Awake()
    {
        tileData = new TileData[mapWidth, mapHeight];
        path = new int[mapWidth * mapHeight, mapHeight * mapWidth];
        InitializePath();
        GenerateMap();
       // Debug.Log(ShowGraph());

    }

    void InitializePath()
    {
        for (int x = mapWidth * mapHeight - 1; x >= 0; x--)
        {
            for (int y = 0; y < mapHeight * mapWidth; y++)
            {
                path[x, y] = 0;
            }
        }
    }

    void GenerateMap()
    {
        for (int x = mapWidth - 1; x >= 0; x--)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float xOffset = x * 2;
                float yOffset = y * 2;
                GameObject newTile = Instantiate(tileReference, new Vector2(xOffset, yOffset), Quaternion.identity);
                tileData[x, y].spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                tileData[x, y].tileID = 0;
                tileData[x, y].spriteRenderer.sprite = emptyTile;
                tileData[x, y].transform = newTile.transform;
            }
        }
        curX = Random.Range(0, mapWidth);
        saveX = curX;
        //StartCoroutine(GeneratePath());
        GeneratePath();
        curX = saveX;
        //GeneratePath();
        SecondPath();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RegenerateMap();
        }
    }

    void RegenerateMap()
    {
        StopAllCoroutines();
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float xOffset = x * 2;
                float yOffset = y * 2;
                tileData[x, y].spriteRenderer.sprite = emptyTile;
                tileData[x, y].tileID = 0;
                tileData[x, y].transform.position = new Vector2(xOffset, yOffset);
            }
        }
        //StartCoroutine(GeneratePath());
        GeneratePath();
        curX = saveX;
        //GeneratePath();
    }

    void GeneratePath()
    {
        //curX = Random.Range(0, mapWidth);
        curY = 0;

        spriteToUse = downPath;

        while (curY <= mapHeight - 1)
        {
            CheckCurrentDirections();
            ChooseDirection();

            if (curY <= mapHeight - 1)
            {
                UpdateMap(curX, curY, spriteToUse);
                // float posY = mapVisuals[curX, curY].gameObject.transform.position.y + pathOffset;
                // mapVisuals[curX, curY].gameObject.transform.position = new Vector3
                //     (mapVisuals[curX, curY].gameObject.transform.position.x, posY);

                // mapVisuals[curX, curY].tileType = MapTileData.TileType.PATH;
            }

            if (curDirection == CurrentDirection.DOWN)
            {
                curY++;
            }
            //StartCoroutine(GeneratePath());
            //yield return new WaitForSeconds(0.05f);
        }
        saveY = curY;
    }

    void SecondPath()
    {

    }
    private void CheckCurrentDirections()
    {
        if (curDirection == CurrentDirection.LEFT && curX - 1 >= 0 && tileData[curX - 1, curY].tileID == 0)
        {
            curX--;
        }
        else if (curDirection == CurrentDirection.RIGHT && curX + 1 <= mapWidth - 1 && tileData[curX + 1, curY].tileID == 0)
        {
            curX++;
        }
        else if (curDirection == CurrentDirection.UP && curY - 1 >= 0 && tileData[curX, curY - 1].tileID == 0)
        {
            if (continueLeft && tileData[curX - 1, curY - 1].tileID == 0 ||
            continueRight && tileData[curX + 1, curY - 1].tileID == 0)
            {
                curY--;
            }
            else
            {
                forceDirectionChange = true;
                tileData[curX, curY].transform.position = new Vector2(tileData[curX, curY].transform.position.x, tileData[curX, curY].transform.position.y);
                // mapVisuals[curX, curY].transform.position = new Vector3(
                //             mapVisuals[curX, curY].transform.position.x,
                //             mapVisuals[curX, curY].gameObject.transform.position.y - pathOffset);
            }
        }
        else if (curDirection != CurrentDirection.DOWN)
        {
            forceDirectionChange = true;
            tileData[curX, curY].transform.position = new Vector2(tileData[curX, curY].transform.position.x, tileData[curX, curY].transform.position.y);
            // mapVisuals[curX, curY].transform.position = new Vector3(
            //             mapVisuals[curX, curY].transform.position.x,
            //             mapVisuals[curX, curY].gameObject.transform.position.y - pathOffset);
        }
    }

    private void ChooseDirection()
    {
        if (currentCount < 3 && !forceDirectionChange)
        {
            currentCount++;
        }
        else
        {
            bool chanceToChange = Mathf.FloorToInt(Random.value * 1.99f) == 0;

            if (chanceToChange || forceDirectionChange || currentCount > 7)
            {
                currentCount = 0;
                forceDirectionChange = false;
                ChangeDirection();
            }

            currentCount++;
        }
    }

    private void ChangeDirection()
    {
        int dirValue = Mathf.FloorToInt(Random.value * 2.99f);

        if (dirValue == 0 && curDirection == CurrentDirection.LEFT && curX - 1 > 0
        || dirValue == 0 && curDirection == CurrentDirection.RIGHT && curX + 1 < mapWidth - 1)
        {
            if (curY - 1 >= 0)
            {
                if (tileData[curX, curY - 1].tileID == 0 &&
                tileData[curX - 1, curY - 1].tileID == 0 &&
                tileData[curX + 1, curY - 1].tileID == 0)
                {
                    GoUp();
                    return;
                }
            }
        }

        if (curDirection == CurrentDirection.LEFT)
        {
            UpdateMap(curX, curY, leftDown);
        }
        else if (curDirection == CurrentDirection.RIGHT)
        {
            UpdateMap(curX, curY, rightDown);
        }

        if (curDirection == CurrentDirection.LEFT || curDirection == CurrentDirection.RIGHT)
        {
            curY++;
            spriteToUse = downPath;
            curDirection = CurrentDirection.DOWN;
            return;
        }

        if (curX - 1 > 0 && curX + 1 < mapWidth - 1 || continueLeft || continueRight)
        {
            if (dirValue == 1 && !continueRight || continueLeft)
            {
                if (tileData[curX - 1, curY].tileID == 0)
                {
                    if (continueLeft)
                    {
                        spriteToUse = rightDown;
                        continueLeft = false;
                    }
                    else
                    {
                        spriteToUse = downLeft;
                    }
                    curDirection = CurrentDirection.LEFT;
                }
            }
            else
            {
                if (tileData[curX + 1, curY].tileID == 0)
                {
                    if (continueRight)
                    {
                        continueRight = false;
                        spriteToUse = leftDown;
                    }
                    else
                    {
                        spriteToUse = downRight;
                    }
                    curDirection = CurrentDirection.RIGHT;
                }
            }
        }
        else if (curX - 1 > 0)
        {
            spriteToUse = downLeft;
            curDirection = CurrentDirection.LEFT;
        }
        else if (curX + 1 < mapWidth - 1)
        {
            spriteToUse = downRight;
            curDirection = CurrentDirection.RIGHT;
        }

        if (curDirection == CurrentDirection.LEFT)
        {
            GoLeft();
        }
        else if (curDirection == CurrentDirection.RIGHT)
        {
            GoRight();
        }
    }


    private void GoUp()
    {
        if (curDirection == CurrentDirection.LEFT)
        {
            UpdateMap(curX, curY, downRight);
            continueLeft = true;
        }
        else
        {
            UpdateMap(curX, curY, downLeft);
            continueRight = true;
        }
        curDirection = CurrentDirection.UP;
        curY--;
        spriteToUse = downPath;
    }

    private void GoLeft()
    {
        UpdateMap(curX, curY, spriteToUse);
        curX--;
        spriteToUse = leftRight;
    }

    private void GoRight()
    {
        UpdateMap(curX, curY, spriteToUse);
        curX++;
        spriteToUse = leftRight;
    }

    private void UpdateMap(int mapX, int mapY, Sprite spriteToUse)
    {
        path[prevX * prevY, mapX * mapY] = 1;
        tileData[mapX, mapY].tileID = 1;
        tileData[mapX, mapY].spriteRenderer.sprite = spriteToUse;
        prevX = mapX;
        prevY = mapY;
    }

    string ShowGraph()
    {
        string s = "";
        for (int x = mapWidth * mapHeight - 1; x >= 0; x--)
        {
            for (int y = 0; y < mapHeight * mapWidth; y++)
            {

                s += path[x, y];
            }
            s += "\n";
        }
        return s;
    }

}
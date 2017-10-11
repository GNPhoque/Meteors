using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksGenerator : MonoBehaviour
{

    public GameObject BlockPrefab;

    GameObject[,] blockPositions = new GameObject[10, 25];
    Vector2 screenSpace;
    // Use this for initialization
    void Start()
    {
        Vector3 screenRes = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        screenSpace = new Vector2(screenRes.x, screenRes.y);
        GenerateBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        //print("ScreenSpace : "+screenSpace+"\nMousePosition : "+Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject Block in blockPositions)
            {
                Destroy(Block);
            }
            GenerateBlocks();
        }
    }

    void GenerateBlocks()
    {
        string leftTeam = "";
        string bottomTeam = "";
        for (int j = 0; j <= blockPositions.GetUpperBound(1); j++)
        {
            Vector3 xPos = new Vector3(-screenSpace.x + BlockPrefab.GetComponent<RectTransform>().rect.width / 2, -screenSpace.y + .03f * j + BlockPrefab.GetComponent<RectTransform>().rect.width * (.5f + j), 0);
            for (int i = 0; i <= blockPositions.GetUpperBound(0); i++)
            {
                if (i > 0)
                    leftTeam = blockPositions[i - 1, j].GetComponent<BlockScript>().team;// ?? "";
                if (j > 0)
                    bottomTeam = blockPositions[i, j - 1].GetComponent<BlockScript>().team;// ?? "";
                blockPositions[i, j] = (GameObject)Instantiate(BlockPrefab);
                TeamRandomizer(blockPositions[i, j], leftTeam, bottomTeam);
                blockPositions[i, j].transform.position = xPos;
                xPos += new Vector3(BlockPrefab.GetComponent<RectTransform>().rect.width + .03f, 0, 0);
            }
        }
    }

    void TeamRandomizer(GameObject block, string leftTeam, string bottomTeam)
    {
        List<string> TeamsList = new List<string>() { "Red", "Blue", "Yellow", "Green", "White" };
        if (leftTeam != null)
        {
            TeamsList.Remove(leftTeam);
        }
        if (bottomTeam != null)
        {
            TeamsList.Remove(bottomTeam);
        }
        string res = TeamsList.ElementAt(Random.Range(0, TeamsList.Count));
        block.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(res);
        block.GetComponent<BlockScript>().team = res;
    }
}

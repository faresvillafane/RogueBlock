using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

[ExecuteInEditMode]
public class TileGroupSave : MonoBehaviour
{
    public Material trapMaterial, fixedMaterial, rotatingMaterialTop, rotatingMaterialBot;
    public Vector2 v2Size = Vector2.one * 5;
    private Vector2 prevV2Size;

    [ContextMenuItem("SaveLevel", "SaveSerializedLevel")]
    [ContextMenuItem("ReadLevel", "ReadString")]
    [SerializeField]
    public GameObject prefTile;
    public GameObject prefTrap, prefEnemy, prefEntrance;

    private List<GameObject> goTiles;

    private Transform tParent;

    public bool bBoardOnTop = true;
    private bool bPrevBoardOnTop = true;

    void Start ()
    {
        prevV2Size = v2Size;
        goTiles = new List<GameObject>();
        tParent = GameObject.FindGameObjectWithTag(BSConstants.TAG_LEVEL_EDITOR).transform;


    }

    // Update is called once per frame
    void Update () {
        if (v2Size != prevV2Size)
        {
            GenerateTiles();
            prevV2Size = v2Size;
        }
        if(bBoardOnTop != bPrevBoardOnTop)
        {
            FlipBoard();
            bPrevBoardOnTop = bBoardOnTop;
        }
	}

    private void GenerateTiles()
    {
        if(goTiles.Count > 0)
        {
            for (int i = goTiles.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(goTiles[i]);
            }
            goTiles.Clear();
        }

        for(int i = 0; i < v2Size.x; i++)
        {
            for (int j = 0; j < v2Size.y; j++)
            {
                GameObject go = Instantiate(prefTile, tParent);
                go.transform.localPosition = new Vector3(i,0,j);
                goTiles.Add(go);
            }
        }
    }

    /* LEVEL SERIALIZED EXAMPLE --> #X,Z|TopTile-BotTile|TopTile-BotTile|TopTile-BotTile|TopTile-BotTile|TopTile-BotTile|#
     #10,10|4-0|0-6|0-0|0-0|0-0|0-6|0-0|0-0|0-0|0-0|0-0|0-0|0-0|0-6|0-0|0-0|0-0|0-0|0-0|0-0|0-0|0-2|#
    */

    public void SaveSerializedLevel()
    {
        string sLevel = v2Size.x.ToString() + BSConstants.LEVEL_SIZE_SEPARATOR.ToString() + v2Size.y.ToString() + BSConstants.LEVEL_TILE_SEPARATOR.ToString();
        for (int i = 0; i < goTiles.Count; i++)
        {
            sLevel += ((int)goTiles[i].GetComponent<TileEditor>().topTileType).ToString() + BSConstants.LEVEL_TILE_UPDOWN_SEPARATOR + ((int)goTiles[i].GetComponent<TileEditor>().botTileType).ToString() + BSConstants.LEVEL_TILE_SEPARATOR;
        }

        sLevel += BSConstants.LEVEL_LEVEL_SEPARATOR;
        WriteString(sLevel);
    }

    private void WriteString(string sSerializedLevel)
    {
        string path = "Assets/Resources/Levels.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(sSerializedLevel);
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load<TextAsset>("Levels");

        //Print the text from the file
        Debug.Log("Level Saved: " + asset.text);
    }

    private void ReadString()
    {
        string path = "Assets/Resources/Levels.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log("Levels: " + reader.ReadToEnd());
        reader.Close();
    }

    private void FlipBoard()
    {
        Debug.Log("Flip Board");
        for (int i = 0; i < goTiles.Count; i++)
        {
            goTiles[i].GetComponent<TileEditor>().bOnTop = bBoardOnTop;
        }
    }

}

using UnityEngine;
using System.Collections;
using System.IO;

public class TerrainGen : MonoBehaviour {
    public GameObject prefabTile, prefabEntrance;
    public GameObject goTerrainHolder;

    //Stores the gameobjects
    private GameObject[,] goMatrixTerrain;
    //Stores the types of tiles
    private int[,] iMatrixTerrain;

    private const int NUMBER_OF_COLUMNS = BSConstants.Z_SIZE;
    private const int NUMBER_OF_ROWS = BSConstants.X_SIZE;
    private const float MIN_DEGREE_ANGLE = .05f;

    private bool bFinishInstantiating = false;

    private Vector3 nextSpinVector = Vector3.zero;
    private Transform tLastTile;

    private Hero hero;
    private bool bFinishedRotating = true;


    public Material trapMaterial, fixedMaterial, rotatingMaterialTop, rotatingMaterialBot;
    public GameObject prefTrap, prefEnemy, prefEntrance;

    public Vector2 v2TileSize;

    public Vector2 v2EntrancePosition, v2ExitPositionTop, v2ExitPositionBot;

    void Awake ()
    {
        Debug.Log("Start()");
        /*
        goMatrixTerrain = new GameObject[BSConstants.Z_SIZE * 2 + BSConstants.CENTER_DIM,
                                BSConstants.Y_SIZE,
                                BSConstants.Z_SIZE * 2 + BSConstants.CENTER_DIM];
        InitMatrix();
        FillIntMatrixShape();
        InstantiateMatrix();
        */
        GenerateLevel();
        bFinishInstantiating = true;
        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
    }

    private void InitMatrix()
    {
        Debug.Log("InitMatrix()");

        iMatrixTerrain = new int[BSConstants.X_SIZE,
                                BSConstants.Y_SIZE];
        for(int i = 0; i < iMatrixTerrain.GetLength(0); i++)
        {
            for (int j = 0; j < iMatrixTerrain.GetLength(1); j++)
            {
                iMatrixTerrain[i, j] = (int)BSEnums.TileType.AIR;
            }
        }
    }
    private void FillMatrixRectangle(int iXStart, int iZStart, int XSize, int ZSize, BSEnums.TileType tile, int iNumberOfTraps = BSConstants.NUMBER_OF_TRAPS_PER_RECTANGLE)
    {
        Debug.Log("FillMatrixRectangle()");

/*
        for (int x = iXStart; x < (iXStart + XSize) && x < iMatrixTerrain.GetLength(0); x++)
        {
            for (int z = iZStart; z < (iZStart + ZSize) && z < iMatrixTerrain.GetLength(2); z++)
            {

                iMatrixTerrain[x, BSConstants.Y_SIZE - 1, z] = (int)tile;
                for(int y = 0; y < BSConstants.Y_SIZE - 1; y++)
                {
                    iMatrixTerrain[x, y, z] = (int)BSEnums.TileType.FIXED;

                }
            }
        }
        //Place Entrance
        iMatrixTerrain[0, 0, Random.Range(0, BSConstants.Z_SIZE - 1)] = (int)BSEnums.TileType.ENTRANCE;

        //Place Exit
        iMatrixTerrain[BSConstants.X_SIZE-1, 0, Random.Range(0, BSConstants.Z_SIZE - 1)] = (int)BSEnums.TileType.EXIT;
        */
    }

    private void FillIntMatrixShape()
    {
        //Center
        FillMatrixRectangle(0, 0, BSConstants.X_SIZE, BSConstants.Z_SIZE, BSEnums.TileType.ROTATING, 0);

    }

    private void InstantiateMatrix()
    {
        /*
        Debug.Log("InstantiateMatrix()");
        for (int x = 0; x < iMatrixTerrain.GetLength(0); x++)
        {
            for (int y = 0; y < iMatrixTerrain.GetLength(1); y++)
            {
                for (int z = 0; z < iMatrixTerrain.GetLength(2); z++)
                {
                    if (iMatrixTerrain[x, y, z] == (int)BSEnums.TileType.ROTATING)
                    {
                        goMatrixTerrain[x, y, z] = Instantiate(prefabTile, new Vector3(x, y - BSConstants.Y_SIZE, z), prefabTile.transform.rotation) as GameObject;
                        goMatrixTerrain[x, y, z].GetComponent<Tile>().SetRotationSpeed(BSConstants.ROTATION_SPEED);
                    }
                    else if (iMatrixTerrain[x, y, z] == (int)BSEnums.TileType.TRAP)
                    {
                        //Instantiate(trap1Prefab, new Vector3(x, y - BSConstants.Y_SIZE, z), prefabTile.transform.rotation);
                    }
                    else if (iMatrixTerrain[x, y, z] == (int)BSEnums.TileType.FIXED)
                    {
                        Instantiate(prefabTile, new Vector3(x, y - BSConstants.Y_SIZE, z), prefabTile.transform.rotation);
                    }
                    else if (iMatrixTerrain[x, y, z] == (int)BSEnums.TileType.ENTRANCE || iMatrixTerrain[x, y, z] == (int)BSEnums.TileType.EXIT)
                    {
                        goMatrixTerrain[x, y, z] = Instantiate(prefabEntrance, new Vector3(x, y - BSConstants.Y_SIZE, z), prefabTile.transform.rotation);
                        if (iMatrixTerrain[x, y, z] == (int)BSEnums.TileType.ENTRANCE)
                        {
                            v3EntrancePosition = new Vector3(x, y, z);
                        }
                        else
                        {
                            v3ExitPosition = new Vector3(x, y, z);
                        }
                    }
                }
            }
        }*/
    }

    // Update is called once per frame
    void Update () {

    }

    public GameObject GetTile(Vector2 v2Tile)
    {
        //Debug.Log("GetTile on position: " +v3HeroPosition);
        return goMatrixTerrain[(int)v2Tile.x, (int)v2Tile.y];
    }

    private string ReadString()
    {
        string path = "Assets/Resources/Levels.txt";
        string sLevels;
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        sLevels = reader.ReadToEnd();
        reader.Close();
        return sLevels;
    }
    // 2,1|0-0|0-2|#
    private void GenerateLevel()
    {
        string sAllLevels = ReadString();

        string[] sLevelsSplited = sAllLevels.Split(BSConstants.LEVEL_LEVEL_SEPARATOR);

        string sLevel = sLevelsSplited[0];
        string[] sTiles = sLevel.Split(BSConstants.LEVEL_TILE_SEPARATOR);

        //GetSize
        string[] sSize = sTiles[0].Split(BSConstants.LEVEL_SIZE_SEPARATOR);

        //Tiles between 1- sTiles.length - 2
        goMatrixTerrain = new GameObject[int.Parse(sSize[0]),
                                int.Parse(sSize[1])];

        v2TileSize = new Vector2(int.Parse(sSize[0]),
                                int.Parse(sSize[1]));
        for (int i = 0; i < int.Parse(sSize[0]); i++)
        {
            for (int j = 0; j < int.Parse(sSize[0]); j++)
            {
                GameObject go = Instantiate(prefabTile);
                go.transform.localPosition = new Vector3(i, 0, j);

                goMatrixTerrain[i, j] = go;
                GameObject goOnTop, goOnBot;
                MeshRenderer goTopTile = go.GetComponentsInChildren<MeshRenderer>()[0];
                MeshRenderer goBotTile = go.GetComponentsInChildren<MeshRenderer>()[1];
                string sTopTileType = sTiles[1 + i * int.Parse(sSize[0]) + j].Split(BSConstants.LEVEL_TILE_UPDOWN_SEPARATOR)[0];
                string sBotTileType = sTiles[1 + i * int.Parse(sSize[0]) + j].Split(BSConstants.LEVEL_TILE_UPDOWN_SEPARATOR)[1];

                go.GetComponentInChildren<Tile>().SetBotTileType((BSEnums.TileType)int.Parse(sBotTileType));
                go.GetComponentInChildren<Tile>().SetTopTileType((BSEnums.TileType)int.Parse(sTopTileType));

                if (sTopTileType == ((int)BSEnums.TileType.AIR).ToString())
                {
                    goTopTile.enabled = false;
                    goBotTile.enabled = false;
                }
                else if (sTopTileType == ((int)BSEnums.TileType.FIXED).ToString())
                {
                    goTopTile.material = fixedMaterial;

                }
                else if (sTopTileType == ((int)BSEnums.TileType.TRAP).ToString())
                {
                    goTopTile.material = trapMaterial;
                    goOnTop = Instantiate(prefTrap, goTopTile.transform);
                    goOnTop.transform.localScale = BSConstants.V3_SPIKE_SCALE;
                    goOnTop.transform.localPosition = BSConstants.V3_SPIKE_POSITION_IN_TILE;

                }
                else if (sTopTileType == ((int)BSEnums.TileType.ENEMY).ToString())
                {
                    goOnTop = Instantiate(prefEnemy, goTopTile.transform);
                    goTopTile.material = rotatingMaterialTop;

                    goOnTop.transform.localScale = BSConstants.V3_ENEMY_SCALE;
                    goOnTop.transform.localPosition = BSConstants.V3_ENEMY_POSITION_IN_TILE;
                }
                else if (sTopTileType == ((int)BSEnums.TileType.ENTRANCE).ToString())
                {
                    goOnTop = Instantiate(prefEntrance, goTopTile.transform);
                    goTopTile.material = rotatingMaterialTop;

                    goOnTop.transform.localScale = BSConstants.V3_ENTRANCE_SCALE;
                    goOnTop.transform.localPosition = BSConstants.V3_ENTRANCE_POSITION_IN_TILE;
                    v2EntrancePosition = new Vector2(i, j);

                }
                else if (sTopTileType == ((int)BSEnums.TileType.EXIT).ToString())
                {
                    goOnTop = Instantiate(prefEntrance, goTopTile.transform);
                    goTopTile.material = rotatingMaterialTop;

                    goOnTop.transform.localScale = BSConstants.V3_ENTRANCE_SCALE;
                    goOnTop.transform.localPosition = BSConstants.V3_ENTRANCE_POSITION_IN_TILE;
                    Debug.Log("EXIT: " + i + "," + j);
                    v2ExitPositionTop = new Vector2(i, j);
                }

                if (sBotTileType == ((int)BSEnums.TileType.AIR).ToString())
                {
                    goTopTile.enabled = false;
                    goBotTile.enabled = false;

                }
                else if (sBotTileType == ((int)BSEnums.TileType.FIXED).ToString())
                {
                    goBotTile.material = fixedMaterial;

                }
                else if (sBotTileType == ((int)BSEnums.TileType.TRAP).ToString())
                {
                    goBotTile.material = trapMaterial;
                    goOnBot = Instantiate(prefTrap, goBotTile.transform);
                    goOnBot.transform.localScale = BSConstants.V3_SPIKE_SCALE;
                    goOnBot.transform.localPosition = BSConstants.V3_SPIKE_POSITION_IN_TILE;

                }
                else if (sBotTileType == ((int)BSEnums.TileType.ENEMY).ToString())
                {
                    goOnBot = Instantiate(prefEnemy, goBotTile.transform);
                    goBotTile.material = rotatingMaterialBot;
                    goOnBot.transform.localScale = BSConstants.V3_ENEMY_SCALE;
                    goOnBot.transform.localPosition = BSConstants.V3_ENEMY_POSITION_IN_TILE;
                }
                else if (sBotTileType == ((int)BSEnums.TileType.EXIT).ToString())
                {
                    goOnBot = Instantiate(prefEntrance, goBotTile.transform);
                    goBotTile.material = rotatingMaterialBot;

                    goOnBot.transform.localScale = BSConstants.V3_ENTRANCE_SCALE;
                    goOnBot.transform.localPosition = BSConstants.V3_ENTRANCE_POSITION_IN_TILE;
                    Debug.Log("EXIT: " + i + "," + j);

                    v2ExitPositionBot = new Vector2(i,j);
                }
                else if (sBotTileType == ((int)BSEnums.TileType.ENTRANCE).ToString())
                {
                    goOnBot = Instantiate(prefEntrance, goBotTile.transform);
                    goBotTile.material = rotatingMaterialBot;

                    goOnBot.transform.localScale = BSConstants.V3_ENTRANCE_SCALE;
                    goOnBot.transform.localPosition = BSConstants.V3_ENTRANCE_POSITION_IN_TILE;

                    v2EntrancePosition = new Vector2(i,j);
                }
            }
        }
    }
}


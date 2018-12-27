using UnityEngine;
using System.Collections;
using System.IO;

public class TerrainGen : MonoBehaviour {
    public GameObject prefabTile, prefabEntrance;
    public GameObject goTerrainHolder;

    //Stores the gameobjects
    private GameObject[,,] goMatrixTerrain;
    //Stores the types of tiles
    private int[,,] iMatrixTerrain;

    private const int NUMBER_OF_COLUMNS = BSConstants.Z_SIZE;
    private const int NUMBER_OF_ROWS = BSConstants.X_SIZE;
    private const float MIN_DEGREE_ANGLE = .05f;

    private bool bFinishInstantiating = false;

    private Vector3 nextSpinVector = Vector3.zero;
    private Transform tLastTile;

    private Hero hero;
    private bool bFinishedRotating = true;

    public Vector3 v3EntrancePosition, v3ExitPosition;

    void Awake ()
    {
        Debug.Log("Start()");

        goMatrixTerrain = new GameObject[BSConstants.Z_SIZE * 2 + BSConstants.CENTER_DIM,
                                BSConstants.Y_SIZE,
                                BSConstants.Z_SIZE * 2 + BSConstants.CENTER_DIM];
        InitMatrix();
        FillIntMatrixShape();
        InstantiateMatrix();
        bFinishInstantiating = true;
        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
    }

    private void InitMatrix()
    {
        Debug.Log("InitMatrix()");

        iMatrixTerrain = new int[BSConstants.X_SIZE,
                                BSConstants.Y_SIZE,
                                BSConstants.Z_SIZE];
        for(int i = 0; i < iMatrixTerrain.GetLength(0); i++)
        {
            for (int j = 0; j < iMatrixTerrain.GetLength(1); j++)
            {
                for (int k = 0; k < iMatrixTerrain.GetLength(2); k++)
                {
                    iMatrixTerrain[i, j, k] = (int)BSEnums.TileType.AIR;
                }
            }
        }
    }
    private void FillMatrixRectangle(int iXStart, int iZStart, int XSize, int ZSize, BSEnums.TileType tile, int iNumberOfTraps = BSConstants.NUMBER_OF_TRAPS_PER_RECTANGLE)
    {
        Debug.Log("FillMatrixRectangle()");

        /*Vector2[] vTraps = new Vector2[iNumberOfTraps];
        bool bTrap = false;
        for(int i = 0; i < iNumberOfTraps; i++)
        {
            vTraps[i] = new Vector2(Random.Range(iXStart, iXStart + X), Random.Range(iZStart, iZStart + Z));
        }*/
        for (int x = iXStart; x < (iXStart + XSize) && x < iMatrixTerrain.GetLength(0); x++)
        {
            for (int z = iZStart; z < (iZStart + ZSize) && z < iMatrixTerrain.GetLength(2); z++)
            {
          /*      for (int i = 0; i < iNumberOfTraps; i++)
                {
                    if(vTraps[i].x == x && vTraps[i].y == z)
                    {
                        bTrap = true;
                    }
                }*/
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

    }

    private void FillIntMatrixShape()
    {
        //Center
        FillMatrixRectangle(0, 0, BSConstants.X_SIZE, BSConstants.Z_SIZE, BSEnums.TileType.ROTATING, 0);

    }

    private void InstantiateMatrix()
    {
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
        }
    }

    // Update is called once per frame
    void Update () {

    }

    public GameObject GetTile(Vector3 v3HeroPosition)
    {
        //Debug.Log("GetTile on position: " +v3HeroPosition);
        return goMatrixTerrain[(int)v3HeroPosition.x, (int)v3HeroPosition.y, (int)v3HeroPosition.z];
    }

    private void ReadString()
    {
        string path = "Assets/Resources/Levels.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log("Levels: " + reader.ReadToEnd());
        reader.Close();
    }

    private void GenerateLevel()
    {
        //pick 3 to 5 random blocks

        //pick the first block and choose a random side (0,3), stick a new block to a random number of tile on that side
        //Repeat the same process on for each of the random blocks the newest added block (be aware of transpassing over the previos block)
    }
}


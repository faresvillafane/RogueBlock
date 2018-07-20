using UnityEngine;
using System.Collections;

public class TerrainGen : MonoBehaviour {
    public GameObject prefabTile;
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

    void Start ()
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
    private void FillMatrixRectangle(int iXStart, int iZStart, int X, int Z, BSEnums.TileType tile, int iNumberOfTraps = BSConstants.NUMBER_OF_TRAPS_PER_RECTANGLE)
    {
        Debug.Log("FillMatrixRectangle()");

        Vector2[] vTraps = new Vector2[iNumberOfTraps];
        bool bTrap = false;
        for(int i = 0; i < iNumberOfTraps; i++)
        {
            vTraps[i] = new Vector2(Random.Range(iXStart, iXStart + X), Random.Range(iZStart, iZStart + Z));
        }
        for (int x = iXStart; x < (iXStart + X) && x < iMatrixTerrain.GetLength(0); x++)
        {
            for (int z = iZStart; z < (iZStart + Z) && z < iMatrixTerrain.GetLength(2); z++)
            {
                for (int i = 0; i < iNumberOfTraps; i++)
                {
                    if(vTraps[i].x == x && vTraps[i].y == z)
                    {
                        bTrap = true;
                    }
                }
                iMatrixTerrain[x, BSConstants.Y_SIZE - 1, z] = (bTrap)?(int) BSEnums.TileType.TRAP : (int)tile;
                for(int y = 0; y < BSConstants.Y_SIZE - 1; y++)
                {
                    iMatrixTerrain[x, y, z] = (bTrap) ? (int)BSEnums.TileType.TRAP : (int)BSEnums.TileType.FIXED;

                }
                bTrap = false;
            }
        }
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
                    if (iMatrixTerrain[x,y,z] == (int)BSEnums.TileType.CENTER)
                    {
                       // Instantiate(prefabCenterTile, new Vector3(x, y - BSConstants.Y_SIZE, z), prefabTile.transform.rotation);
                    }
                    else if (iMatrixTerrain[x, y, z] == (int)BSEnums.TileType.ROTATING)
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
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {

    }

    public bool ShouldEnemyMove(Vector3 position)
    {
        return ShouldEnemyMoveTo(position, BSEnums.TileType.ROTATING);
    }

    public bool ShouldEnemyMoveInverse(Vector3 position)
    {
        return ShouldEnemyMoveTo(position, BSEnums.TileType.INVERSE_ROTATING);
    }

    public bool ShouldEnemyMoveTo(Vector3 position, BSEnums.TileType tile)
    {
        if (goMatrixTerrain[(int)position.x, BSConstants.Y_SIZE - 1, (int)position.z] != null && goMatrixTerrain[(int)position.x, BSConstants.Y_SIZE - 1, (int)position.z].GetComponent<Tile>() != null)
        {
            return (goMatrixTerrain[(int)position.x, BSConstants.Y_SIZE - 1, (int)position.z].GetComponent<Tile>().GetTileType() == tile);
        }
        return false;
    }

    public GameObject GetTile(Vector3 v3HeroPosition)
    {
        Debug.Log("GetTile on position: " +v3HeroPosition);
        return goMatrixTerrain[(int)v3HeroPosition.x, (int)v3HeroPosition.y, (int)v3HeroPosition.z];
    }
}


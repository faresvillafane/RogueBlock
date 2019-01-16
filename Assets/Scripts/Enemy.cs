//#define CONSTANT_FORCE
//ALSO ENABLE ENEMY MOVEMENT X AND Z, on the prefab
using UnityEngine;
using System.Collections;
public class Enemy : MonoBehaviour {
    private Hero hero;
    private const float DELAY_BETWEEN_INPUTS = 1f;
    private float fLastTimeMovement = 0;

    private TerrainGen terrainGen;
    public bool bOnTop = true;
    private Vector3 v3MatrixPosition;

    // Use this for initialization
    void Start ()
    {
        hero = GameObject.FindGameObjectWithTag(BSConstants.TAG_HERO).GetComponent<Hero>();
        terrainGen = GameObject.FindGameObjectWithTag(BSConstants.TAG_GAME_CONTROLLER).GetComponent<TerrainGen>();
        //v3MatrixPosition = this.transform.position = new Vector3(Random.Range(0,BSConstants.X_SIZE), 0, Random.Range(0, BSConstants.Z_SIZE));
        //transform.position = new Vector3(transform.position.x, BSConstants.POSITION_OVER_SCENARIO_Y, transform.position.z);
        if (bOnTop)
        {
            transform.SetParent(terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponentInChildren<Tile>().goTopTile.transform);
        }
        else
        {
            transform.SetParent(terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponentInChildren<Tile>().goBotTile.transform);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponent<Tile>().GetTileTypeFrom(bOnTop) == BSEnums.TileType.TRAP)
        {
            Die();
        }
        if (bOnTop == hero.bOnTop  
            && terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponent<Tile>().bFinishedRotating
            && Time.realtimeSinceStartup >= fLastTimeMovement + DELAY_BETWEEN_INPUTS)
        {
            fLastTimeMovement = Time.realtimeSinceStartup;
            Vector3 v3Nextstep = GetNextStep();
            Vector3 v3Rot;
            if (v3Nextstep == Vector3.right)
            {
                v3Rot = new Vector3(0, 180, 0);
            }
            else if (v3Nextstep == Vector3.left)
            {
                v3Rot = new Vector3(0, 0, 0);

            }
            else if (v3Nextstep == Vector3.back)
            {
                v3Rot = new Vector3(0, 270, 0);
            }
            else
            {
                v3Rot = new Vector3(0, 90, 0);
            }
            transform.parent = null;
            transform.rotation = Quaternion.Euler(v3Rot);
            transform.position += v3Nextstep;
            v3MatrixPosition += v3Nextstep;

            if (bOnTop)
            {
                if (terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponentInChildren<Tile>().bOnTop)
                {
                    transform.SetParent(terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponentInChildren<Tile>().goTopTile.transform);
                }
                else
                {
                    transform.SetParent(terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponentInChildren<Tile>().goBotTile.transform);
                }
            }
            else
            {
                if (terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponentInChildren<Tile>().bOnTop)
                {
                    transform.SetParent(terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponentInChildren<Tile>().goBotTile.transform);
                }
                else
                {
                    transform.SetParent(terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponentInChildren<Tile>().goTopTile.transform);

                }
            }

        }
        else if (bOnTop != hero.bOnTop)
        {
            fLastTimeMovement = Time.realtimeSinceStartup;
        }
    }


    private void Die()
    {
        Destroy(this.gameObject);
    }

    public void InitializeEnemy(bool bOnTopPrm, Vector3 v3StartPosition)
    {
        v3MatrixPosition = v3StartPosition;
        bOnTop = bOnTopPrm;
    }


    private Vector3 GetNextStep()
    {
        Vector3 v3DistanceBetween = hero.transform.position -this.transform.position;
        Vector3 v3NextStep = Vector3.zero;

        if (v3DistanceBetween.x != 0 
            && v3DistanceBetween.z != 0)
        {
            if(Random.Range(0f, 1f) > .5f)
            {
                v3NextStep = new Vector3(0, 0, (v3DistanceBetween.z < 0) ? -1 : 1);
            }
            else
            {
                v3NextStep = new Vector3((v3DistanceBetween.x < 0) ? -1 : 1, 0, 0);
            }
        }
        else if (v3DistanceBetween.x != 0)
        {
            v3NextStep = new Vector3((v3DistanceBetween.x < 0) ? -1 : 1, 0, 0);
        }
        else if (v3DistanceBetween.z != 0)
        {
            v3NextStep = new Vector3(0, 0,(v3DistanceBetween.z < 0) ? -1 : 1);
        }

        return v3NextStep;
    }
}

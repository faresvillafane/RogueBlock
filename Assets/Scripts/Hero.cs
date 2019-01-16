using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    private int iMana = BSConstants.MAX_MANA;
    private int iHealth = 100;
    private bool bRecoveringMana = false;
    private TerrainGen terrainGen;
    private Text HP;
    private Text MP;
    private Vector3 v3MatrixPosition;
    public bool bLastInputX = false;
    private Vector3 v3FacingDirection = Vector3.forward;

    private const float DELAY_BETWEEN_INPUTS = .25f;
    private const float SWAP_COOLDOWN = .5f;
    private float fLastTimeMovement = 0, fLastTimeSwap = 0;

    public bool bOnTop = true;
    // Use this for initialization
    void Start ()
    {
        terrainGen = GameObject.FindGameObjectWithTag(BSConstants.TAG_GAME_CONTROLLER).GetComponent<TerrainGen>();
        v3MatrixPosition = transform.position = new Vector3(terrainGen.v2EntrancePosition.x,0,terrainGen.v2EntrancePosition.y);

        transform.position = new Vector3(transform.position.x, BSConstants.POSITION_OVER_SCENARIO_Y, transform.position.z);

        v3FacingDirection = Vector3.right;
        Vector3 v3Rot;
        v3Rot = new Vector3(0, 180, 0);
        transform.rotation = Quaternion.Euler(v3Rot);

        transform.SetParent(terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponentInChildren<Tile>().goTopTile.transform);
    }
	
	// Update is called once per frame
	void Update ()
    {
        InputController();
    }

    private void InputController()
    {
        if ((BSUtils.WorldPosToMatrix(v3MatrixPosition) == terrainGen.v2ExitPositionTop &&
            terrainGen.GetTile(terrainGen.v2ExitPositionTop).GetComponent<Tile>().bOnTop == bOnTop) 
            || (BSUtils.WorldPosToMatrix(v3MatrixPosition) == terrainGen.v2ExitPositionBot &&
            terrainGen.GetTile(terrainGen.v2ExitPositionBot).GetComponent<Tile>().bOnTop != bOnTop)
            )
        {
            WonLevel();
        }
        else if (terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition)).GetComponent<Tile>().GetTileTypeFrom(bOnTop) == BSEnums.TileType.TRAP)
        {
            Die();
        }

        if (iMana > BSConstants.SPELL_COST)
        {

            if (Time.realtimeSinceStartup >= fLastTimeSwap + SWAP_COOLDOWN)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    fLastTimeSwap = Time.realtimeSinceStartup;
                    SwapTile();
                }
            }


            if (Time.realtimeSinceStartup >= fLastTimeMovement + DELAY_BETWEEN_INPUTS /*&& terrainGen.GetTile(v3MatrixPosition).GetComponentInChildren<Tile>().bFinishedRotating*/)
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    MoveInDirection(Vector3.forward);
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    MoveInDirection(Vector3.back);

                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    MoveInDirection(Vector3.right);

                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    MoveInDirection(Vector3.left);
                }
            }
        }
    }

    public void MoveInDirection(Vector3 v3MoveKey)
    {
        //Controlar bordes, y vacios
        Vector3 v3ResultPosition = v3MatrixPosition + v3MoveKey;
        transform.SetParent(null);
        if (v3FacingDirection == v3MoveKey && IsValidPosition(v3ResultPosition) && terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3ResultPosition)).GetComponentInChildren<Tile>().bFinishedRotating)
        {
            transform.position += v3MoveKey;
            v3MatrixPosition += v3MoveKey;
        }
        else
        {
            v3FacingDirection = v3MoveKey;
            Vector3 v3Rot;
            if(v3FacingDirection == Vector3.right)
            {
                v3Rot = new Vector3(0, 180, 0);
            }
            else if(v3FacingDirection == Vector3.left)
            {
                v3Rot = new Vector3(0, 0, 0);

            }
            else if(v3FacingDirection == Vector3.back)
            {
                v3Rot = new Vector3(0, 270, 0);
            }
            else
            {
                v3Rot = new Vector3(0, 90, 0);
            }
            transform.rotation = Quaternion.Euler(v3Rot);
        }
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

        fLastTimeMovement = Time.realtimeSinceStartup;
    } 

    public bool IsValidPosition(Vector3 v3PositionToCheck)
    {
        return v3PositionToCheck.x >= 0 && v3PositionToCheck.z >= 0
            && v3PositionToCheck.x < terrainGen.v2TileSize.x && v3PositionToCheck.z < terrainGen.v2TileSize.y
            && terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3PositionToCheck)).GetComponentInChildren<Tile>().GetTileTypeFrom(bOnTop) != BSEnums.TileType.AIR;


    }



    public void SwapTile()
    {
        BSEnums.SwipeDirection swipeDir = BSEnums.SwipeDirection.BACK;

        if (v3FacingDirection == Vector3.forward)
        {
            swipeDir = BSEnums.SwipeDirection.FORWARD;
        }
        else if(v3FacingDirection == Vector3.right)
        {
            swipeDir = BSEnums.SwipeDirection.RIGHT;
        }
        else if (v3FacingDirection == Vector3.left)
        {
            swipeDir = BSEnums.SwipeDirection.LEFT;
        }

        terrainGen.GetTile(BSUtils.WorldPosToMatrix(v3MatrixPosition + v3FacingDirection)).GetComponentInChildren<Tile>().GetNextMovement(swipeDir);

    }

    /*
    public void MoveTerrain(BSEnums.SwipeDirection swipe)
    {
        if (swipe == BSEnums.SwipeDirection.RIGHT
            || swipe == BSEnums.SwipeDirection.LEFT)
        {
            for (int i = 0; i < BSConstants.X_SIZE; i++)
            {
                terrainGen.GetTile(new Vector3(i, v3MatrixPosition.y, v3MatrixPosition.z)).GetComponentInChildren<Tile>().GetNextMovement(swipe);

            }
        }
        else if (swipe == BSEnums.SwipeDirection.BACK
                || swipe == BSEnums.SwipeDirection.FORWARD)
        {
            for (int i = 0; i < BSConstants.Z_SIZE; i++)
            {
                terrainGen.GetTile(new Vector3(v3MatrixPosition.x, v3MatrixPosition.y, i)).GetComponentInChildren<Tile>().GetNextMovement(swipe);
            }
        }
    }*/

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals(BSConstants.TAG_ENEMY))
        {
            Die();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        /*if (col.gameObject.tag.Equals(BSConstants.TAG_TILE) && terrainGen.GetTile(v3MatrixPosition).GetComponentInChildren<Tile>().GetTileType().Equals(BSEnums.TileType.TRAP))
        {
            Die();
        }*/
    }

    public void Die()
    {
        SceneManager.LoadScene("Main");

    }

    public void WonLevel()
    {
        SceneManager.LoadScene("Main");
    }
}

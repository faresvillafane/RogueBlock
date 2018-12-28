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
        v3MatrixPosition = transform.position = terrainGen.v3EntrancePosition;

        transform.position = new Vector3(transform.position.x, BSConstants.POSITION_OVER_SCENARIO_Y, transform.position.z);
    }
	
	// Update is called once per frame
	void Update ()
    {
        InputController();
    }

    private void InputController()
    {
        if (v3MatrixPosition == terrainGen.v3ExitPosition)
        {
            WonLevel();
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
                transform.SetParent(terrainGen.GetTile(v3MatrixPosition).GetComponentInChildren<Tile>().goTopTile.transform);
            }
        }
    }

    public void MoveInDirection(Vector3 v3MoveKey)
    {
        transform.SetParent(null);
        if (v3FacingDirection == v3MoveKey && terrainGen.GetTile(v3MatrixPosition + v3MoveKey).GetComponentInChildren<Tile>().bFinishedRotating)
        {
            transform.position += v3MoveKey;
            v3MatrixPosition += v3MoveKey;
        }
        else
        {
            v3FacingDirection = v3MoveKey;
        }
        fLastTimeMovement = Time.realtimeSinceStartup;
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

        terrainGen.GetTile(v3MatrixPosition + v3FacingDirection).GetComponentInChildren<Tile>().GetNextMovement(swipeDir);

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
            //Die();
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
        //SceneManager.LoadScene("Main");
    }
}

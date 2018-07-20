using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private const float DELAY_BETWEEN_INPUTS = .5f;
    private const float SWAP_COOLDOWN = .5f;
    private float fLastTimeMovement = 0, fLastTimeSwap = 0;
    // Use this for initialization
    void Start () {
        terrainGen = GameObject.FindGameObjectWithTag(BSConstants.TAG_GAME_CONTROLLER).GetComponent<TerrainGen>();
        v3MatrixPosition = transform.position = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
        InputController();

    }

    private void InputController()
    {
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


            if (Time.realtimeSinceStartup >= fLastTimeMovement + DELAY_BETWEEN_INPUTS)
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    transform.position += Vector3.forward;
                    v3MatrixPosition += Vector3.forward;
                    fLastTimeMovement = Time.realtimeSinceStartup;
                    v3FacingDirection = Vector3.forward;
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    transform.position += Vector3.back;
                    v3MatrixPosition += Vector3.back;
                    fLastTimeMovement = Time.realtimeSinceStartup;
                    v3FacingDirection = Vector3.back;
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    transform.position += Vector3.right;
                    v3MatrixPosition += Vector3.right;
                    fLastTimeMovement = Time.realtimeSinceStartup;
                    v3FacingDirection = Vector3.right;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    transform.position += Vector3.left;
                    v3MatrixPosition += Vector3.left;
                    fLastTimeMovement = Time.realtimeSinceStartup;
                    v3FacingDirection = Vector3.left;
                }
                transform.SetParent(terrainGen.GetTile(v3MatrixPosition).GetComponentInChildren<Tile>().goTopTile.transform);
            }
        }
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
    }

    private void ChangeEnemyPos(BSEnums.SwipeDirection swipe)
    {
        switch (swipe)
        {
            case BSEnums.SwipeDirection.FORWARD:
                ChangeEnemyZPos(true);
                break;
            case BSEnums.SwipeDirection.BACK:
                ChangeEnemyZPos(false);
                break;
            case BSEnums.SwipeDirection.RIGHT:
                ChangeEnemyXPos(true);
                break;
            case BSEnums.SwipeDirection.LEFT:
                ChangeEnemyXPos(false);
                break;
        }

    }

    public void ChangeEnemyXPos(bool bPositive)
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(BSConstants.TAG_ENEMY))
        {
            enemy.GetComponent<Enemy>().ChangePositionX(bPositive);
        }
    }
    public void ChangeEnemyZPos(bool bPositive)
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(BSConstants.TAG_ENEMY))
        {
            enemy.GetComponent<Enemy>().ChangePositionZ(bPositive);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals(BSConstants.TAG_TILE))
        {
        }
    }
}

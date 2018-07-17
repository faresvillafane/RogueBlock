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
        if (terrainGen.FinishedRotatingTiles() && iMana > BSConstants.SPELL_COST)
        {
            if (Input.GetKeyUp("up"))
            {
                MoveTerrain(BSEnums.SwipeDirection.UP);
            }

            if (Input.GetKeyUp("down"))
            {
                MoveTerrain(BSEnums.SwipeDirection.DOWN);
            }

            if (Input.GetKeyUp("right"))
            {
                MoveTerrain(BSEnums.SwipeDirection.RIGHT);
                bLastInputX = true;
            }
            if (Input.GetKeyUp("left"))
            {
                MoveTerrain(BSEnums.SwipeDirection.LEFT);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                transform.position += Vector3.forward;
                v3MatrixPosition += Vector3.forward;
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                transform.position += Vector3.right;
                v3MatrixPosition += Vector3.right;

            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                transform.position += Vector3.left;
                v3MatrixPosition += Vector3.left;


            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                transform.position += Vector3.back;
                v3MatrixPosition += Vector3.back;

            }
        }
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
        else if (swipe == BSEnums.SwipeDirection.DOWN
                || swipe == BSEnums.SwipeDirection.UP)
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
            case BSEnums.SwipeDirection.UP:
                ChangeEnemyZPos(true);
                break;
            case BSEnums.SwipeDirection.DOWN:
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
        //Debug.Log("OnCollisionEnter(): " + collision.gameObject.tag);
        if (collision.gameObject.tag.Equals(BSConstants.TAG_TILE))
        {
           // Debug.Log("OnCollisionEnter(): " + BSConstants.TAG_TILE);

            //collision.gameObject.GetComponentInParent<Tile>().SetTileType(BSEnums.TileType.FIXED);
        }
    }
}

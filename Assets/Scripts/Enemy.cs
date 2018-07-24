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
        v3MatrixPosition = this.transform.position = new Vector3(Random.Range(0,BSConstants.X_SIZE), 0, Random.Range(0, BSConstants.Z_SIZE));
        this.transform.position += Vector3.up;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (bOnTop && Time.realtimeSinceStartup >= fLastTimeMovement + DELAY_BETWEEN_INPUTS)
        {
            fLastTimeMovement = Time.realtimeSinceStartup;
            Vector3 v3Nextstep = GetNextStep();
            transform.position += v3Nextstep;
            v3MatrixPosition += v3Nextstep;
            transform.SetParent(terrainGen.GetTile(v3MatrixPosition).GetComponentInChildren<Tile>().goTopTile.transform);

        }
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

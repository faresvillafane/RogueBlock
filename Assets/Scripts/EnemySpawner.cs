using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public GameObject goEnemy1;

    private bool binstantiating = false;
    // Use this for initialization

    private GameObject[] enemies;

    private int iNumberOfEnemiesAlive = 0; 

    private int iRandomEnemyAmountPerWave = 0;

    void Start()
    {
        enemies = new GameObject[BSConstants.ENEMY_POOL_SIZE];
        FillEnemyPool();

    }
	
    private void FillEnemyPool()
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = Instantiate(goEnemy1, BSConstants.OBJECT_OUT_OF_SCREEN, goEnemy1.transform.rotation) as GameObject;
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if (!binstantiating)
        {
            StartCoroutine(SpawnEnemyWave());
        }
	}

    private int GetRandomTile()
    {
        return Random.Range(0, 4);
    }

    private Vector3 GetRandomSpawnPoint(int iTile)
    {
        int iFixedY = BSConstants.ENEMY_Y_SPAWN;
        Vector3 newV3 = Vector3.zero;
       // Debug.Log(iTile);
        switch (iTile)
        {
            case 0:
                //InveredAxis
                newV3 = new Vector3(0, iFixedY, Random.Range(BSConstants.Z_SIZE, BSConstants.Z_SIZE + BSConstants.X_SIZE ));
                break;
            case 1:
                newV3 = new Vector3(BSConstants.Z_SIZE * 2 + BSConstants.CENTER_DIM - 1, iFixedY, Random.Range(BSConstants.Z_SIZE, BSConstants.Z_SIZE + BSConstants.X_SIZE ));
                break;
            case 2:
                newV3 = new Vector3(Random.Range(BSConstants.Z_SIZE, BSConstants.Z_SIZE + BSConstants.X_SIZE ), iFixedY, BSConstants.Z_SIZE * 2 + BSConstants.CENTER_DIM - 1);
                break;
            case 3:
                newV3 = new Vector3(Random.Range(BSConstants.Z_SIZE, BSConstants.Z_SIZE + BSConstants.X_SIZE ), iFixedY, 0);
                break;
        }
        return newV3;
    }

    private void SpawnNewEnemy()
    {
        int iRandTile = GetRandomTile();
        Vector3 v3 = GetRandomSpawnPoint(iRandTile);
        //RepositionGameObject(GetDisabledEnemy(), v3, iRandTile);
        binstantiating = false;
    }

    private IEnumerator SpawnEnemyWave()
    {
        binstantiating = true;

        yield return new WaitForSeconds(5f);

        iRandomEnemyAmountPerWave = Random.Range(3, 7);

        for (int i = 0; i < iRandomEnemyAmountPerWave; i++)
        {
            SpawnNewEnemy();
        }
    }




}

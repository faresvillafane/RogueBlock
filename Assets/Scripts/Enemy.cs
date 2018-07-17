//#define CONSTANT_FORCE
//ALSO ENABLE ENEMY MOVEMENT X AND Z, on the prefab
using UnityEngine;
using System.Collections;
public class Enemy : MonoBehaviour {
    private bool bTouchingFloor = false;

    private bool bAdvancing = false;

    private Vector3 directionVector;
    private Vector3 targetVector;

    private bool bDisabledObject = true;

    private Hero hero;
    private bool bDamagedHero = false;

    private BSEnums.EnemyType enemyType = BSEnums.EnemyType.BASIC;
    private TerrainGen terrainGen;

    private Vector3 storedVelocity = Vector3.zero;

    private Collision lastCollision = null;

    public Material[] materials;

    private int iHealth = 10;
    // Use this for initialization
    void Start ()
    {
        hero = GameObject.FindGameObjectWithTag(BSConstants.TAG_HERO).GetComponent<Hero>();
        terrainGen = GameObject.FindGameObjectWithTag(BSConstants.TAG_GAME_CONTROLLER).GetComponent<TerrainGen>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!bDisabledObject)
        {
        #if CONSTANT_FORCE
            if (bAdvancing)
        #else
            if (!bAdvancing)
        #endif
            {
                StartCoroutine(Advance());
            }
#if CONSTANT_FORCE
            /*
            if(directionVector.x != 0)
            {
                this.transform.position = new Vector3(this.transform.position.x,
                                                    this.transform.position.y,
                                                    Mathf.Lerp(this.transform.position.z, (int) this.transform.position.z, BSConstants.ENEMY_LERP_SPEED));
            }
            else
            {
                this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, (int)this.transform.position.x, BSConstants.ENEMY_LERP_SPEED),
                                                    this.transform.position.y,
                                                    this.transform.position.z);
            }*/
#else
            this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, targetVector.x, BSConstants.ENEMY_LERP_SPEED),
                                                this.transform.position.y,
                                                Mathf.Lerp(this.transform.position.z, targetVector.z, BSConstants.ENEMY_LERP_SPEED));
#endif

            if (!IsInBounds())
            {
                StartCoroutine(DisableObject());
            }
        }
	}

    private IEnumerator Advance()
    {
        bAdvancing = true;

        yield return new WaitForSeconds(BSConstants.ADVANCE_DELAY);

        targetVector +=  directionVector;
        
        bAdvancing = false;

    }


    public void SetFromTileVelocity(int iTile)
    {
        //Debug.Log("iTile: " + iTile);
        switch (iTile)
        {
            case 0:
                SetDirection(Vector3.right);
                break;
            case 1:
                SetDirection(Vector3.left);
                break;
            case 2:
                SetDirection(Vector3.back);
                break;
            case 3:
                SetDirection(Vector3.forward);
                break;
        }
    }


    public void SetDirection(Vector3 v3)
    {
        directionVector = v3;
#if CONSTANT_FORCE
        //targetVector = targetVector +  directionVector * 3;
        storedVelocity = directionVector;
        this.GetComponent<Rigidbody>().velocity = storedVelocity;
#endif

    }
        
    void OnCollisionEnter(Collision collision)
    {

        CheckCollsion(collision);
#if CONSTANT_FORCE
        if(lastCollision != null && lastCollision != collision)
        {
            this.GetComponent<Rigidbody>().velocity = storedVelocity;
        }
#endif
        if (collision.gameObject.tag.Equals(BSConstants.TAG_TILE))
        {
            if(this.enemyType == BSEnums.EnemyType.FLOOR_FREEZING)
            {
                collision.gameObject.GetComponent<Tile>().SetTileType(BSEnums.TileType.FIXED, materials[1]);
            }
            if (this.enemyType == BSEnums.EnemyType.FLOOR_INVERTED)
            {
                collision.gameObject.GetComponent<Tile>().SetTileType(BSEnums.TileType.INVERSE_ROTATING, materials[2]);
            }
        }
#if CONSTANT_FORCE
        lastCollision = collision;
#endif
    }

    void OnTriggerEnter(Collider collision)
    {

        if(collision.gameObject.tag.Equals(BSConstants.TAG_WATER))
        {
            StartCoroutine(DisableObject(BSConstants.DISABLE_OBJECT_DELAY));
        }
        else if (collision.gameObject.tag.Equals(BSConstants.TAG_TRAP))
        {
            StartCoroutine(DisableObject(BSConstants.DISABLE_OBJECT_DELAY));
        }

    }

    private IEnumerator DisableObject(float timer = .1f)
    {
        yield return new WaitForSeconds(timer);
        bDisabledObject = true;
        //this.GetComponent<SphereCollider>().enabled = false;
        //this.GetComponent<Rigidbody>().isKinematic = true;
        //this.transform.position = BSConstants.OBJECT_OUT_OF_SCREEN;
    }

    public void Reset(Vector3 position)
    {
        this.transform.position = position;
        targetVector = transform.position;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<SphereCollider>().enabled = true;
        bDisabledObject = false;
        bDamagedHero = false;
        BSEnums.EnemyType randET = GetRandomEnemyType();
        SetEnemy(randET, materials[(int)randET]);
        iHealth = 10;
    }

    public void SetEnemy(BSEnums.EnemyType et, Material mat)
    {
        enemyType = et;
        this.GetComponent<Renderer>().sharedMaterial = mat;
        
    }

    private BSEnums.EnemyType GetRandomEnemyType()
    {
        float fRandomValue = Random.value;
        for(int i = 0; i <= (int)BSEnums.EnemyType.FLOOR_INVERTED; i++)
        {
            if (fRandomValue <= BSConstants.ENEMY_SPAWN_CHANCE[i])
            {
                return (BSEnums.EnemyType)i;
            }
        }
        return BSEnums.EnemyType.BASIC;
    }

    private void CheckCollsion(Collision collision)
    {
        if (collision.gameObject.tag.Equals(BSConstants.TAG_CENTER_TILE))
        {
            if (!bDamagedHero)
            {
                bDamagedHero = true;
            }
            StartCoroutine(DisableObject(.3f));
            RepositionOutOfBounds();
        }
        else
        {
            if (!bDisabledObject)
            {
                bTouchingFloor = true;
            }
        }
    }
    
    void OnCollisionStay(Collision collision)
    {
        CheckCollsion(collision);
    }
    void OnCollisionExit(Collision collision)
    {
        bTouchingFloor = false;
    }

    public void ChangePositionX(bool bPositive)
    {
        if (!bDisabledObject )
        {
#if CONSTANT_FORCE
            ApplyForce((bPositive)? Vector3.right: Vector3.left);
#else
            if (terrainGen.ShouldEnemyMove(this.transform.position))
            {
                ChangePosition(new Vector3((bPositive) ? this.transform.position.x + 1 : this.transform.position.x - 1, this.transform.position.y, this.transform.position.z));
            }
            else if (terrainGen.ShouldEnemyMoveInverse(this.transform.position))
            {
                ChangePosition(new Vector3((!bPositive) ? this.transform.position.x + 1 : this.transform.position.x - 1, this.transform.position.y, this.transform.position.z));
            }
#endif
        }
    }

    public void ChangePositionZ(bool bPositive)
    {
        if (!bDisabledObject)
        {
#if CONSTANT_FORCE
            ApplyForce((bPositive) ? Vector3.forward : Vector3.back);
#else
            if (terrainGen.ShouldEnemyMove(this.transform.position))
            {
                ChangePosition(new Vector3(this.transform.position.x, this.transform.position.y, (bPositive) ? this.transform.position.z + 1 : this.transform.position.z - 1));
            }
            else if (terrainGen.ShouldEnemyMoveInverse(this.transform.position))
            {
                ChangePosition(new Vector3(this.transform.position.x, this.transform.position.y, (!bPositive) ? this.transform.position.z + 1 : this.transform.position.z - 1));
            }
#endif
        }
    }

    public void ApplyForce(Vector3 direction)
    {
        Vector3 YForce = Vector3.up * 150;
        storedVelocity = this.GetComponent<Rigidbody>().velocity;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().AddForce(YForce + direction * 100);
    }

    public void ChangePosition(Vector3 newPosition)
    {
        targetVector = newPosition;
    }

    public bool IsInBounds()
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position, Vector3.down * 5, out hit);
        if(hit.collider != null)
        {
            return hit.collider.tag != BSConstants.TAG_WATER;
        }
        else
        {
            return false;
        }
    }

    public bool LerpAtTarget(Vector3 actual, Vector3 target)
    {
        return Vector3.Distance(actual, target) <= .1f;
    }

    public bool IsDisabled()
    {
        return bDisabledObject;
    }

    public void Reposition(Vector3 vNewPos)
    {
        this.transform.position = vNewPos;
    }

    public void RepositionOutOfBounds()
    {
        Reposition(BSConstants.OBJECT_OUT_OF_SCREEN);
        this.GetComponent<Rigidbody>().isKinematic = true;
    }
}

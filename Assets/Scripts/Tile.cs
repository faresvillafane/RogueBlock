using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public GameObject goTopTile, goBotTile;
    private BSEnums.TileType tileType = BSEnums.TileType.ROTATING;
    private BSEnums.TileType prevTileType = BSEnums.TileType.ROTATING;
    private Quaternion targetRotation = Quaternion.Euler(Vector3.zero);
    private float fRotatingSpeed;

    private Material initMaterial;

    private bool bFinishedRotating = true;

    private Vector3 v3PositionInMatrix;

    // Use this for initialization
    void Start ()
    {
       // initMaterial = this.GetComponent<Renderer>().sharedMaterial;

    }

    public void SetPositionInMatrix(Vector3 newPosition)
    {
        v3PositionInMatrix = newPosition;
    }
	
    public void SetRotationSpeed(float fNewSpeed)
    {
        fRotatingSpeed = fNewSpeed;
    }
    public void SetTargetRotation(Quaternion qNewTargetRotation)
    {
        targetRotation = qNewTargetRotation;
    }
    public float GetRotationSpeed()
    {
        return fRotatingSpeed;
    }

    public Quaternion GetTargetRotation()
    {
        return targetRotation;
    }

    public BSEnums.TileType GetTileType()
    {
        return tileType;
    }

    public void SetTileType(BSEnums.TileType tt)
    {
        tileType = tt;
        //StartCoroutine(RecoverTileType());
    }

    public void SetTileType(BSEnums.TileType tt, Material newMat)
    {
        tileType = tt;
        this.GetComponent<Renderer>().sharedMaterial = newMat;
        StartCoroutine(RecoverTileType());
    }

    private IEnumerator RecoverTileType()
    {
        yield return new WaitForSeconds(BSConstants.RECOVER_TIME_TILE);
        tileType = BSEnums.TileType.ROTATING;
        this.GetComponent<Renderer>().sharedMaterial = initMaterial;

    }

    void Update()
    {
        RotateMovingTiles();
    }

    private void RotateMovingTiles()
    {
        
        if (!bFinishedRotating)
        {
            bFinishedRotating = true;

            if (this.tileType == BSEnums.TileType.ROTATING)
            {
                bFinishedRotating &= RotateTile();
            }
        }
    }
        


    private bool RotateTile()
    {
        if (Quaternion.Angle(this.transform.rotation, this.GetTargetRotation()) >= 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, this.GetTargetRotation(), this.GetRotationSpeed());
            return false;
        }
        else
        {
            transform.rotation = GetTargetRotation();
            Debug.Log("Finish Rotating");
            return true;
        }

    }


    public void GetNextMovement(BSEnums.SwipeDirection swipe)
    {
        if (tileType == BSEnums.TileType.ROTATING)
        {
            Transform tile = this.transform;
            Vector3 nextSpinVector = Vector3.zero;
            if (tileType == BSEnums.TileType.ROTATING)
            {
                switch (swipe)
                {
                    case BSEnums.SwipeDirection.FORWARD:
                        nextSpinVector = TurnUp(tile);
                        break;
                    case BSEnums.SwipeDirection.BACK:
                        nextSpinVector = -TurnUp(tile);
                        break;
                    case BSEnums.SwipeDirection.RIGHT:
                        nextSpinVector = TurnRight(tile);
                        break;
                    case BSEnums.SwipeDirection.LEFT:
                        nextSpinVector = -TurnRight(tile);
                        break;
                }
            }
            else
            {
                switch (swipe)
                {
                    case BSEnums.SwipeDirection.FORWARD:
                        nextSpinVector = -TurnUp(tile);
                        break;
                    case BSEnums.SwipeDirection.BACK:
                        nextSpinVector = TurnUp(tile);
                        break;
                    case BSEnums.SwipeDirection.RIGHT:
                        nextSpinVector = -TurnRight(tile);
                        break;
                    case BSEnums.SwipeDirection.LEFT:
                        nextSpinVector = TurnRight(tile);
                        break;
                }
            }
            if(GetComponentInChildren<Enemy>() != null)
            {
                GetComponentInChildren<Enemy>().bOnTop = !GetComponentInChildren<Enemy>().bOnTop;
            }
            //pregunta al script y no a la matriz por si un enemigo la cambio
            if (tileType == BSEnums.TileType.ROTATING
                || tileType == BSEnums.TileType.INVERSE_ROTATING)
            {
                SetTargetRotation(GetTargetRotation() * Quaternion.AngleAxis(179.9f, nextSpinVector));
            }
            bFinishedRotating = false;
        }

    }

    private Vector3 TurnUp(Transform tile)
    {
        Vector3 newRotation = Vector3.zero;
        if (VectorSameDirection(tile.right, Vector3.right))
        {
            newRotation = (tile.right.x > 0) ? Vector3.right : Vector3.left;
        }

        else if (VectorSameDirection(tile.right, Vector3.up))
        {

            newRotation = Vector3.up;

            if (VectorSameDirection(tile.up, Vector3.right))
            {
                if (tile.up.x < 0)
                {
                    newRotation = Vector3.down;
                }
            }
            else if (VectorSameDirection(tile.forward, Vector3.right))
            {
                if (tile.forward.x < 0)
                {
                    newRotation = Vector3.back;
                }
                else
                {
                    newRotation = Vector3.forward;
                }
            }
        }
        else if (VectorSameDirection(tile.right, Vector3.forward))
        {
            newRotation = Vector3.up;

            if (VectorSameDirection(tile.up, Vector3.right))
            {
                if (tile.up.x < 0)
                {
                    newRotation = Vector3.down;
                }
            }
            else if (VectorSameDirection(tile.forward, Vector3.right))
            {
                if (tile.forward.x < 0)
                {
                    newRotation = Vector3.back;
                }

                else
                {
                    newRotation = Vector3.forward;
                }
            }
        }
        return newRotation;
    }

    private Vector3 TurnRight(Transform tile)
    {
        Vector3 newRotation = Vector3.zero;
        if (VectorSameDirection(tile.forward, Vector3.forward))
        {
            newRotation = (tile.forward.z < 0) ? Vector3.forward : Vector3.back;
        }

        else if (VectorSameDirection(tile.forward, Vector3.up))
        {

            newRotation = Vector3.down;

            if (VectorSameDirection(tile.up, Vector3.forward))
            {
                if (tile.up.z < 0)
                {
                    newRotation = Vector3.up;
                }
                else
                {
                    newRotation = Vector3.down;
                }
            }
            else if (VectorSameDirection(tile.right, Vector3.forward))
            {
                if (tile.right.z < 0)
                {
                    newRotation = Vector3.right;
                }
                else
                {
                    newRotation = Vector3.left;
                }
            }
        }
        else if (VectorSameDirection(tile.forward, Vector3.right))
        {
            newRotation = Vector3.down;

            if (VectorSameDirection(tile.up, Vector3.forward))
            {
                if (tile.up.z < 0)
                {
                    newRotation = Vector3.up;
                }
                else
                {
                    newRotation = Vector3.down;
                }
            }
            else if (VectorSameDirection(tile.right, Vector3.forward))
            {
                if (tile.right.z < 0)
                {
                    newRotation = Vector3.right;
                }

                else
                {
                    newRotation = Vector3.left;
                }
            }
        }

        return newRotation;
    }

    private bool VectorSameDirection(Vector3 v1, Vector3 v2)
    {
        return (Mathf.Abs(v1.x) - Mathf.Abs(v2.x)) < BSConstants. MIN_DEGREE_ANGLE && (Mathf.Abs(v1.y) - Mathf.Abs(v2.y)) < BSConstants.MIN_DEGREE_ANGLE && (Mathf.Abs(v1.z) - Mathf.Abs(v2.z)) < BSConstants.MIN_DEGREE_ANGLE;
    }

}

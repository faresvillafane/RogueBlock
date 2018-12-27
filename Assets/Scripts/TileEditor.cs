using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TileEditor : MonoBehaviour {

    public BSEnums.TileType topTileType = BSEnums.TileType.ROTATING, botTileType = BSEnums.TileType.ROTATING;
    private BSEnums.TileType prevTopTileType = BSEnums.TileType.ROTATING, prevBotTileType = BSEnums.TileType.ROTATING;
    public bool bOnTop = true;
    private bool bPrevBOnTop;

    private TileGroupSave tgs;

    private MeshRenderer goTopTile;
    private MeshRenderer goBotTile;
    // Use this for initialization
    void Start ()
    {
        bPrevBOnTop = bOnTop;
        prevTopTileType = topTileType;
        prevBotTileType = botTileType;
        tgs = GetComponentInParent<TileGroupSave>();
        goTopTile = GetComponentsInChildren<MeshRenderer>()[0];
        goBotTile = GetComponentsInChildren<MeshRenderer>()[1];
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(bPrevBOnTop != bOnTop)
        {
            if (bOnTop)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0,0,180);
            }
            bPrevBOnTop = bOnTop;
        }

        if (prevTopTileType != topTileType)
        {
            goTopTile.enabled = true;
            //goBotTile.enabled = true;

            if (topTileType == BSEnums.TileType.AIR)
            {
                goTopTile.enabled = false;
                goBotTile.enabled = false;
                prevBotTileType = botTileType = BSEnums.TileType.AIR;

            }
            else if (topTileType == BSEnums.TileType.FIXED)
            {
                goTopTile.material = tgs.fixedMaterial;

            }
            else if (topTileType == BSEnums.TileType.TRAP)
            {
                goTopTile.material = tgs.trapMaterial;

            }
            else if (topTileType == BSEnums.TileType.ROTATING)
            {
                goTopTile.material = tgs.rotatingMaterialTop;

            }

            prevTopTileType = topTileType;
        }

        if (prevBotTileType != botTileType)
        {
            goBotTile.enabled = true;
            //goTopTile.enabled = true;

            if (botTileType == BSEnums.TileType.AIR)
            {
                goTopTile.enabled = false;
                goBotTile.enabled = false;
                prevTopTileType = topTileType = BSEnums.TileType.AIR;
            }
            else if (botTileType == BSEnums.TileType.FIXED)
            {
                goBotTile.material = tgs.fixedMaterial;

            }
            else if (botTileType == BSEnums.TileType.TRAP)
            {
                goBotTile.material = tgs.trapMaterial;

            }
            else if (botTileType == BSEnums.TileType.ROTATING)
            {
                goBotTile.material = tgs.rotatingMaterialBot;

            }

            prevBotTileType = botTileType;
        }
    }
}

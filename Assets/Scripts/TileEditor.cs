using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TileEditor : MonoBehaviour {

    public BSEnums.TileType topTileType = BSEnums.TileType.ROTATING, botTileType = BSEnums.TileType.ROTATING;
    private BSEnums.TileType prevTopTileType = BSEnums.TileType.ROTATING, prevBotTileType = BSEnums.TileType.ROTATING;
    private GameObject goOnTop, goOnBot;
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
        //if(bPrevBOnTop != bOnTop)
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
            if(goOnTop != null)
            {
                DestroyImmediate(goOnTop);
            }
            //goBotTile.enabled = true;

            if (topTileType == BSEnums.TileType.AIR)
            {
                goTopTile.enabled = false;
                goBotTile.enabled = false;
                if (goOnBot != null)
                {
                    DestroyImmediate(goOnBot);
                }
                prevBotTileType = botTileType = BSEnums.TileType.AIR;

            }
            else if (topTileType == BSEnums.TileType.FIXED)
            {
                goTopTile.material = tgs.fixedMaterial;

            }
            else if (topTileType == BSEnums.TileType.TRAP)
            {
                goTopTile.material = tgs.trapMaterial;
                goOnTop = Instantiate(tgs.prefTrap, goTopTile.transform);
                goOnTop.transform.localScale = BSConstants.V3_SPIKE_SCALE;
                goOnTop.transform.localPosition = BSConstants.V3_SPIKE_POSITION_IN_TILE;

            }
            else if (topTileType == BSEnums.TileType.ROTATING)
            {
                goTopTile.material = tgs.rotatingMaterialTop;

            }
            else if(topTileType == BSEnums.TileType.ENEMY)
            {
                goOnTop = Instantiate(tgs.prefEnemy, goTopTile.transform);
                goTopTile.material = tgs.rotatingMaterialTop;

                goOnTop.transform.localScale = BSConstants.V3_ENEMY_SCALE;
                goOnTop.transform.localPosition = BSConstants.V3_ENEMY_POSITION_IN_TILE;
            }
            else if (topTileType == BSEnums.TileType.ENTRANCE
                || topTileType == BSEnums.TileType.EXIT)
            {
                goOnTop = Instantiate(tgs.prefEntrance, goTopTile.transform);
                goTopTile.material = tgs.rotatingMaterialTop;

                goOnTop.transform.localScale = BSConstants.V3_ENTRANCE_SCALE;
                goOnTop.transform.localPosition = BSConstants.V3_ENTRANCE_POSITION_IN_TILE;
            }

            prevTopTileType = topTileType;
        }

        if (prevBotTileType != botTileType)
        {
            goBotTile.enabled = true;
            //goTopTile.enabled = true;
            if (goOnBot != null)
            {
                DestroyImmediate(goOnBot);
            }
            if (botTileType == BSEnums.TileType.AIR)
            {
                goTopTile.enabled = false;
                goBotTile.enabled = false;
                if (goOnTop != null)
                {
                    DestroyImmediate(goOnTop);
                }

                prevTopTileType = topTileType = BSEnums.TileType.AIR;
            }
            else if (botTileType == BSEnums.TileType.FIXED)
            {
                goBotTile.material = tgs.fixedMaterial;

            }
            else if (botTileType == BSEnums.TileType.TRAP)
            {
                goBotTile.material = tgs.trapMaterial;
                goOnBot = Instantiate(tgs.prefTrap, goBotTile.transform);
                goOnBot.transform.localScale = BSConstants.V3_SPIKE_SCALE;
                goOnBot.transform.localPosition = BSConstants.V3_SPIKE_POSITION_IN_TILE;

            }
            else if (botTileType == BSEnums.TileType.ROTATING)
            {
                goBotTile.material = tgs.rotatingMaterialBot;

            }

            else if (botTileType == BSEnums.TileType.ENEMY)
            {
                goOnBot = Instantiate(tgs.prefEnemy, goBotTile.transform);
                goBotTile.material = tgs.rotatingMaterialBot;
                goOnBot.transform.localScale    = BSConstants.V3_ENEMY_SCALE;
                goOnBot.transform.localPosition = BSConstants.V3_ENEMY_POSITION_IN_TILE;
            }
            else if (botTileType == BSEnums.TileType.ENTRANCE
                    || botTileType == BSEnums.TileType.EXIT)
            {
                goOnBot = Instantiate(tgs.prefEntrance, goBotTile.transform);
                goBotTile.material = tgs.rotatingMaterialBot;

                goOnBot.transform.localScale = BSConstants.V3_ENTRANCE_SCALE;
                goOnBot.transform.localPosition = BSConstants.V3_ENTRANCE_POSITION_IN_TILE;
            }

            prevBotTileType = botTileType;
        }
    }
}

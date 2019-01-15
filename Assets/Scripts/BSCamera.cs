using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSCamera : MonoBehaviour {
    private Hero hero;
    public bool bRotate = false;
    // Use this for initialization
    public bool bSeekPlayer = true;
    void Start ()
    {
		
        hero = GameObject.FindGameObjectWithTag(BSConstants.TAG_HERO).GetComponent<Hero>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (bSeekPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, hero.transform.position + new Vector3(0, 8, -4), Time.deltaTime * 3);
        }
        if (bRotate)
        {
            bRotate = false;
            RotateToOtherSide();
        }
	}

    public void RotateToOtherSide()
    {
        transform.RotateAround(hero.transform.position, Vector3.forward, 180);
    }
}

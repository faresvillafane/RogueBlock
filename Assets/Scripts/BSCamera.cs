using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSCamera : MonoBehaviour {
    private Hero hero;

    // Use this for initialization
    void Start ()
    {
		
        hero = GameObject.FindGameObjectWithTag(BSConstants.TAG_HERO).GetComponent<Hero>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, hero.transform.position + new Vector3(0,8,-4), Time.deltaTime * 3);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraController : MonoBehaviour {

    public CAndy mAndy;

    private CVector lastPlayerPosition;
    private float distanceToMove;
    
    // Use this for initialization
	void Start () {

        mAndy = CGame.inst().getAndy();

        if (mAndy == null)
        {
            lastPlayerPosition = new CVector(0, 0, 0);
        }
        else
        {
            lastPlayerPosition = mAndy.getPos();
        }
        //lastPlayerPosition = thePlayer.getPos();

    }
	
	// Update is called once per frame
	void Update () {

        if (mAndy == null)
        {
            distanceToMove = 5;
        }
        else
        {
            distanceToMove = mAndy.getX() - lastPlayerPosition.x;
        }

        //distanceToMove = thePlayer.getX() - lastPlayerPosition.x;

        transform.position = new Vector3(transform.position.x + distanceToMove, transform.position.y, transform.position.z);

        if (mAndy == null)
        {
            lastPlayerPosition = new CVector(0, 0, 0);
        }
        else
        {
            lastPlayerPosition = mAndy.getPos();
        }

        //lastPlayerPosition = thePlayer.getPos();

    }
}

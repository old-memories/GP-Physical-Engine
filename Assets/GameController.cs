using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject bevel;
    public GameObject plane;
    public GameObject ground;
    public GameObject ball;

    public float sensitive;

    public float right;

    public BevelController bevelController;
    public PlaneController planeController;

    private Vector3 initBallPos;

    private Vector3 initBevelPos;

    private bool newball;

	// Use this for initialization
	void Start () {
        bevelController = bevel.GetComponent<BevelController>();
        planeController = plane.GetComponent<PlaneController>();
        initBallPos = bevel.transform.position + new Vector3(0, 5, 0);
        initBevelPos = bevel.transform.position;
        newball = false;
        StartCoroutine(MovePlane());
        StartCoroutine(NewBall());
    }

    // Update is called once per frame
    void Update () {
        if (newball){
            bevelController.gameObject.transform.position = initBevelPos+ new Vector3( 0,Random.Range(-2.0f,2.0f) ,0);
            Instantiate(ball, initBallPos, transform.rotation,transform);
            newball = false;
        }
        
    }

    IEnumerator MovePlane()
    {
        while (true)
        {
            right = (Input.GetKey("d") ? 1.0f : 0) - (Input.GetKey("a") ? 1.0f : 0);
            planeController.gameObject.transform.Translate(0, 0, right*sensitive);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator NewBall()
    {
        while (true)
        {
            newball = true;
            yield return new WaitForSeconds(3);
        }
      
    }
}

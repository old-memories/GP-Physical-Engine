using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject bevelLeft;
    public GameObject bevelRight;
    public GameObject plane;
    public GameObject ground;
    public GameObject ball;

    public float sensitive;

    public float right;

    public BevelController bevelLeftController;
    public BevelController bevelRightController;

    public PlaneController planeController;

    private Vector3 initBallLeftPos;
    private Vector3 initBallRightPos;

    private Vector3 initBevelLeftPos;
    private Vector3 initBevelRightPos;

    private bool newball;

	// Use this for initialization
	void Start () {
        bevelLeftController = bevelLeft.GetComponent<BevelController>();
        bevelRightController = bevelRight.GetComponent<BevelController>();

        planeController = plane.GetComponent<PlaneController>();
        initBevelLeftPos = bevelLeft.transform.position;
        initBevelRightPos = bevelRight.transform.position;
        initBallLeftPos = initBevelLeftPos + new Vector3(0, 5, 0);
        initBallRightPos = initBevelRightPos + new Vector3(0, 5, 0);
       

        newball = false;
        StartCoroutine(MovePlane());
        StartCoroutine(NewBall());
    }

    // Update is called once per frame
    void Update () {
        if (newball){
            bevelLeftController.gameObject.transform.position = initBevelLeftPos + new Vector3( 0,Random.Range(-2.0f,2.0f) ,0);
            bevelRightController.gameObject.transform.position = initBevelRightPos + new Vector3(0, Random.Range(-2.0f, 2.0f), 0);

            Instantiate(ball, initBallLeftPos, transform.rotation,transform);
            Instantiate(ball, initBallRightPos, transform.rotation, transform);
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
            yield return new WaitForSeconds(5);
        }
      
    }
}

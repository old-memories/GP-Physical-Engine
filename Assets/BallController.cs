using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public PhysicsManager physicsManager;


    [Range(1.0f, 10.0f)]
    public float mass = 1.0f;

    [Range(0.0f, 1.0f)]
    public float bounciness = 0.0f;

    public BounceCombineType bounceCombineType;

    private Vector3 vel = Vector3.zero;


    private Vector3 initVel;
    private Vector3 initPos;


    
    
    // Use this for initialization
	void Start () {
        initPos = transform.position;
        initVel = vel;
        name = physicsManager.AddObject("ball", mass, Vector3.zero);
        physicsManager.SetObject(name, mass, vel);
        physicsManager.SetObject(name, bounciness, bounceCombineType);
        physicsManager.AddForce(name, "Earth", ForceType.GRAVITY, physicsManager.CalGravity(mass));
        StartCoroutine(UpdatePosition());
	}
	
	// Update is called once per frame
	void Update () {
        
       
	}

    IEnumerator UpdatePosition()
    {
        while (true)
        {
            if (physicsManager.started)
            {
                physicsManager.GetObject(name, out mass, out vel);
                transform.position += vel * physicsManager.timeStep;
                Vector3 sumForce = physicsManager.CalForce(name);
                physicsManager.GetObject(name, out mass, out vel);
                //Debug.Log("sunForce: " + sumForce);
                Vector3 acc = physicsManager.CalAcc(sumForce, mass);
                vel += acc * physicsManager.timeStep;
                physicsManager.SetObject(name, mass, vel);
                //Debug.Log("vel: " + vel);
            }
            if (!physicsManager.started)
            {
                transform.position = initPos;
                vel = initVel;
                physicsManager.SetObject(name, mass, vel);
                physicsManager.SetObject(name, bounciness, bounceCombineType);
            }
            yield return new WaitForSeconds(physicsManager.timeStep);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bevel")
        {
            Debug.Log("Create Normal Force");
            Vector3 v2;
            physicsManager.CalHit(name, other.gameObject.transform.parent.gameObject.name,transform.position, other.gameObject.transform.parent.gameObject.transform.position, out vel, out v2);
            Debug.Log("Hit Calculate: v1: " + vel + " v2: " + v2);
            physicsManager.AddForce(name, other.gameObject.transform.parent.gameObject.name,ForceType.NORMAL);
        }
        if (other.gameObject.tag == "plane")
        {
            Debug.Log("Create Normal Force");
            Vector3 v2;
            physicsManager.CalHit(name, other.gameObject.name,transform.position,other.gameObject.transform.position,out vel,out v2);
            Debug.Log("Hit Calculate: v1: " + vel + " v2: " + v2);

            physicsManager.AddForce(name, other.gameObject.name, ForceType.NORMAL);
        }
        if (other.gameObject.tag == "ball")
        {
            tag = "none";
            Debug.Log("Hit two balls");
            Vector3 v2;
            physicsManager.CalHit(name, other.gameObject.name, transform.position, other.gameObject.transform.position, out vel, out v2);
            Debug.Log("Hit Calculate: v1: " + vel + " v2: " + v2);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "bevel")
        {
            Debug.Log("Remove Normal Force");
            physicsManager.RemoveForce(name, other.gameObject.transform.parent.gameObject.name, ForceType.NORMAL);
        }
        if (other.gameObject.tag == "plane")
        {
            Debug.Log("Remove Normal Force");

            physicsManager.RemoveForce(name, other.gameObject.name, ForceType.NORMAL);
        }
        if (other.gameObject.tag == "ball")
        {
            tag = "ball";
        }

    }

   
}

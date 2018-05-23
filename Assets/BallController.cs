using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public PhysicsManager physicsManager;

    [Range(1.0f, 10.0f)]
    public float mass = 1.0f;

    public Vector3 vel = Vector3.zero;

    public bool started;

    private Vector3 initVel;
    private Vector3 initPos;


    
    
    // Use this for initialization
	void Start () {
        started = false;
        initPos = transform.position;
        initVel = vel;
        name = physicsManager.AddObject("ball", mass, vel);
        physicsManager.AddForce(name, "Earth", ForceType.GRAVITY, physicsManager.CalGravity(mass));

	}
	
	// Update is called once per frame
	void Update () {
        if (started)
        {
            transform.position += vel * Time.deltaTime;
            Vector3 sumForce = physicsManager.CalForce(name);
            Debug.Log("sunForce: " + sumForce);
            Vector3 acc = physicsManager.CalAcc(sumForce, mass);
            vel += acc * Time.deltaTime;
        }
        if (!started)
        {
            transform.position = initPos;
            vel = initVel;
        }
       
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bevel")
        {
            Debug.Log("Create Normal Force");
            physicsManager.AddForce(name, other.gameObject.transform.parent.gameObject.name,ForceType.NORMAL);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "bevel")
        {
            Debug.Log("Remove Normal Force");
            physicsManager.RemoveForce(name, other.gameObject.transform.parent.gameObject.name, ForceType.NORMAL);
        }
    }

    public void OnStartButtonClicked()
    {
        started = !started;
    }
}

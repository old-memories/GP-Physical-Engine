using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHit : MonoBehaviour {

    public Rigidbody rigid;

    public Vector3 force;
    
    // Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(force, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update () {
	}
}

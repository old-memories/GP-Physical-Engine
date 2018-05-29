using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BevelController : MonoBehaviour {

    public PhysicsManager physicsManager;

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;

    [Range(0,Mathf.PI/2.0f)]
    public float angle = Mathf.PI/4.0f;
    [Range(1,5)]
    public float scale = 1;

    public Vector3 normal;

    private float initPosY;

    private const  float bevelMass =-1;

    [Range(0.0f, 1.0f)]
    public float bounciness = 0.0f;

    public BounceCombineType bounceCombineType;

    public Vector3 CalNormal()
    {
        Vector3 v1 = p1.transform.position - p2.transform.position;
        Vector3 v2 = p2.transform.position - p3.transform.position;
        return Vector3.Normalize(Vector3.Cross(v1, v2));
    }

    private void Awake()
    {
        physicsManager = transform.parent.GetComponent<PhysicsManager>();
    }


    // Use this for initialization
    void Start () {
        initPosY = transform.localPosition.y;
        transform.localScale = new Vector3(transform.localScale.x, scale*Mathf.Tan(angle), scale);
        transform.localPosition = new Vector3(transform.position.x, initPosY * transform.localScale.y, transform.position.z);
        angle = Mathf.Atan2(transform.localScale.y,transform.localScale.z);

        normal = CalNormal();
        name = physicsManager.AddObject("bevel", bevelMass, normal);
        
        physicsManager.SetObject(name, normal);
        physicsManager.SetObject(name, bounciness, bounceCombineType);
    }

    // Update is called once per frame
    void Update () {
        transform.localScale = new Vector3(transform.localScale.x, scale * Mathf.Tan(angle), scale);
        transform.localPosition = new Vector3(transform.position.x, initPosY * transform.localScale.y, transform.position.z);
        angle = Mathf.Atan2(transform.localScale.y, transform.localScale.z);
        normal = CalNormal();
        //Debug.Log("normal: " + normal);
        physicsManager.SetObject(name, normal);
        physicsManager.SetObject(name, bounciness, bounceCombineType);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            physicsManager.AddForce(name, other.gameObject.name, ForceType.NORMAL);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            physicsManager.RemoveForce(name, other.gameObject.name, ForceType.NORMAL);
        }
    }
}

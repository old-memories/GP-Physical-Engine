using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour {

    public PhysicsManager physicsManager;

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;

    public Vector3 normal;

    private const float planeMass = -1;

    [Range(0.0f, 1.0f)]
    public float bounciness = 0.0f;

    public BounceCombineType bounceCombineType;


    [Range(1, 5)]
    public float scale = 1; //ONLY X and Z 

    private Vector3 initScale;

    public Vector3 CalNormal()
    {
        Vector3 v1 = p1.transform.position - p2.transform.position;
        Vector3 v2 = p2.transform.position - p3.transform.position;
        return Vector3.Normalize(Vector3.Cross(v1, v2));
    }

    // Use this for initialization
    void Start () {
        initScale = transform.localScale;
        normal = CalNormal();
        transform.localScale = scale * (initScale - Vector3.Dot(initScale, normal) * initScale);
        name = physicsManager.AddObject("plane", planeMass, normal);
        physicsManager.SetObject(name, normal);
        physicsManager.SetObject(name,bounciness,bounceCombineType);


    }

    // Update is called once per frame
    void Update () {
        normal = CalNormal();
        transform.localScale = scale*(initScale - Vector3.Dot(initScale, normal) * initScale);
        physicsManager.SetObject(name, normal);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BevelController : MonoBehaviour {

    public PhysicsManager physicsManager;

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject H;
    public GameObject S;
    public GameObject L;

   public Vector3 initScaleH;
   public Vector3 initScaleL;
   public Vector3 initScaleS;

 
    [Range(1,5)]
    public float height = 1;
    [Range(1, 5)]
    public float length = 1;

    public Vector3 normal;


    private const  float bevelMass =-1;

    [Range(0.0f, 1.0f)]
    public float bounciness = 0.0f;

    public BounceCombineType bounceCombineType;

    [Range(0.0f, 1.0f)]
    public float friction = 0.0f;

    public FrictionCombineType frictionCombineType;


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

        initScaleH = H.transform.localScale;
        initScaleL = L.transform.localScale;
        initScaleS = S.transform.localScale;


 


        height = initScaleH.y;
        length = initScaleL.y;

        //transform.localScale = new Vector3(transform.localScale.x, scale*Mathf.Tan(angle), scale);
        //transform.localPosition = new Vector3(transform.position.x, initPosY * transform.localScale.y, transform.position.z);
        //angle = Mathf.Atan2(transform.localScale.y,transform.localScale.z);

        normal = CalNormal();
        name = physicsManager.AddObject("bevel", bevelMass, normal);
        
        physicsManager.SetObject(name, normal);
        physicsManager.SetObject(name, bounciness, bounceCombineType);
        physicsManager.SetObject(name, friction, frictionCombineType);

    }

    // Update is called once per frames.
    void Update () {
        H.transform.localPosition = new Vector3(H.transform.localPosition.x, H.transform.localPosition.y+(height - H.transform.localScale.y) * 0.5f, H.transform.localPosition.z);
        L.transform.localPosition = new Vector3(L.transform.localPosition.x, L.transform.localPosition.y, L.transform.localPosition.z - (length - L.transform.localScale.y) * 0.5f);
        S.transform.localPosition = new Vector3(S.transform.localPosition.x, S.transform.localPosition.y + (height-H.transform.localScale.y) * 0.5f, S.transform.localPosition.z - (length-L.transform.localScale.y) * 0.5f);
        H.transform.localScale = new Vector3(H.transform.localScale.x, height, H.transform.localScale.z);
        L.transform.localScale = new Vector3(L.transform.localScale.x, length, L.transform.localScale.z);
        S.transform.localScale = new Vector3(S.transform.localScale.x, Mathf.Sqrt(height * height + length * length), S.transform.localScale.z);
        S.transform.localEulerAngles = new Vector3(Mathf.Atan(length / height)*Mathf.Rad2Deg, S.transform.rotation.y, S.transform.rotation.z);
        // transform.localScale = new Vector3(transform.localScale.x, scale * Mathf.Tan(angle), scale);
        //transform.localPosition = new Vector3(transform.position.x, initPosY * transform.localScale.y, transform.position.z);
        //angle = Mathf.Atan2(transform.localScale.y, transform.localScale.z);
        normal = CalNormal();
        //Debug.Log("normal: " + normal);
        physicsManager.SetObject(name, normal);
        physicsManager.SetObject(name, bounciness, bounceCombineType);
        physicsManager.SetObject(name, friction, frictionCombineType);


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

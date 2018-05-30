using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RopeController : MonoBehaviour {

    public float HighAllow = 0.2f;
    public float LowAllow = 0.1f;
    public int nodeCount = 30;
    LineRenderer lineRen;
    List<GameObject> nodes;
    //List<Vector3> pos;

    public GameObject stdNode;
    //public Transform shooter;

    private Vector3[] points;


    private float distance;

    public int target = 0;
    public Vector3 addForce;

    public bool isStraight;
    public float length;

    // Use this for initialization
    void Start () {
        nodes = new List<GameObject>();

        points = new Vector3[nodeCount];
        //pos = new List<Vector3>();

        lineRen = GetComponent<LineRenderer>();
        lineRen.positionCount = nodeCount;
        for(int i = 0; i < nodeCount; i++)
        {
            GameObject tmp = Instantiate(stdNode) as GameObject;
            tmp.transform.parent = transform;
            tmp.name = tmp.transform.parent.gameObject.name + "_node_" + i.ToString();
            nodes.Add(tmp);
            //pos.Add(tmp.transform.position);
        }
         for (int i = 0; i < nodeCount; i++)
        {
            points[i] = nodes[i].transform.position;
        }

        StartCoroutine(CheckAllow());
    }
	
	// Update is called once per frame
	void Update () {
        //CheckMoving();
        //CheckAllow();

        for (int i = 0; i < nodeCount; i++)
        {
            nodes[i].transform.position = points[i];
        }

        for (int i = 0; i < nodeCount; i++)
        {
            points[i] = nodes[i].transform.position;
        }
        points[target] += new Vector3(0.1f,0, 0.1f);

       
        lineRen.SetPositions(points);

        lineRen.startWidth = stdNode.transform.localScale.x;
        lineRen.endWidth = stdNode.transform.localScale.x;

        if (Mathf.Abs(Vector3.Magnitude(nodes[0].transform.position - nodes[nodeCount - 1].transform.position) - (nodeCount - 1) * HighAllow) < 0.01)
        {
            isStraight = true;
        }
        else
        {
            isStraight = false;
        }
        Debug.Log(isStraight);
        length = (nodeCount - 1) * HighAllow;
	}

    /*
    void CheckMoving()
    {
        for(int i = 0; i < nodeCount; i++)
        {
            if (nodes[i].transform.position != lastPos[i])
            {
                lastPos[i] = nodes[i].transform.position;
                target = i;
                return;
            }
        }
    }
    */

    
    IEnumerator CheckAllow()
    {

        while (true)
        {
            if (target == -1) yield break;

            //addForce += new Vector3(0, 0, Random.Range(-0.1f, 0.1f));

            for (int i = target - 1; i >= 0; --i)
            {
                distance = Vector3.Magnitude(points[i] - points[i+1]);
                if (distance > HighAllow * HighAllow)
                {
                    //Debug.Log(distance);
                    points[i] = Vector3.Lerp(points[i], points[i+1], (distance - HighAllow) / distance);
                    //Debug.Log(Vector3.Magnitude(nodes[i].transform.position - nodes[i + 1].transform.position));
                }
                if (distance < LowAllow * LowAllow && distance != 0)
                {
                    Vector3 tmp = points[i+1] - points[i];
                    tmp *= LowAllow / distance;
                    tmp += points[i];
                    points[i] = tmp;
                }
            }
            for (int i = target + 1; i < nodeCount; i++)
            {
                distance = Vector3.Magnitude(points[i] - points[i-1]);
                if (distance > HighAllow * HighAllow)
                {
                    //Debug.Log(distance);

                    points[i] = Vector3.Lerp(points[i], points[i-1], (distance - HighAllow) / distance);

                    //Debug.Log(Vector3.Magnitude(nodes[i].transform.position - nodes[i - 1].transform.position));

                }
                if (distance < LowAllow * LowAllow && distance != 0)
                {
                    Vector3 tmp = points[i-1] - points[i];
                    //Debug.Log(tmp);
                    tmp *= LowAllow / distance;
                    //Debug.Log(tmp);
                    tmp += points[i];
                    //Debug.Log(tmp);
                    points[i] = tmp;
                }
            }
            //nodes[target].transform.position += addForce;
            //Debug.Log(Vector3.Magnitude(nodes[0].transform.position - nodes[1].transform.position));

            //Debug.Log(Vector3.Magnitude(nodes[0].transform.position - nodes[nodeCount - 1].transform.position));

            yield return new WaitForEndOfFrame();

        }


    }
}

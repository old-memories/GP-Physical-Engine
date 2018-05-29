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
    private float distance;

    public int target = 0;
    public Vector3 addForce;

    public bool isStraight;
    public float length;

    // Use this for initialization
    void Start () {
        nodes = new List<GameObject>();
        //pos = new List<Vector3>();

        lineRen = GetComponent<LineRenderer>();
        lineRen.positionCount = nodeCount;
        for(int i = 0; i < nodeCount; i++)
        {
            GameObject tmp = Instantiate(stdNode) as GameObject;
            tmp.transform.parent = transform;
            nodes.Add(tmp);
            //pos.Add(tmp.transform.position);
        }
        addForce = new Vector3(0.05f, 0, 0);

        StartCoroutine(CheckAllow());
    }
	
	// Update is called once per frame
	void Update () {
        //CheckMoving();
        //CheckAllow();
        

        var points = new Vector3[nodeCount];
        for (int i = 0; i < nodeCount; i++)
        {
            points[i] = nodes[i].transform.position;
        }
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
                distance = Vector3.Magnitude(nodes[i].transform.position - nodes[i + 1].transform.position);
                if (distance > HighAllow * HighAllow)
                {
                    //Debug.Log(distance);
                    nodes[i].transform.position = Vector3.Lerp(nodes[i].transform.position, nodes[i + 1].transform.position, (distance - HighAllow) / distance);
                    //Debug.Log(Vector3.Magnitude(nodes[i].transform.position - nodes[i + 1].transform.position));
                }
                if (distance < LowAllow * LowAllow && distance != 0)
                {
                    Vector3 tmp = nodes[i + 1].transform.position - nodes[i].transform.position;
                    tmp *= LowAllow / distance;
                    tmp += nodes[i].transform.position;
                    nodes[i].transform.position = tmp;
                }
            }
            for (int i = target + 1; i < nodeCount; i++)
            {
                distance = Vector3.Magnitude(nodes[i].transform.position - nodes[i - 1].transform.position);
                if (distance > HighAllow * HighAllow)
                {
                    //Debug.Log(distance);

                    nodes[i].transform.position = Vector3.Lerp(nodes[i].transform.position, nodes[i - 1].transform.position, (distance - HighAllow) / distance);

                    //Debug.Log(Vector3.Magnitude(nodes[i].transform.position - nodes[i - 1].transform.position));

                }
                if (distance < LowAllow * LowAllow && distance != 0)
                {
                    Vector3 tmp = nodes[i - 1].transform.position - nodes[i].transform.position;
                    //Debug.Log(tmp);
                    tmp *= LowAllow / distance;
                    //Debug.Log(tmp);
                    tmp += nodes[i].transform.position;
                    //Debug.Log(tmp);
                    nodes[i].transform.position = tmp;
                }
            }
            //nodes[target].transform.position += addForce;
            //Debug.Log(Vector3.Magnitude(nodes[0].transform.position - nodes[1].transform.position));

            //Debug.Log(Vector3.Magnitude(nodes[0].transform.position - nodes[nodeCount - 1].transform.position));

            yield return new WaitForSeconds(0.01f);

        }


    }
}

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public string inputX;
    public string inputY;


    public string inputForward;
    public string inputBack;
    public string inputLeft;
    public string inputRight;

    public float inputSensitive;

    public int mouseButtonNumber;



    //����������  
    public float sensitivityX = 10F;
    public float sensitivityY = 10F;

    //��������ӽ�(Y�ӽ�)  
    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;
    Camera ca;

    void Update()
    {
        if (Input.GetMouseButton(mouseButtonNumber))
        {
            //��������ƶ��Ŀ���(����), ������������ת�ĽǶ�(����X)  
            float rotationX = transform.localEulerAngles.y + Input.GetAxis(inputX) * sensitivityX;

            //��������ƶ��Ŀ���(����), ������������ת�ĽǶ�(����Y)  
            rotationY += Input.GetAxis(inputY) * sensitivityY;
            //�Ƕ�����. rotationYС��min,����min. ����max,����max. ���򷵻�value   
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            //��������һ������Ƕ�  
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }

        float moveForward = (Input.GetKey(inputForward) ? 1.0f : 0) - (Input.GetKey(inputBack) ? 1.0f : 0);
        float moveRight = (Input.GetKey(inputRight) ? 1.0f : 0) - (Input.GetKey(inputLeft) ? 1.0f : 0);

        transform.Translate(new Vector3(moveRight, 0, moveForward) *inputSensitive);

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (ca.fieldOfView <= 100)
                ca.fieldOfView += 2;
            if (ca.orthographicSize <= 20)
                ca.orthographicSize += 0.5F;
        }
        //Zoom in  
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (ca.fieldOfView > 2)
                ca.fieldOfView -= 2;
            if (ca.orthographicSize >= 1)
                ca.orthographicSize -= 0.5F;
        }

    }

    void Start()
    {
        ca = GetComponent<Camera>();
    }
}
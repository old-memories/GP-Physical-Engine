using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ForceType
{
    GRAVITY,
    NORMAL,
    FRICTION,
    HIT,
}

public enum ModelType
{
    NONE,
    BALL,
    PLANE,
    BEVEL,
}

public enum BounceCombineType
{
    Average,
    Minimum,
    Multiply,
    Maximum,
}

public enum FrictionCombineType
{
    Average,
    Minimum,
    Multiply,
    Maximum,
}


public class Force
{
    public Vector3 volume;
    public string giverName;
    public ForceType type;
    public string forceName;
    public bool needCal;

    public Force()
    {

    }
    public Force(string giverName, ForceType type, string forceName)
    {
        this.giverName = giverName;
        this.type = type;
        this.forceName = forceName;
        this.volume = Vector3.zero;
        this.needCal = true;
    }
    public Force(string giverName, ForceType type, string forceName, Vector3 volume)
    {
        this.giverName = giverName;
        this.type = type;
        this.forceName = forceName;
        this.volume = volume;
        this.needCal = false;
    }

    public override string ToString()
    {
        return "forceName: " + forceName + " volume: " + volume + " type: " + type + " giverName: " + giverName; 
    }

}

public class Model
{
    public string name;
    public ModelType modelType;
    public Vector3 normal;
    public float mass;
    public Vector3 vel;
    public float bounciness;
    public BounceCombineType bounceCombineType;
    public float friction;
    public FrictionCombineType frictionCombineType;

    public Dictionary<string,Force> forces;

    public Model()
    {
        forces = new Dictionary<string, Force>();
    }

    public Model(string name,float mass, ModelType modelType)
    {
        this.name = name;
        this.modelType = modelType;
        this.forces = new Dictionary<string, Force>();
        this.normal = Vector3.zero;
        this.mass = mass;
        this.vel = Vector3.zero;
        this.bounciness = 0;
        this.bounceCombineType = BounceCombineType.Average;
        this.friction = 0;
        this.frictionCombineType = FrictionCombineType.Average;
    }

    public Model(string name,float mass, Vector3 normal, ModelType modelType)
    {
        this.name = name;
        this.modelType = modelType;
        this.forces = new Dictionary<string, Force>();
        this.normal = normal;
        this.mass = mass;
        this.vel = Vector3.zero;
        this.bounciness = 0;
        this.bounceCombineType = BounceCombineType.Average;
        this.friction = 0;
        this.frictionCombineType = FrictionCombineType.Average;
    }








    public void AddForce(string giverName, ForceType type,string name)
    {
        Force f = new Force(giverName,type, name);
        Debug.Log(f.ToString());
        try
        {
            forces.Add(name, f);

        }
        catch (Exception)
        {
            Debug.Log("AddForce: Adding force: " + f.ToString() + " failed.");
        }
    }

    public void AddForce(string giverName, ForceType type, Vector3 volume, string name)
    {
        Force f = new Force(giverName, type, name,volume);
        Debug.Log(f.ToString());
        try
        {
            forces.Add(name, f);

        }
        catch (Exception)
        {
            Debug.Log("AddForce: Adding force: " + f.ToString() + " failed.");
        }
    }


    public bool RemoveForce(string forceName)
    {
        return forces.Remove(forceName);
    }

    public void ChangeForce(string forceName, Vector3 volume)
    {
        if(forces[forceName].needCal==false)
            forces[forceName].volume = volume;
    }
    public void  FindForce(string forceName, ref Force f)
    {
         f = forces[forceName];
    }

    public new string ToString()
    {
        return "name: " + name + " mass: " + mass + " bounciness :" + bounciness +"bounce combine type: "+bounceCombineType+ " normal: " + normal;
    }
    public void SetAttribute(string name)
    {
        this.name = name;
    }

    public void SetAttribute(string name, float mass)
    {
        this.name = name;
        this.mass = mass;
    }


    public void SetAttribute(string name, Vector3 normal)
    {
        this.name = name;
        this.normal = normal;
    }
    public void SetAttribute(string name, float mass, Vector3 vel)
    {
        this.name = name;
        this.mass = mass;
        this.vel = vel;

    }

    public void SetAttribute(string name, float bounciness, BounceCombineType bounceCombineType)
    {
        this.name = name;
        this.bounciness = bounciness;
        this.bounceCombineType = bounceCombineType;
    }

    public void SetAttribute(string name, float friction, FrictionCombineType frictionCombineType)
    {
        this.name = name;
        this.friction = friction;
        this.frictionCombineType = frictionCombineType;
    }


    public void  GetAttribute(out string name)
    {
        name = this.name;
    }


    public void GetAttribute(out string name, out float mass)
    {
        name = this.name;
        mass = this.mass;
    }

    public void GetAttribute(out string name, out Vector3 normal)
    {
        name = this.name;
        normal = this.normal;
    }

    public void GetAttribute(out string name, out float mass, out Vector3 vel)
    {
        name = this.name;
        mass = this.mass;
        vel = this.vel;
    }

    public void GetAttribute(out string name, out  float bounciness, out BounceCombineType bounceCombineType)
    {
        name = this.name;
        bounciness = this.bounciness;
        bounceCombineType = this.bounceCombineType;
    }

    public void GetAttribute(out string name, out float friction, out FrictionCombineType frictionCombineType)
    {
        name = this.name;
        friction = this.friction;
        frictionCombineType = this.frictionCombineType;
    }


}

public class PhysicsManager : MonoBehaviour
{

    public Vector3 g;
    public Dictionary<string,Model> models;

    [Range(0.001f, 0.01f)]
    public float timeStep = 0.005f;

    public float ignoreVectorValue;

    public bool started;

    public Color selectedColor;
    public Color oldColor;

    public GameObject selectedGameObject;

    public GameObject ball;


    public string inputForward;
    public string inputBack;
    public string inputLeft;
    public string inputRight;
    public string inputUp;
    public string inputDown;

    public float inputSensitive = 0.1f;


    public Camera ca;
    private Ray ra;
    private RaycastHit hit;


    private int bevelCount;
    private int ballCount;
    private int planeCount;

    // Use this for initialization

    private void Awake()
    {
        started = false;
        bevelCount = 0;
        ballCount = 0;
        planeCount = 0;
        selectedGameObject = gameObject;
        models = new Dictionary<string, Model>();
        Debug.Log("awake physicasManager");
    }
    void Start () {
        StartCoroutine(MouseMove());
        StartCoroutine(SelectedGameObjectControl());

    }

    // Update is called once per frame
    void Update () {
       
    }



    IEnumerator MouseMove()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ra = ca.ScreenPointToRay(Input.mousePosition);
                //如果点击了一个物体
                if (Physics.Raycast(ra, out hit))
                {
                    //如果原来已选中，恢复selectedGameObject颜色
                    if (selectedGameObject.name != name)
                    {
                        //选中的是斜面
                        if (selectedGameObject.tag == "bevel")
                        {
                            foreach (var child in selectedGameObject.GetComponentsInChildren<Transform>())
                            {
                                //对斜面所有子物体遍历
                                if (child.gameObject.tag == "bevel" && child.gameObject.name != selectedGameObject.name)
                                {
                                    child.gameObject.GetComponent<MeshRenderer>().material.color = oldColor;
                                }
                            }
                        }
                        //选中的不是斜面
                        else
                        {
                            selectedGameObject.GetComponent<MeshRenderer>().material.color = oldColor;
                        }
                    }
                    //如果原来未选中



                    //记录原颜色, 更新选中物体颜色, 把selectedGameObject覆盖掉

                    //如果选中的是斜面（的子物体）
                    if (hit.collider.gameObject.tag == "bevel")
                    {
                        //对斜面所有子物体遍历
                        foreach (var child in hit.collider.transform.parent.gameObject.GetComponentsInChildren<Transform>())
                        {
                            if (child.gameObject.tag == "bevel" && child.gameObject.name != hit.collider.transform.parent.gameObject.name)
                            {
                                oldColor = child.gameObject.GetComponent<MeshRenderer>().material.color;
                                child.gameObject.GetComponent<MeshRenderer>().material.color = selectedColor;
                            }
                        }

                        selectedGameObject = hit.collider.transform.parent.gameObject;

                    }
                    //选中的不是斜面
                    else
                    {
                        oldColor = hit.collider.gameObject.GetComponent<MeshRenderer>().material.color;
                        hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = selectedColor;
                        selectedGameObject = hit.collider.gameObject;

                    }
                   

                }
                //如果没有点击到物体
                else
                {
                    //如果已经选中了物体，恢复selectedGameObject颜色
                    if (selectedGameObject.name != gameObject.name)
                    {
                        if (selectedGameObject.tag == "bevel")
                        {
                            foreach (var child in selectedGameObject.GetComponentsInChildren<Transform>())
                            {
                                if (child.gameObject.tag == "bevel" && child.gameObject.name != selectedGameObject.name)
                                {
                                    child.gameObject.GetComponent<MeshRenderer>().material.color = oldColor;
                                }
                            }
                        }
                        else
                        {
                            selectedGameObject.GetComponent<MeshRenderer>().material.color = oldColor;

                        }
                        //恢复未选中状态
                        selectedGameObject = gameObject;

                    }
                    //如果未选中物体
                }

            }
            yield return null;

        }
    }

    IEnumerator SelectedGameObjectControl()
    {
        while (true)
        {
            float moveForward = (Input.GetKey(inputForward) ? 1.0f : 0) - (Input.GetKey(inputBack) ? 1.0f : 0);
            float moveRight = (Input.GetKey(inputRight) ? 1.0f : 0) - (Input.GetKey(inputLeft) ? 1.0f : 0);
            float moveUp = (Input.GetKey(inputUp) ? 1.0f : 0) - (Input.GetKey(inputDown) ? 1.0f : 0);
            //如果是未选中
            if (selectedGameObject==gameObject)
            {
               


            }
            //选中了物体
            else
            {
                if (!started)
                {
                    //float y = ca.transform.rotation.eulerAngles.y;
                    //Vector3 target = Quaternion.Euler(0, y, 0) * new Vector3(-moveForward, moveUp, moveRight);
                    Vector3 target = new Vector3(moveForward, moveUp, moveRight);
                    selectedGameObject.transform.Translate( target * inputSensitive);
                }
            }
            yield return null;
           
        }
    }

    public void OnStartButtonClicked()
    {
        started = !started;
    }

    public void OnNewBallButtonClicked()
    {
        Instantiate(ball,transform);
    }

    public Vector3 CalGravity(float mass)
    {
        return mass * g;
    }

    public Vector3 CalAcc(Vector3 sumForce, float mass)
    {
        return sumForce / mass;
    }

    public string AddForce(string name, string giverName, ForceType type, Vector3 volume,string forceName)
    {
        Model model = new Model();
        GetObject(name, out model);
        model.AddForce(giverName, type, volume, forceName);
        return forceName;
    }

    public string AddForce(string name, string giverName, ForceType type, Vector3 volume)
    {
        Model model = new Model();
        GetObject(name, out model);
        string _name = type + "_f_" + giverName + "_to_" + name;
        model.AddForce(giverName, type, volume, _name);
        Debug.Log(model.ToString());
        return _name;
    }

    public string AddForce(string name, string giverName, ForceType type)
    {
        Model model = new Model();
        GetObject(name, out model);
        string _name = type + "_f_" + giverName + "_to_" + name;
        model.AddForce(giverName,type,_name);
        Debug.Log(model.ToString());
        return _name;
    }


    public void RemoveForce(string name,string forceName)
    {
        Model model = new Model();
        GetObject(name, out model);
        Debug.Log("Remove: " + forceName + " " + model.RemoveForce(forceName));
    }
    public void RemoveForce(string name, string giverName, ForceType type)
    {
        Model model = new Model();
        GetObject(name, out model);
        string forceName = type + "_f_" + giverName + "_to_" + name;
        Debug.Log("Remove: "+forceName+" "+model.RemoveForce(forceName));
    }

    public void ChangeForce(string name, string forceName,Vector3 volume)
    {
        Model model = new Model();
        GetObject(name, out model);
        model.ChangeForce(forceName, volume);
    }
    public void ChangeForce(string name, string giverName, ForceType type,Vector3 volume)
    {
        Model model = new Model();
        GetObject(name, out model);
        string forceName = type + "_f_" + giverName + "_to_" + name;
        model.ChangeForce(forceName, volume);
    }

    public void FindForce(string name, string giverName, ForceType type,ref Force f)
    {
        Model model = new Model();
        GetObject(name, out model);
        string forceName = type + "_f_" + giverName + "_to_" + name;
       model.FindForce(forceName,ref f);
    }

    public string AddObject(string tag,float mass, Vector3 normal)
    {
        if(tag == "bevel")
        {
            string _name = "bevel_" + bevelCount.ToString();
            Model bevel = new Model(_name, mass, normal, ModelType.BEVEL);
            Debug.Log(bevel.ToString());
            Debug.Log("add "+_name);
            bevelCount++;
            Debug.Log(models);
            models.Add(_name,bevel);
            return _name;
        }
        if (tag == "ball")
        {
            string _name = "ball_" + ballCount.ToString();
            Model ball = new Model(_name, mass, ModelType.BALL);
            Debug.Log(ball.ToString());
            Debug.Log("add " + _name);
            ballCount++;
            Debug.Log(models);
            models.Add(_name, ball);
            return _name;
        }

        if (tag == "plane")
        {
            string _name = "plane_" + planeCount.ToString();
            Model plane = new Model(_name, mass,normal, ModelType.PLANE);
            Debug.Log(plane.ToString());
            Debug.Log("add " + _name);
            planeCount++;
            Debug.Log(models);
            models.Add(_name, plane);

            return _name;
        }
        return null;
        
    }

    public void GetObject(string name, out Model model)
    {
        models.TryGetValue(name, out model);
    }

    public void GetObject(string name, out float mass)
    {
        Model model = new Model();
        models.TryGetValue(name, out model);
        model.GetAttribute(out name, out mass);
    }

    public void GetObject(string name, out Vector3 normal)
    {
        Model model = new Model();
        models.TryGetValue(name, out model);
        model.GetAttribute(out name, out normal);
    }

    public void GetObject(string name , out float mass, out Vector3 vel)
    {
        Model model = new Model();
        models.TryGetValue(name, out model);
        model.GetAttribute(out name, out mass,out  vel);
    }

    public void GetObject(string name, out float bounciness, out BounceCombineType bounceCombineType)
    {
        Model model = new Model();
        models.TryGetValue(name, out model);
        model.GetAttribute(out name, out bounciness, out bounceCombineType);
    }

    public void SetObject(string name, Vector3 normal)
    {
        Model model = new Model();
        GetObject(name, out model);
        //Debug.Log(model.ToString());
        model.SetAttribute(name, normal);
        models[name] = model;
    }

    public void SetObject(string name, float mass, Vector3 vel)
    {
        Model model = new Model();
        GetObject(name, out model);
        //Debug.Log(model.ToString());
        model.SetAttribute(name, mass, vel);
        models[name] = model;
    }

    public void SetObject(string name, float bounciness, BounceCombineType bounceCombineType)
    {
        Model model = new Model();
        GetObject(name, out model);
        //Debug.Log(model.ToString());
        model.SetAttribute(name, bounciness, bounceCombineType);
        models[name] = model;
    }

    public void SetObject(string name, float friction, FrictionCombineType frictionCombineType)
    {
        Model model = new Model();
        GetObject(name, out model);
        //Debug.Log(model.ToString());
        model.SetAttribute(name, friction, frictionCombineType);
        models[name] = model;
    }


    //计算支持力的方法
    public Vector3 CalNormal(string name, float mass,Vector3 vel, string giverName, Vector3 sumForce)
    {
        Vector3 normalForce = Vector3.zero;
        Model model = new Model();
        GetObject(giverName, out model);
        string temp = "";
        Vector3 normalDir = Vector3.zero;
        if (model.modelType == ModelType.BEVEL)
        {
            model.GetAttribute(out temp, out normalDir);

        }
        else if (model.modelType == ModelType.PLANE)
        {
            model.GetAttribute(out temp, out normalDir);

        }
        else
        {
            Debug.Log("CalNormal: No Valid Model Type");
            return normalForce;
        }
        float sumForceNormal = Vector3.Dot(sumForce, normalDir);
        if(Mathf.Abs(Vector3.Dot(vel, normalDir)) <= ignoreVectorValue)
        {
            normalForce = -sumForceNormal * normalDir;

        }
        else
        {
            Debug.Log("CalNormal: GiverName: "+giverName+" Vel: "+vel+" More Than Ignore Hit Value :"+ignoreVectorValue+" in NormalDir: "+normalDir);
            return normalForce;
        }
        return normalForce;
    }

    //计算摩擦力的方法
    public Vector3 CalFriction(string name, float mass, ref Vector3 vel, string giverName, Vector3 NormalForce, Vector3 sumForce)
    {
        Vector3 frictionForce = Vector3.zero;
        Model self = new Model();
        GetObject(name, out self);
        Model giver = new Model();
        GetObject(giverName, out giver);

        //根据两个物体的摩擦系数组合类型的优先级决定最后的摩擦系数组合类型
        FrictionCombineType frictionCombineType = self.frictionCombineType.CompareTo(giver.frictionCombineType) > 0 ? self.frictionCombineType : giver.frictionCombineType;
        float friction = 0.0f;
        if (frictionCombineType == FrictionCombineType.Average)
        {
            friction = (self.friction + giver.friction) / 2.0f;
        }

        if (frictionCombineType == FrictionCombineType.Minimum)
        {
            friction = Mathf.Min(self.friction, giver.friction);

        }

        if (frictionCombineType == FrictionCombineType.Multiply)
        {
            friction = self.friction * giver.friction;
        }

        if (frictionCombineType == FrictionCombineType.Maximum)
        {
            friction = Mathf.Max(self.friction, giver.friction);

        }

        Vector3 giverVel = giver.vel;
        Debug.Log(Vector3.Magnitude(friction * NormalForce));
        Debug.Log(Vector3.Magnitude(sumForce + NormalForce));
        if (AlignVector3((giverVel-vel), new Vector3(ignoreVectorValue, ignoreVectorValue, ignoreVectorValue)) == Vector3.zero 
            && Vector3.Magnitude(friction*NormalForce)>= Vector3.Magnitude(sumForce+NormalForce))
        {
            frictionForce = -(sumForce+NormalForce);
            vel = giverVel;
            Debug.Log(name+" static firction: " + frictionForce);
        }
        else
        {
            frictionForce = -Vector3.Magnitude(friction * NormalForce)*Vector3.Normalize(vel-giverVel);
            Debug.Log(name + " dynamic firction: " + frictionForce);

        }
        return frictionForce;
    }

    //计算合力的方法
    public Vector3 CalForce(string name)
    {
        Model model = new Model();
        GetObject(name, out model);
        Vector3 sum = Vector3.zero;
        Vector3 sumNormal = Vector3.zero;
        Vector3 sumFriction = Vector3.zero;

        //先计算所有已知大小的力的合力，并忽略冲撞力（ForceType.HIT）
        foreach (Force f in model.forces.Values)
        {
            //needCal为false表明已知大小
            if (f.needCal == false && f.type!=ForceType.HIT)
            {
                sum += f.volume;
            }

        }
        //再计算所有未知大小的支持力，进而计算合力，并忽略冲撞力（ForceType.HIT）
        foreach (Force f in model.forces.Values)
        {
            if (f.needCal == true && f.type != ForceType.HIT)
            {
                if (f.type == ForceType.NORMAL)
                {
                    //Debug.Log("CalNormal: " + f.ToString());
                    string temp = "";
                    float mass = 0;
                    Vector3 vel = Vector3.zero;
                    model.GetAttribute(out temp, out mass, out vel);
                    f.volume = CalNormal(name, mass, vel, f.giverName, sum);
                    sumNormal += f.volume;
                   
                }
            }
            
        }
        //再计算所有未知大小的摩擦力，进而计算合力，并忽略冲撞力（ForceType.HIT）
        foreach (Force f in model.forces.Values)
        {
            if (f.needCal == true && f.type != ForceType.HIT)
            {
                if (f.type == ForceType.FRICTION)
                {
                    //Debug.Log("CalFriction: " + f.ToString());
                    string temp = "";
                    float mass = 0;
                    Vector3 vel = Vector3.zero;
                    Force normalForce = new Force();
                    model.GetAttribute(out temp, out mass, out vel);
                    f.volume = CalFriction(name, mass, ref vel, f.giverName, sumNormal,sum);
                    sumFriction += f.volume;
                    model.SetAttribute(temp, mass, vel);
                }
            }

        }
        return sum+sumNormal+ sumFriction;
    }

    public void CalHit(string name, string otherName,Vector3 p1, Vector3 p2, out Vector3 v1,out Vector3 v2)
    {

        Model self = new Model();
        GetObject(name, out self);
        Model other = new Model();
        GetObject(otherName, out other);
        //根据两个物体的恢复系数组合类型的优先级决定最后的恢复系数组合类型
        BounceCombineType bounceCombineType = self.bounceCombineType.CompareTo(other.bounceCombineType) > 0 ? self.bounceCombineType : other.bounceCombineType;
        float bounciness = 0.0f;
        if (bounceCombineType == BounceCombineType.Average)
        {
            bounciness = (self.bounciness + other.bounciness) / 2.0f;
        }

        if (bounceCombineType == BounceCombineType.Minimum)
        {
            bounciness = Mathf.Min(self.bounciness , other.bounciness);

        }

        if (bounceCombineType == BounceCombineType.Multiply)
        {
            bounciness = self.bounciness * other.bounciness;
        }

        if (bounceCombineType == BounceCombineType.Maximum)
        {
            bounciness = Mathf.Max(self.bounciness , other.bounciness);

        }
       

        float m1 = self.mass;
        float m2 = other.mass;
        Vector3 v10 = self.vel;
        Vector3 v20 = other.vel;


        Debug.Log("self: " + self.ToString());
        Debug.Log("other: " + other.ToString());
        Debug.Log("self v10: " + v10);
        Debug.Log("other v20: " + v20);

        Vector3 normal1 = self.normal;
        Vector3 normal2 = other.normal;
        Debug.Log("p1: " + p1 + " p2: " + p2);
        Vector3 dir = p1 - p2;
        Debug.Log("dir: " + dir.x +" "+ dir.y +" "+ dir.z);
        dir = AlignVector3(dir, new Vector3(ignoreVectorValue, ignoreVectorValue, ignoreVectorValue));
        dir = Vector3.Normalize(dir);


        Debug.Log("CalHit: dir:" + dir);

        //小球模型没有表面法向量（设为0），所以这里是两个小球碰撞的情况
        if (normal1 == Vector3.zero && normal2 == Vector3.zero)
        {
            //一动一静
            if (v10 == Vector3.zero)
            {

                Vector3 v20_normal = Vector3.Dot(v20, dir) * dir;
                //Debug.Log("v20_normal: " + v20_normal);
                Vector3 v20_else = v20 - v20_normal;
                v20 = v20 - v20_else;
                //Debug.Log("v20: " + v20 + " v20_else: " + v20_else);
                if (v10 == v20)
                {
                    v1 = v10;
                    v2 = v20+ v20_else;
                    SetObject(name, m1, v1);
                    SetObject(otherName, m2, v2);
                    return;

                }
                if (m2 < 0)
                {
                    Debug.Log("CalHit: m2>>m1");
                    v1 = v20 + v20 * bounciness - v10 * bounciness;
                    v2 = v20;
                }
                else
                {
                    Debug.Log("CalHit: m2~m1");
                    v1 = (m2 * v20 * (1.0f + bounciness) + m1 * v10 - m2 * v10 * bounciness) / (m1 + m2);
                    v2 = (m1 * v10 * (1.0f + bounciness) + m2 * v20 - m1 * v20 * bounciness) / (m1 + m2);
                }
                
                v2 = v2 + v20_else;
              


            }
            //一动一静
            else if (v20 == Vector3.zero)
            {
                Vector3 v10_normal = Vector3.Dot(v10, dir) * dir;
                Vector3 v10_else = v10 - v10_normal;
                v10 = v10 - v10_else;
                if (v10 == v20)
                {
                    v1 = v10+v10_else;
                    v2 = v20;
                    SetObject(name, m1, v1);
                    SetObject(otherName, m2, v2);
                    return;

                }
                if (m2 < 0)
                {
                    Debug.Log("CalHit: m2>>m1");
                   
                        v1 = v20 + v20 * bounciness - v10 * bounciness;
                        v2 = v20;
                    

                }
                else
                {
                    Debug.Log("CalHit: m2~m1");

                    v1 = (m2 * v20 * (1.0f + bounciness) + m1 * v10 - m2 * v10 * bounciness) / (m1 + m2);
                        v2 = (m1 * v10 * (1.0f + bounciness) + m2 * v20 - m1 * v20 * bounciness) / (m1 + m2);
                    


                }
                v1 = v1 + v10_else;
            }
            //两动，只支持一种碰撞情况
            else
            {
                v10 = AlignVector3(v10, new Vector3(ignoreVectorValue, ignoreVectorValue, ignoreVectorValue));
                v20 = AlignVector3(v20, new Vector3(ignoreVectorValue, ignoreVectorValue, ignoreVectorValue));
                float angle1 = Mathf.Acos(Mathf.Abs(Vector3.Dot(Vector3.Normalize(v10), dir)));
                float angle2 = Mathf.Acos(Mathf.Abs(Vector3.Dot(Vector3.Normalize(v20), dir)));

                //两个小球都有速度且至少一个速度不在连心线方向上，无法计算
                if (angle1 > ignoreVectorValue|| angle2 > ignoreVectorValue)
                {
                    Debug.Log("CalHit: two velocities not supported");
                    v1 = self.vel;
                    v2 = other.vel;
                }
                //两个小球都有速度且都在连心线上,即一维的碰撞
                else
                {

                    if (v10 == v20)
                    {
                        v1 = v10;
                        v2 = v20;
                        SetObject(name, m1, v1);
                        SetObject(otherName, m2, v2);
                        return;

                    }
                    if (m2 < 0)
                    {
                        Debug.Log("CalHit: m2>>m1");

                        v1 = v20 + v20 * bounciness - v10 * bounciness;
                        v2 = v20;


                    }
                    else
                    {
                        Debug.Log("CalHit: m2~m1");

                        v1 = (m2 * v20 * (1.0f + bounciness) + m1 * v10 - m2 * v10 * bounciness) / (m1 + m2);
                        v2 = (m1 * v10 * (1.0f + bounciness) + m2 * v20 - m1 * v20 * bounciness) / (m1 + m2);



                    }
                  

                }

            }
        }
        //小球碰表面
        else if (normal1 == Vector3.zero)
        {
            if(v20 != Vector3.zero)
            {
                Debug.Log("CalHit: one normal with velocity not supported");
                v1 = self.vel;
                v2 = other.vel;
            }
            Vector3 normalDir = AlignVector3(normal2, new Vector3(ignoreVectorValue, ignoreVectorValue, ignoreVectorValue));
            ;
            Vector3 v10_normal = Vector3.Dot(v10, normalDir) * normalDir;
            Vector3 v10_else = v10 - v10_normal;
            v10 = v10 - v10_else;
            if (v10 == v20)
            {
                v1 = v10+ v10_else;
                v2 = v20;
                SetObject(name, m1, v1);
                SetObject(otherName, m2, v2);
                return;

            }
            if (m2 < 0)
            {
                Debug.Log("CalHit: m2>>m1");
               
                    v1 = v20 + v20 * bounciness - v10 * bounciness;
                    v2 = v20;
                

            }
            else
            {
                Debug.Log("CalHit: m2~m1");

                v1 = (m2 * v20 * (1.0f + bounciness) + m1 * v10 - m2 * v10 * bounciness) / (m1 + m2);
                    v2 = (m1 * v10 * (1.0f + bounciness) + m2 * v20 - m1 * v20 * bounciness) / (m1 + m2);
                


            }
            v1 = v1 + v10_else;

        }
        //小球碰表面

        else if (normal2 == Vector3.zero)
        {
            if (v10 != Vector3.zero)
            {
                Debug.Log("CalHit: one normal with velocity not supported");
                v1 = self.vel;
                v2 = other.vel;
            }
            Vector3 normalDir = AlignVector3(normal1, new Vector3(ignoreVectorValue, ignoreVectorValue, ignoreVectorValue));
            Vector3 v20_normal = Vector3.Dot(v20, normalDir) * normalDir;
            Vector3 v20_else = v20 - v20_normal;
            v20 = v20 - v20_else;
            if (v10 == v20)
            {
                v1 = v10;
                v2 = v20+v20_else;
                SetObject(name, m1, v1);
                SetObject(otherName, m2, v2);
                return;

            }
            if (m2 < 0)
            {
                Debug.Log("CalHit: m2>>m1");
                v1 = v20 + v20 * bounciness - v10 * bounciness;
                v2 = v20;
            }
            else
            {
                Debug.Log("CalHit: m2~m1");

                v1 = (m2 * v20 * (1.0f + bounciness) + m1 * v10 - m2 * v10 * bounciness) / (m1 + m2);
                v2 = (m1 * v10 * (1.0f + bounciness) + m2 * v20 - m1 * v20 * bounciness) / (m1 + m2);
            }
            v2 = v2 + v20_else;
        }
        else
        {
            Debug.Log("CalHit: two normal not supported");
            v1 = self.vel;
            v2 = other.vel;
        }
        Debug.Log("self v1: " + v1);
        Debug.Log("other v2: " + v2);
        SetObject(name, m1, v1);
        SetObject(otherName, m2, v2);


    }

    //忽略小的误差
    public Vector3 AlignVector3(Vector3 v, Vector3 align)
    {
        if (Mathf.Abs(v.x) < align.x)
        {
            Debug.Log("v.x : " + v.x);
            v.x = 0;
        }
        if (Mathf.Abs(v.y) < align.y)
        {
            Debug.Log("v.y : " + v.y);

            v.y = 0;
        }
        if (Mathf.Abs(v.z) < align.z)
        {
            Debug.Log("v.z : " + v.z);
            v.z = 0;
        }
        return v;
    }

    
}

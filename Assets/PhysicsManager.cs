using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ForceType
{
    GRAVITY,
    NORMAL,
    FRICTION,
    HIT
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
    }








    public void AddForce(string giverName, ForceType type,string name)
    {
        Force f = new Force(giverName,type, name);
        Debug.Log(f.ToString());
        forces.Add(name, f);
    }

    public void AddForce(string giverName, ForceType type, Vector3 volume, string name)
    {
        Force f = new Force(giverName, type, name,volume);
        Debug.Log(f.ToString());
        forces.Add(name, f);
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


}

public class PhysicsManager : MonoBehaviour {

    public Vector3 g;
    public Dictionary<string,Model> models;

    [Range(0.001f, 0.01f)]
    public float timeStep = 0.005f;

    public float ignoreVectorValue;

    public bool started;


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
        models = new Dictionary<string, Model>();
        Debug.Log("awake physicasManager");
    }
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnStartButtonClicked()
    {
        started = !started;
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
            Debug.Log("CalNormal: No Valid Normal Vector");
            return normalForce;
        }
        float sumForceNormal = Vector3.Dot(sumForce, normalDir);
        if(Mathf.Abs(Vector3.Dot(vel, normalDir)) <= ignoreVectorValue)
        {
            normalForce = -sumForceNormal * normalDir;

        }
        else
        {
            Debug.Log("CalNormal: Vel: "+vel+" More Than Ignore Hit Value. GiverName: "+giverName+" NormalDir: "+normalDir);
            return normalForce;
        }
        return normalForce;
    }


    public Vector3 CalForce(string name)
    {
        Model model = new Model();
        GetObject(name, out model);
        Vector3 sum = Vector3.zero;
        foreach (string key in model.forces.Keys)
        {
            Force f = model.forces[key];
            if (f.needCal == false)
            {
                sum += f.volume;
            }

        }

        foreach (string key in model.forces.Keys)
        {
            Force f = model.forces[key];

            if (f.type == ForceType.NORMAL)
            {
                if (model.modelType == ModelType.BALL)
                {
                    string temp = "";
                    float mass = 0;
                    Vector3 vel = Vector3.zero;
                    model.GetAttribute(out temp, out mass, out vel);
                    f.volume = CalNormal(name, mass,vel, f.giverName,sum);
                    sum += f.volume;
                }
            }
            if (f.type == ForceType.FRICTION)
            {
                Debug.Log("FRICTION NOT IMPLEMENTED");
            }
        }
        return sum;
    }

    public void CalHit(string name, string otherName,Vector3 p1, Vector3 p2, out Vector3 v1,out Vector3 v2)
    {

        Model self = new Model();
        GetObject(name, out self);
        Model other = new Model();
        GetObject(otherName, out other);
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

        if (normal1 == Vector3.zero && normal2 == Vector3.zero)
        {
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
            else
            {
                Vector3 velDir = v20 - v10;
                velDir = AlignVector3(velDir, new Vector3(ignoreVectorValue, ignoreVectorValue, ignoreVectorValue));
                velDir = Vector3.Normalize(velDir);
                Debug.Log("velDir: " + velDir);
                Debug.Log("dir: " + dir);

                if (Mathf.Abs(Vector3.Angle(velDir, dir)) > ignoreVectorValue)
                {
                    Debug.Log("CalHit: two velocities not supported");
                    v1 = self.vel;
                    v2 = other.vel;
                }
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

        else if(normal2 == Vector3.zero)
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

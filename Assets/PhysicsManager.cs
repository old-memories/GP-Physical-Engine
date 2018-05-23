using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ForceType
{
    GRAVITY,
    NORMAL,
    FRICTION,
}

public enum ModelType
{
    BEVEL,
    BALL,

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
    public Dictionary<string,Force> forces;

    public Model()
    {
        forces = new Dictionary<string, Force>();
    }

    public Model(string name,ModelType modelType)
    {
        this.name = name;
        this.modelType = modelType;
        forces = new Dictionary<string, Force>();

    }







    public virtual void AddForce(string giverName, ForceType type,string name)
    {
        Force f = new Force(giverName,type, name);
        Debug.Log(f.ToString());
        forces.Add(name, f);
    }

    public virtual void AddForce(string giverName, ForceType type, Vector3 volume, string name)
    {
        Force f = new Force(giverName, type, name,volume);
        Debug.Log(f.ToString());
        forces.Add(name, f);
    }


    public virtual bool RemoveForce(string forceName)
    {
        return forces.Remove(forceName);
    }

    public virtual void ChangeForce(string forceName, Vector3 volume)
    {
        if(forces[forceName].needCal==false)
            forces[forceName].volume = volume;
    }

    public new virtual string ToString()
    {
        return "name: "+name;
    }
    public virtual void SetAttribute(string name)
    {
        this.name = name;
    }
    public virtual void SetAttribute(string name, Vector3 normal, float height, float length)
    {
        this.name = name;
    }
    public virtual void SetAttribute(string name, float mass, Vector3 vel)
    {
        this.name = name;

    }

    public virtual void  GetAttribute(out string name)
    {
        name = this.name;
    }

    public virtual void GetAttribute(out string name, out Vector3 normal, out float height,out float length)
    {
        name = this.name;
        normal = Vector3.zero;
        height = 0.0f;
        length = 0.0f;
    }
    public virtual void GetAttribute(out string name, out float mass, out Vector3 vel)
    {
        name = this.name;
        mass = 0.0f;
        vel = Vector3.zero;
    }


}

public class Bevel : Model
{
    public Vector3 normal;
    public float height;
    public float length;

    public Bevel(Vector3 normal,float height,float length,string name):base(name,ModelType.BEVEL)
    {
        this.normal = normal;
        this.height = height;
        this.length = length;
    }
    public override string ToString()
    {
        return base.ToString()+ " height: " + height + " length: " + length + " normal: " + normal;
    }
    public override void SetAttribute(string name,Vector3 normal, float height, float length)
    {
        base.SetAttribute(name);
        this.normal = normal;
        this.height = height;
        this.length = length;
    }
    public override void GetAttribute(out string name, out Vector3 normal, out float height, out float length)
    {
        name = this.name;
        normal = this.normal;
        height = this.height;
        length = this.length;
    }
}

public class Ball : Model
{
    public float mass;
    public Vector3 vel;

    public Ball(float mass,Vector3 vel, string name) : base(name,ModelType.BALL)
    {
        this.mass = mass;
        this.vel = vel;
    }
    public override string ToString()
    {
        return base.ToString() + " mass: " + mass + " vel: " + vel;
    }
    public override void SetAttribute(string name, float mass, Vector3 vel)
    {
        base.SetAttribute(name);
        this.mass = mass;
        this.vel = vel;
    }

    public override void GetAttribute(out string name, out float mass, out Vector3 vel)
    {
        name = this.name;
        mass = this.mass;
        vel = this.vel;
    }
}

public class PhysicsManager : MonoBehaviour {

    public Vector3 g;
    public Dictionary<string,Model> models;
    private int bevelCount;
    private int ballCount;

    // Use this for initialization

    private void Awake()
    {
        bevelCount = 0;
        ballCount = 0;
        models = new Dictionary<string, Model>();

    }
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
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

    public string AddObject(string tag,Vector3 normal,float height,float length)
    {
        if(tag == "bevel")
        {
            string _name = "bevel_" + bevelCount.ToString();
            Bevel bevel= new Bevel(normal,height,length,_name);
            Debug.Log(bevel.ToString());
            Debug.Log("add "+_name);
            bevelCount++;
            models.Add(_name,bevel);
            return _name;
        }
        return null;
        
    }

    public string AddObject(string tag, float mass, Vector3 vel)
    {
        if (tag == "ball")
        {
            string _name = "ball_" + ballCount.ToString();
            Ball ball = new Ball(mass, vel, _name);
            Debug.Log(ball.ToString());
            Debug.Log("add " + _name);
            ballCount++;
            models.Add(_name, ball);
            return _name;
        }
        return null;
    }

    public void GetObject(string name, out Model model)
    {
        models.TryGetValue(name, out model);
    }

    public void SetObject(string name, Vector3 normal, float height, float length)
    {
        Model model = new Model();
        GetObject(name, out model);
        //Debug.Log(model.ToString());
        model.SetAttribute(name, normal,height,length);
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

    public Vector3 CalNormal(float mass,Vector3 vel, string giverName, Vector3 sumForce)
    {
        Vector3 normal = Vector3.zero;
        Model model = new Model();
        GetObject(giverName, out model);
        if (model.modelType == ModelType.BEVEL)
        {
            string temp = "";
            Vector3 normalDir = Vector3.zero;
            float height = 0.0f;
            float length = 0.0f;
            model.GetAttribute(out temp, out normalDir, out height, out length);
            float sumForceNormal = Vector3.Dot(sumForce, normalDir);
            if (Mathf.Abs(Vector3.Dot(vel, normalDir)) <= 0.01f)
            {
                normal = -sumForceNormal * normalDir;

            }
        }

        return normal;
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
                    f.volume = CalNormal(mass,vel, f.giverName,sum);
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
}

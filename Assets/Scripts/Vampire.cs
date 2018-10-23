using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Vehicle
{

    //attributes
    public Material targetMat;
    public GameObject target;

    // Use this for initialization
    protected override void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void CalcSteeringForces()
    {
        //find the closest human and hunt them down, paying no mind to building edges
        Vector3 ultimateForce = new Vector3();
        if (target)
        {
            ultimateForce += Seek(target.transform.position) * seekWeight;
        }
        //no humans left, wander
        else
        {
            ultimateForce += Wander() * wanderWeight;
        }
        //avoid obstacles
        ultimateForce += AvoidObstacles() * avoidObstacleWeight;
        ultimateForce = ultimateForce.normalized * maxForce;
        ApplyForce(ultimateForce);
    }

    protected override void OnRenderObject()
    {
        base.OnRenderObject();
        if (Manager.Instance.debugLines)
        {
            if (target)
            {
                targetMat.SetPass(0);
                GL.Begin(GL.LINES);
                GL.Vertex(transform.position);
                GL.Vertex(target.transform.position);
                GL.End();
            }
        }
    }

    //collision detection with wooden stakes
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Stake_Wood(Clone)")
        {
            GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().woodenStakes.Remove(collision.gameObject);
            Destroy(collision.gameObject);
            GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().score++;
            GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().vampires.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}

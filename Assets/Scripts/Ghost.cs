using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Vehicle
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

    //protected override void OnRenderObject()
    //{
    //    base.OnRenderObject();
    //    if (Manager.Instance.debugLines)
    //    {
    //        if (target)
    //        {
    //            targetMat.SetPass(0);
    //            GL.Begin(GL.LINES);
    //            GL.Vertex(transform.position);
    //            GL.Vertex(target.transform.position);
    //            GL.End();
    //        }
    //    }
    //}

    //collision detection with wooden stakes
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Steak(Clone)")
        {
            Destroy(collision.gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().score++;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // decrement player health when a ghost or vampire is "hitting" them
            if (other.transform.parent != null && other.transform.parent.gameObject.tag == "Meat")
            {
            Destroy(other.gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().score++;
            Destroy(gameObject);
        }
        
    }
}

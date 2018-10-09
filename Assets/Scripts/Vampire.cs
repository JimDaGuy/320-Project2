using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Vehicle
{

    //attributes
    public Material targetMat;
    public GameObject target;

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
        ////avoid general obstacles only because they're at the zombie's eye level
        //foreach (GameObject obstacle in Manager.Instance.generalObstacles)
        //{
        //    ultimateForce += AvoidObstacle(obstacle.transform.position, Manager.Instance.generalObstacleRadius) * avoidObstacleWeight;
        //}
        ultimateForce = ultimateForce.normalized * maxForce;
        ApplyForce(ultimateForce);
    }

    //protected override void OnRenderObject()
    //{
    //    base.OnRenderObject();
    //    if(Manager.Instance.debugLines)
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
}

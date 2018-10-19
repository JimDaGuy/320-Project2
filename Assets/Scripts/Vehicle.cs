using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    //attributes
    //for debug lines
    public Material forwardMat;
    public Material rightMat;
    public Material futureMat;
    //for forces
    public Vector3 direction;
    public Vector3 position; //for determining where the obj should be on the screen
    public Vector3 velocity; //to be calculated in relation to net force acceleration
    public float maxSpeed;
    public Vector3 acceleration; //determind via force calculations
    public float mass;
    public float maxForce;
    public float radius;
    public float reactionDistance;
    //for weights
    public float seekWeight;
    public float wanderWeight;
    public float avoidObstacleWeight;
    public float separationWeight;
    protected int wanderRotation = 0;

    // Use this for initialization
    protected virtual void Start()
    {
        //start position wherever the vehicle is located
        position = transform.position;
        //start direction wherever the vehicle is facing
        direction = transform.forward;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CalcSteeringForces();
        UpdatePosition();
        SetTransform();
    }

    //abstract method child classes will use to calcuate their forces
    protected abstract void CalcSteeringForces();

    //helper method for updating position
    protected void UpdatePosition()
    {
        //grab (relative) start position every frame (for testing different positions inside the running scene)
        position = transform.position;
        //new movement formula
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        position += velocity * Time.deltaTime;
        direction = velocity.normalized;
        acceleration = Vector3.zero;
    }

    //helper method for setting position and rotation every frame
    protected void SetTransform()
    {
        transform.position = position;

        if (direction.sqrMagnitude > 0.1)//prevents look vector error
        {
            transform.forward = direction;
        }
    }

    //helper method for applying forces
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    //helper method for seeking
    public Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desiredVelocity = new Vector3(targetPos.x - gameObject.transform.position.x, targetPos.y - gameObject.transform.position.y, targetPos.z - gameObject.transform.position.z);
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        return desiredVelocity - velocity;
    }

    //helper method for pursuing
    public Vector3 Pursue(GameObject pursueTarget)
    {
        Vector3 targetPos = pursueTarget.transform.position + pursueTarget.GetComponent<Vehicle>().velocity;
        return Seek(targetPos);
    }

    //helper method for fleeing
    public Vector3 Flee(Vector3 fleePos)
    {
        Vector3 desiredVelocity = new Vector3(gameObject.transform.position.x - fleePos.x, gameObject.transform.position.y - fleePos.y, gameObject.transform.position.z - fleePos.z);
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        return desiredVelocity - velocity;
    }

    //helper method for evading
    public Vector3 Evade(GameObject evadeTarget)
    {
        Vector3 targetPos = evadeTarget.transform.position + evadeTarget.GetComponent<Vehicle>().velocity;
        return Flee(targetPos);
    }


    //helper method for wall avoidance
    public Vector3 AvoidObstacles()
    {
        RaycastHit hit = new RaycastHit();
        //check to see if the ray hits anything
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, reactionDistance))
        {
            Vector3 desiredVelocity = hit.normal * 3;
            // Find a spot in the world along that normal
            return Seek(desiredVelocity + hit.point);
        }
        else
        {
            return Vector3.zero;
        }
    }

    //helper method for wandering
    public Vector3 Wander()
    {
        //add a (limited) random rotation to the total rotation
        wanderRotation += Random.Range(-30, 31);
        //keep the var within reasonable values
        if(wanderRotation > 360)
        {
            wanderRotation = wanderRotation - 360;
        }
        else if(wanderRotation < -360)
        {
            wanderRotation = wanderRotation + 360;
        }
        Vector3 desiredVelocity = gameObject.transform.forward * 2 + Quaternion.Euler(0, 90 + wanderRotation, 0) * new Vector3(1,0,1).normalized;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        return desiredVelocity - velocity;
    }

    //helper method for flock separation
    public Vector3 Separation(GameObject[] neighbors)
    {
        //only "neighbor" found was itself
        if(neighbors.Length < 2)
        {
            return Vector3.zero;
        }
        else
        {
            Vector3 desiredVelocity = new Vector3(0, 0, 0);
            foreach (GameObject neighbor in neighbors)
            {
                float distanceToNeighbor = (transform.position - neighbor.transform.position).sqrMagnitude;
                //looking at itself, ignore
                if(distanceToNeighbor == 0)
                {
                    continue;
                }
                else
                {
                    //see if the neighbor is close enough to worry about
                    if(distanceToNeighbor > 4)
                    {
                        continue;
                    }
                    else
                    {
                        desiredVelocity += AvoidObstacles() * (1/Mathf.Sqrt(distanceToNeighbor)); //appends a proportional velocity w/ an AvoidObstacle call (which assumes neighbors have a radius of 1)
                    }
                }
            }
            desiredVelocity = desiredVelocity.normalized * maxSpeed;
            //check to make sure desired velocity isn't just the zero vector
            if(desiredVelocity.sqrMagnitude == 0)
            {
                return Vector3.zero;
            }
            else
            {
                return desiredVelocity - velocity;
            }
        }
    }

    //helper method for applying friction force
    public void ApplyFriction(float fCoeff)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * fCoeff;
        ApplyForce(friction);
    }

    //helper method for collision
    public bool CircleCollision(GameObject checkedObj)
    {
        Vector3 cToC = checkedObj.transform.position - gameObject.transform.position;
        if(Mathf.Pow(checkedObj.GetComponent<Vehicle>().radius + radius, 2) < cToC.sqrMagnitude)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //for drawing debug lines in the actual game
    protected virtual void OnRenderObject()
    {
        if (Manager.Instance.debugLines)
        {
            // forward vector
            forwardMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(transform.position);
            GL.Vertex(transform.position + transform.forward * 2);
            GL.End();

            //right vector
            rightMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(transform.position);
            GL.Vertex(transform.position + transform.right * 2);
            GL.End();

            //future position vector
            futureMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(transform.position);
            GL.Vertex(transform.position + velocity);
            GL.End();
        }
    }
}

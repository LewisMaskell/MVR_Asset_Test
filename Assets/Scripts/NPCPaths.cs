using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public abstract class NPCPath
{
    public abstract void Step(float t);
    protected NPCMover sunseeker;
    protected Vector3 origin;
    protected float speed;
    protected float step;
    protected float distance;
    protected NPCPath(NPCMover sunseeker, Vector3 origin, float speed)
    {
        this.sunseeker = sunseeker;
        this.origin = origin;
        this.speed = speed;
    }
}

public class CirclePath : NPCPath
{
    public CirclePath(NPCMover sunseeker, Vector3 origin, float speed, float distance) : base(sunseeker, origin, speed)
    {
        this.distance = distance;
    }

    public override void Step(float t)
    {
        //step = (step + t*speed) % distance;
        step = (step + (t * speed / 10)) % (Mathf.PI * 2);
        Vector3 nextStep = new Vector3
        (
            origin.x + Mathf.Cos(step) * distance,
            origin.y,
            origin.z + Mathf.Sin(step) * distance
        );
        sunseeker.transform.rotation = Quaternion.LookRotation((nextStep - sunseeker.transform.position).normalized);
        sunseeker.transform.position = nextStep;

    }
}


//since Unity2021 does not have any packages available for creating splines, I instead opted into making a "patrol" system where the sunseeker will move between waypoints, however implements smooth rotation to make the movement seem somewhat realistic
public class BezierPath : NPCPath
{
    private Transform[] waypoints;
    private int next;

    private Quaternion nextRotation;
    private float angularSpeed;
    public BezierPath(NPCMover sunseeker, Vector3 origin, float speed, float angularSpeed, Transform[] waypoints) : base(sunseeker, origin, speed)
    {
        this.angularSpeed = angularSpeed;
        this.waypoints = waypoints;
        next = 1;
        
        sunseeker.transform.position = waypoints[0].transform.position;
        sunseeker.transform.rotation = Quaternion.LookRotation((waypoints[next].transform.position - sunseeker.transform.position).normalized);

    }
    public override void Step(float t)
    {

        if (Vector3.Distance(sunseeker.transform.position, waypoints[next].position) <= 40f)
        {
            next = (next + 1) % waypoints.Length;
            
        }
        nextRotation = Quaternion.LookRotation((waypoints[next].position - sunseeker.transform.position));
        sunseeker.transform.rotation = Quaternion.RotateTowards(sunseeker.transform.rotation, nextRotation, angularSpeed * t);
        sunseeker.transform.position += sunseeker.transform.forward * speed * t;

    }
}

public class FigureEightPath : NPCPath
{
    
    public FigureEightPath(NPCMover sunseeker, Vector3 origin, float speed, float distance) : base(sunseeker, origin, speed)
    {
        
        this.distance = distance;
        sunseeker.transform.position = origin;
        
    }
    public override void Step(float t)
    {
            
        step = (step + (t * speed * .01f)) % (Mathf.PI * 2);
        //implement parametric equation for figure 8 curves
        //x = a sin(t)
        //y = a sin(t)cos(t)
        Vector3 nextStep = new Vector3
            (
                origin.x + (distance * Mathf.Sin(step)),
                origin.y,
                origin.z + (distance *Mathf.Sin(step) * Mathf.Cos(step))

            );
        
        sunseeker.transform.rotation = Quaternion.LookRotation((nextStep - sunseeker.transform.position).normalized);
        sunseeker.transform.position = nextStep;
        
    }
}
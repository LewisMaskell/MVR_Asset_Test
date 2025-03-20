using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMover : MonoBehaviour
{
    private NPCPath currentPath;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void SetState(NPCPath path)
    {
        currentPath = path;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        currentPath.Step(Time.deltaTime);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Constantly spins plane propeller & planes in shop by rotating at constant rate
public class Spin : MonoBehaviour {
    private int speed = 20; 

    void Update()
    {
        //The object transforms/rotates on the y axis (speed is 20, which is multiplied by the time between frames)
        transform.Rotate(0, speed*Time.deltaTime, 0);
    }
}

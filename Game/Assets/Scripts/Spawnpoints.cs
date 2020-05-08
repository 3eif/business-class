using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoints : MonoBehaviour
{
    public static Transform[] points;
    public Transform question;

    //This function gets all the spawnpoints for the questions, and replaces them with question cubes in their position
    void Awake(){
        points = new Transform[transform.childCount];
        for(int i = 0; i < points.Length; i++){
            points[i] = transform.GetChild(i);
            Instantiate(question, new Vector3(points[i].position.x, points[i].position.y, points[i].position.z), question.rotation);
        }
    }
}

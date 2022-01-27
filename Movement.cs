using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float speedMoving = 1;
    float speedRotation = 5;
    List<Map> PathMap = new List<Map>();

    void Update()
    {
        KeyHandler();
        Move();
    }

    Vector3 Calibrate(Vector3 curV3)
    {
        Vector3 newV3 = new Vector3((float)Math.Round((double)curV3.x, 1),
                                    (float)Math.Round((double)curV3.y, 1),
                                    (float)Math.Round((double)curV3.z, 1));

        return newV3;
    }

    Quaternion Calibrate(Quaternion curQ)
    {
        
        Quaternion newQ = new Quaternion(Mathf.Round(curQ.x),
                                         Mathf.Round(curQ.y),
                                         Mathf.Round(curQ.z),
                                         Mathf.Round(curQ.w));
        return newQ;
    }

    void newPath(int Angle)
    {
        Map path;
        var newDir = Quaternion.AngleAxis(Angle, Vector3.forward);

        if (transform.rotation != newDir)
        {
            path = new Map(newDir, 0);
            PathMap.Add(path);
        }
        else if (PathMap.Count < 10)
        {
            if (PathMap.Count == 0)
            {
                path = new Map(newDir, 1);
                PathMap.Add(path);
            }
            else if (PathMap.Count > 1)
            {
                if (PathMap[1].Dir != newDir)
                {
                    path = new Map(newDir, 1);
                    PathMap.Add(path);
                }
            }
        }
    }

    void KeyHandler()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            newPath(0);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            newPath(180);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            newPath(270);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            newPath(90);
        }

        if (Input.GetKeyDown(KeyCode.Escape))   // STOP
        {
            if (PathMap.Count > 1)
            {
                PathMap.RemoveRange(1, PathMap.Count - 1);
            }
        }


        if (Input.GetKeyUp(KeyCode.W) ||
            Input.GetKeyUp(KeyCode.S) ||
            Input.GetKeyUp(KeyCode.D) ||
            Input.GetKeyUp(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.W))
            {
                newPath(0);
            }

            if (Input.GetKey(KeyCode.S))
            {
                newPath(180);
            }

            if (Input.GetKey(KeyCode.D))
            {
                newPath(270);
            }

            if (Input.GetKey(KeyCode.A))
            {
                newPath(90);
            }
        }



        //if (Input.GetKey(KeyCode.W))
        //{
        //    newPath(0);
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    newPath(180);
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    newPath(270);
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    newPath(90);
        //}
    }

    void Move()
    {
        if (PathMap.Count != 0)
        {
            if (transform.rotation != PathMap[0].Dir)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, PathMap[0].Dir, speedRotation);
            }
            else
            {
                if (PathMap[0].Length > 0)
                {
                    Map curPath = PathMap[0];

                    transform.Translate(Vector3.up * Mathf.Min(curPath.Length, speedMoving * Time.deltaTime));

                    curPath.Length -= speedMoving * Time.deltaTime;

                    if (curPath.Length > 0)
                    {
                        PathMap[0] = curPath;
                    }
                    else
                    {
                        transform.rotation = Calibrate(transform.rotation);
                        transform.position = Calibrate(transform.position);
                        
                        PathMap.RemoveAt(0);
                    }
                }
                else //if (PathMap[0].Length < 0.1)
                {
                    transform.rotation = Calibrate(transform.rotation);
                    PathMap.RemoveAt(0);
                }
            }
        }
    }

    struct Map
    {
        Quaternion dir;
        float len;

        public Map(Quaternion Direction, float Length)
        {
            dir = Direction;
            len = Length;
        }

        public Quaternion Dir { get => dir; set => dir = value; }
        public float Length { get => len; set => len = value; }
    }
}

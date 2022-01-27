using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Animator animatorL;
    Animator animatorR;
    GameObject TrackL;
    GameObject TrackR;

    float speedMoving = 1;
    float speedRotation = 5;
    List<Map> PathMap = new List<Map>();
    float time0;
    float delay = 0.2f;

    private void Start()
    {
        TrackL = transform.Find("TrackL").gameObject;
        TrackR = transform.Find("TrackR").gameObject;
        animatorL = TrackL.GetComponent<Animator>();
        animatorR = TrackR.GetComponent<Animator>();
    }

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

    void newPath(int Angle, bool IsJustPressed)
    {
        Map path;
        var newDir = Quaternion.AngleAxis(Angle, Vector3.forward);

        if (IsJustPressed)
        {
            if ((transform.rotation != newDir) && (PathMap.Count == 0))
            {
                path = new Map(newDir, 0);
                time0 = Time.time;
            }
            else
            {
                path = new Map(newDir, 1);
            }

            if (PathMap.Count < 10)
            {
                PathMap.Add(path);
            }
        }
        else
        {
            path = new Map(newDir, 1);

            if ((PathMap.Count < 1) && ((Time.time - time0) > delay))
            {
                PathMap.Add(path);
            }
        }
    }

    void KeyHandler()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            newPath(0, true);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            newPath(180, true);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            newPath(270, true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            newPath(90, true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))   // STOP
        {
            if (PathMap.Count > 1)
            {
                PathMap.RemoveRange(1, PathMap.Count - 1);
            }
        }

        if (!Input.GetKey(KeyCode.W) &&    // Конечный пункт - поворот
            !Input.GetKey(KeyCode.S) &&
            !Input.GetKey(KeyCode.D) &&
            !Input.GetKey(KeyCode.A))
        {
            if ((PathMap.Count >= 2) && (PathMap[PathMap.Count - 1].Dir != PathMap[PathMap.Count - 2].Dir))
            {
                Map path = new Map(PathMap[PathMap.Count - 1].Dir, 0);
                PathMap[PathMap.Count - 1] = path;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            newPath(0, false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            newPath(180, false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            newPath(270, false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            newPath(90, false);
        }
    }

    void Move()
    {
        if (PathMap.Count != 0)
        {
            animatorL.SetBool("Going", true); // Начать анимацию
            animatorR.SetBool("Going", true); // Начать анимацию

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

                        animatorL.SetBool("Going", false); // Остановить анимацию
                        animatorR.SetBool("Going", false); // Остановить анимацию
                    }
                }
                else
                {
                    transform.rotation = Calibrate(transform.rotation);
                    PathMap.RemoveAt(0);

                    animatorL.SetBool("Going", false); // Остановить анимацию
                    animatorR.SetBool("Going", false); // Остановить анимацию
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

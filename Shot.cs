using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject Shell;
    float speedShot = 1;
    float speedOfShell = 3;
    float time0 = 0;
    List<GameObject> Shells = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && ((Time.time - time0) > speedShot))
        {
            Shells.Add(Instantiate(Shell, transform.position, transform.rotation));
            time0 = Time.time;
        }
        Move();
    }

    void Move()
    {
        if (Shells.Count != 0)
        {
            for (int i = 0; i < Shells.Count; i++)
            {
                Shells[i].transform.Translate(Vector3.up * speedOfShell * Time.deltaTime);

                if ((Mathf.Abs(Shells[i].transform.position.x) > 7) || (Mathf.Abs(Shells[i].transform.position.y) > 7))
                {
                    Destroy(Shells[i]);
                    Shells.RemoveAt(i);
                    
                }
            }
        }
    }
}

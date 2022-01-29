using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject Shell;
    
    float speedShot = 1;
    float speedOfShell = 3;
    float timeS0 = 0;

    Animator aFire;

    GameObject curShell;
    List<GameObject> Shells = new List<GameObject>();

    void Start()
    {
        aFire = transform.Find("Tower").gameObject.transform.Find("Fire").gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && ((Time.time - timeS0) > speedShot))
        {
            // Добавление на сцену анимации огня
            aFire.SetTrigger("Fire");

            // Добавление на сцену анимации снаряда
            Vector3 posShell = new Vector3(transform.position.x,
                                           transform.position.y + 0.6f,
                                           transform.position.z);

            curShell = Instantiate(Shell, posShell, Quaternion.identity);
            curShell.transform.RotateAround(transform.position,
                                            Vector3.forward,
                                            transform.rotation.eulerAngles.z);
            Shells.Add(curShell);
            timeS0 = Time.time;
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

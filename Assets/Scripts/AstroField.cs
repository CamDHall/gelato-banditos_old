﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroField : MonoBehaviour {

    bool populated = false;
    bool turnedOff = false;
    List<GameObject> field = new List<GameObject>();

    public GameObject astroid;
    int size;

    int numAstroids;

    BoxCollider box;
    float camDepth;

    float playerDist = 0;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        numAstroids = Random.Range(10, 15);
        size = AstroSpawner.Instance.col_size;

        camDepth = Camera.main.farClipPlane;
    }

    private void Update()
    {
        playerDist = Vector3.Distance(PlayerMovement.player.transform.position, box.ClosestPoint(PlayerMovement.player.transform.position));
        if (!populated)
        {
            if (playerDist < camDepth)
            {
                populated = true;
                Populate();
            }
        }
        else
        {
            // Turn off if far away
            if (playerDist > camDepth)
            {
                if (!turnedOff)
                {
                    TurnOff();
                }
            }

            // Turn on if retentering quadrant
            if (Vector3.Distance(PlayerMovement.player.transform.position, box.ClosestPoint(PlayerMovement.player.transform.position)) < (camDepth + 100))
            {
                if (turnedOff)
                {
                    turnedOff = false;
                    Populate();
                }
            }
        }
    }

    void Populate()
    {
        for (int i = 0; i < numAstroids; i++)
        {
            Vector3 Pos = new Vector3((float)Random.Range(-size/2, size/2), (float)Random.Range(-size/2, size/2), (float)Random.Range(-size/2, size/2));
            GameObject temp = Instantiate(astroid);
            temp.transform.parent = transform;
            temp.transform.localPosition = Pos;
            field.Add(temp);
            //AstroSpawner.Instance.astroids.Add(temp, Random.Range(2, 5));
            AstroSpawner.Instance.astroids.Add(temp, 1);

            if(Random.Range(0, 100) > 95)
            {
                BanditoSpawner.Instance.SpawnEnemies(temp);
            }
        }
    }

    void TurnOff()
    {
        turnedOff = true;

        GameObject[] temp = field.ToArray();

        foreach(GameObject obj in temp)
        {
            if(obj == null)
            {
                field.Remove(obj);
            }
        }

        foreach(GameObject astro in field)
        {
            astro.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        Debug.Log(coll.gameObject);
    }
}

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

    public List<GameObject> enemies = new List<GameObject>();

    float playerDist = 0;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        numAstroids = Random.Range(90, 100);
        size = AstroSpawner.Instance.col_size;

        camDepth = Camera.main.farClipPlane;
    }

    private void Update()
    {
        playerDist = Vector3.Distance(PlayerMovement.player.transform.position, box.ClosestPoint(PlayerMovement.player.transform.position));

        // Turn off if far away
        if (playerDist > camDepth)
        {
            if (!turnedOff)
            {
                TurnOff();
            }
        }

        if (playerDist < camDepth)
        {
            if (!populated)
            {
                Populate();
            }
            else
            {
                if (turnedOff)
                {
                    TurnOn();
                }
            }
        }
    }

    void Populate()
    {
        populated = true;
        for (int i = 0; i < numAstroids; i++)
        {
            Vector3 Pos = new Vector3((float)Random.Range(-size/2, size/2), (float)Random.Range(-size/2, size/2), (float)Random.Range(-size/2, size/2));
            GameObject temp = Instantiate(astroid);
            temp.transform.parent = transform;
            temp.transform.localPosition = Pos;
            field.Add(temp);
            //AstroSpawner.Instance.astroids.Add(temp, Random.Range(2, 5));
            AstroSpawner.Instance.astroids.Add(temp, 1);

            if(Random.Range(0, 100) > 80)
            {
                BanditoSpawner.Instance.SpawnEnemies(temp, gameObject);
            }
        }

        TurnOn();
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

        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }

    void TurnOn()
    {
        turnedOff = false;

        GameObject[] temp = field.ToArray();

        foreach (GameObject obj in temp)
        {
            if (obj == null)
            {
                field.Remove(obj);
            }
        }

        foreach (GameObject astro in field)
        {
            astro.SetActive(true);
        }

        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }
}

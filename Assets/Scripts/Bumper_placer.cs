﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper_placer : MonoBehaviour {

    public List<GameObject> Bumpers;
    public List<Transform> SpawnPoints;
    public List<GameObject> spawnedBumpers;

    public int Spawns;
    public int randomspawn;

    public float timer = 5f;
	
	void Start () {
        for (int i = 0; i < Spawns; i++)
        {
            randomspawn = Random.Range(0, SpawnPoints.Count);

            Transform spawnPosition = SpawnPoints[randomspawn];
            
            Instantiate(Bumpers[Random.Range(0, Bumpers.Count)], spawnPosition);
            SpawnPoints.RemoveAt(randomspawn);
        }
	}
	

	void Update () {
        timer -= Time.deltaTime;
        for (int i = 0; i < Spawns; i++)
        {

        }

        if(timer <= 0)
        {

        }
	}
}

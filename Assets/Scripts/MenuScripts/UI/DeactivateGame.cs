﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeactivateGame : Button_3D {

    public GameObject spawnBig, spawnSmall;
    public GameObject panelToEnable;
    public GameObject panelToDisable;
    public ChangePlayBoardSize c_p_b;

     void FixedUpdate()
    {
        buttonPressed.AddListener(OnStart);
    }

    void OnStart()
    {
        spawnSmall = GameObject.FindGameObjectWithTag("SpawnerKlein");
        spawnBig = GameObject.FindGameObjectWithTag("SpawnerBig");
        panelToDisable.SetActive(false);
        panelToEnable.SetActive(true);
        if (c_p_b.isBoardSmall == true)
        {
            foreach (Transform child in spawnSmall.transform)
            {
                child.gameObject.SetActive(false);
            }
            spawnSmall.GetComponent<Bumper_placer>().enabled = false;
        }
        else
        {
            foreach (Transform child in spawnBig.transform)
            {
                child.gameObject.SetActive(false);
            }
            spawnBig.GetComponent<Bumper_placer>().enabled = false;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PointSystem : MonoBehaviour
{
    [Header("Texts")]
    public Text points_Var;

    [Header("Objects")]
    public GameObject spawnSmall, spawnBig;

    [Header("Scores")]
    public int score = 0;
    public int targetScore = 0;
    public int pointSlider;

    public void AddPoints()
    {
        pointSlider = PlayerPrefs.GetInt("User_" + PlayerPrefs.GetString("User") + "PointAmount");
        if (targetScore <= 16)
        targetScore += 1;
        score += 100 * (targetScore + 1);
        points_Var.text = score.ToString();
        Debug.Log(targetScore);
        // PlayerPrefs.SetInt("User_" + PlayerPrefs.GetString("User") + "Score", score);
        //PlayerPrefs.SetInt("User_" + PlayerPrefs.GetString("User") + "TargetScore", targetScore);
        if (targetScore == 2 + (1 * pointSlider) || targetScore == 6 + (1 * pointSlider) || targetScore == 10 + (1 * pointSlider) || targetScore == 12 + (1 * pointSlider))
        {
            Debug.Log("test");
            spawnSmall.GetComponent<Bumper_placer>().LevelUpTile();
            spawnBig.GetComponent<Bumper_placer>().LevelUpTile();
        }
        else if (targetScore == 4 + (1 * pointSlider) || targetScore == 8 + (1 * pointSlider))
        {
            spawnSmall.GetComponent<Bumper_placer>().LevelUpBoard();
            spawnBig.GetComponent<Bumper_placer>().LevelUpBoard();
        }
    }

    public void Missed()
    {
        pointSlider = PlayerPrefs.GetInt("User_" + PlayerPrefs.GetString("User") + "PointAmount");
        if (targetScore >= 1)
            targetScore -= 1;
        //PlayerPrefs.SetInt("User_" + PlayerPrefs.GetString("User") + "TargetScore", targetScore);
        if (targetScore == 1 + (1 * pointSlider) || targetScore == 5 + (1 * pointSlider) || targetScore == 9 + (1 * pointSlider) || targetScore == 11 + (1 * pointSlider))
        {
            spawnSmall.GetComponent<Bumper_placer>().LevelDownTile();
            spawnBig.GetComponent<Bumper_placer>().LevelDownTile();
        }
        else if (targetScore == 3 + (1 * pointSlider) || targetScore == 7 + (1 * pointSlider))
        {
            spawnSmall.GetComponent<Bumper_placer>().LevelDownBoard();
            spawnBig.GetComponent<Bumper_placer>().LevelDownBoard();
        }
    }

    public void ResetScene()
    {
        //still needs to be added
    }
}

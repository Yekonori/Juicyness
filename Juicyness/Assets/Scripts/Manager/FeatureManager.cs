﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureManager : MonoBehaviour
{
    private GameObject player;
    private bool isPlayerBlack = false;
    private bool areEnemiesBlack = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onPlayerAssigned += () =>
        {
            player = GameManager.instance.player;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            string input = Input.inputString;
            switch (input)
            {
                case "1":
                case "&":
                    PlayerBecomesBlack();
                    break;
                case "2":
                case "é":
                    EnemiesBecomeBlack();
                    break;
                case "3":
                case @"""":
                    print("Third feature");
                    break;
                case "4":
                case "'":
                    print("Fourth feature");
                    break;
                case "5":
                case "(":
                    print("Fifth feature");
                    break;
                case "6":
                case "-":
                    print("Sixth feature");
                    break;
                case "7":
                case "è":
                    print("Seventh feature");
                    break;
                case "8":
                case "_":
                    print("Eigth feature");
                    break;
                default:
                    Debug.Log("Pressed char: " + Input.inputString);
                    break;
            }
        }
    }

    public void PlayerBecomesBlack()
    {
        if (!isPlayerBlack)
        {
            isPlayerBlack = true;
            player.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            isPlayerBlack = false;
            player.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    public void EnemiesBecomeBlack()
    {
        if (!areEnemiesBlack)
        {
            areEnemiesBlack = true;
            foreach(EnemyMouvementManager enemyLine in EnemyManager.instance.enemiesMouvementManager)
            {
                foreach (GameObject enemy in enemyLine.enemies)
                {
                    enemy.GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
        }
        else
        {
            areEnemiesBlack = false; 
            foreach (EnemyMouvementManager enemyLine in EnemyManager.instance.enemiesMouvementManager)
            {
                foreach (GameObject enemy in enemyLine.enemies)
                {
                    enemy.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }

}
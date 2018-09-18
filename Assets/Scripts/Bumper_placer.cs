﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnEntity
{
    Empty, Bumper, Ball
}

public enum Direction
{
    Up, Down, Right, Left
}

public enum BorderSide
{
    Nothing = 0, Top = 1, Bottom = 2, Right = 4, Left = 8
}

public class Bumper_placer : MonoBehaviour
{
    [Header("External Scripts")]
    public Ball_Spawn b_s;

    [Header("Misc")]
    public GameObject[] bumpers;
    public List<GameObject> spawnedBumpers = new List<GameObject>();
    public int spawns;
    public float timer = 2.5f;
    public bool timerOn;
    [SerializeField] private float bumperSpawnAmount = 2;

    public static Bumper_placer instance;
    Direction ballDirection;
    SpawnEntity spawnPlace;
    bool lastBumperIsBumper1;
    int lastBumperIsBumperInt1;

    [HideInInspector] public SpawnEntity[,] board = new SpawnEntity[6, 6];
    private Vector2 ballBoardIndex;
    Vector2 lastBumperIndex;
    Vector2 randomBumperIndex = Vector2.zero;
    private Direction ballDirectionIndex;
    public float horizontalSpacing;
    public float verticalSpacing;

    public int testBumpersToSpawn = 4;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        timerOn = true;
        RandomizeBallPosition();
        GenerateBumpers(testBumpersToSpawn);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && timerOn == true)
        {
            BumperVisibility();
           b_s.instantiatedArrow.GetComponent<MeshRenderer>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            GenerateBumpers(testBumpersToSpawn);
        }
    }

    void GenerateBumpers(int bumperAmount)
    {
        //Randomize ball position first because the bumper needs to be aware of where the ball spawns before it can be generated

        print("Ball position: " + ballBoardIndex.x + "\t" + ballBoardIndex.y);

        for (int i = 0; i < bumperAmount; i++)
        {
            GenerateBumper(i == 0);
        }
    }

    void GenerateBumper(bool firstIteration)
    {

        //Vector2 randomBumperIndex = Vector2.zero;
        if (firstIteration)
        {
            lastBumperIsBumperInt1 = Random.Range(0, 2);
            if (lastBumperIsBumperInt1 == 0)
            {
                lastBumperIsBumper1 = true;
            }
            else
            {
                lastBumperIsBumper1 = false;
            }

            //Calculate a new random bumper index using the location of where the ball spawns
            if (ballBoardIndex.x == 0)
            {
                ballDirection = Direction.Right;
                randomBumperIndex = GenerateRandomBumperIndex();
                randomBumperIndex.y = ballBoardIndex.y;
            }
            else if (ballBoardIndex.y == 0)
            {
                ballDirection = Direction.Up;
                randomBumperIndex = GenerateRandomBumperIndex();
                randomBumperIndex.x = ballBoardIndex.x;
            }
            else if (ballBoardIndex.x == board.GetLength(0) - 1)
            {
                ballDirection = Direction.Left;
                randomBumperIndex = GenerateRandomBumperIndex();
                randomBumperIndex.y = ballBoardIndex.y;
            }
            else if (ballBoardIndex.y == board.GetLength(0)-1)
            {
                ballDirection = Direction.Down;
                randomBumperIndex = GenerateRandomBumperIndex();
                randomBumperIndex.x = ballBoardIndex.x;
            }
            Debug.Log(ballDirection + "ded");
        }
        else
        {
            //Calculate a new random bumper index using the location of the last bumper
            if (ballDirection == Direction.Up)
            {
                ballDirection = lastBumperIsBumper1 ? Direction.Right : Direction.Left;

                randomBumperIndex.x = lastBumperIsBumper1 ? Random.Range(randomBumperIndex.y + 1, board.GetLength(0) - 2) : Random.Range(1, randomBumperIndex.y - 1);
                GetBumperSide();
                BorderSide bumperSide = GetBumperSide();
                //make function that returns on what side the bumper is placed.
                if (IsValidBumper(bumperSide, ballDirection, lastBumperIsBumper1))
                {
                    lastBumperIsBumperInt1 = lastBumperIsBumper1 ? 0 : 1;
                    ballDirection = lastBumperIsBumper1 ? Direction.Right : Direction.Left;
                }

            }
            else if (ballDirection == Direction.Down)
            {
                ballDirection = lastBumperIsBumper1 ? Direction.Left : Direction.Right;
                randomBumperIndex.x = lastBumperIsBumper1 ? Random.Range(1, randomBumperIndex.y - 1) : Random.Range(randomBumperIndex.y + 1, board.GetLength(0) - 2);
                GetBumperSide();
                BorderSide bumperSide = GetBumperSide();
                //make function that returns on what side the bumper is placed.
                if (!IsValidBumper(bumperSide, ballDirection, lastBumperIsBumper1))
                {
                    lastBumperIsBumperInt1 = lastBumperIsBumper1 ? 0 : 1;
                    ballDirection = lastBumperIsBumper1 ? Direction.Left : Direction.Right;
                }
            }
            else if (ballDirection == Direction.Left)
            {
                ballDirection = lastBumperIsBumper1 ? Direction.Down: Direction.Up;
                randomBumperIndex.y = lastBumperIsBumper1 ? Random.Range(1, randomBumperIndex.x - 1) : Random.Range(randomBumperIndex.x + 1, board.GetLength(0) - 2);
                GetBumperSide();
                BorderSide bumperSide = GetBumperSide();
                //make function that returns on what side the bumper is placed.
                if (!IsValidBumper(bumperSide, ballDirection, lastBumperIsBumper1))
                {
                    lastBumperIsBumperInt1 = lastBumperIsBumper1 ? 0 : 1;
                    ballDirection = lastBumperIsBumper1 ? Direction.Down : Direction.Up;
                }
            }
            else if (ballDirection == Direction.Right)
            {
                ballDirection = lastBumperIsBumper1 ? Direction.Up : Direction.Down;
                randomBumperIndex.y = lastBumperIsBumper1 ? Random.Range(randomBumperIndex.x + 1, board.GetLength(0) - 2) : Random.Range(1, randomBumperIndex.x - 1);
                GetBumperSide();
                BorderSide bumperSide = GetBumperSide();
                //make function that returns on what side the bumper is placed.
                if (!IsValidBumper(bumperSide, ballDirection, lastBumperIsBumper1))
                {
                    lastBumperIsBumperInt1 = lastBumperIsBumper1 ? 0 : 1;
                    ballDirection = lastBumperIsBumper1 ? Direction.Up : Direction.Down;
                }
            }
            Debug.Log(ballDirection);
            lastBumperIsBumperInt1 = Random.Range(0, 2);
            if (lastBumperIsBumperInt1 == 0)
            {
                lastBumperIsBumper1 = true;
            }
            else
            {
                lastBumperIsBumper1 = false;
            }
        }

        spawnPlace = board[(int)randomBumperIndex.x, (int)randomBumperIndex.y];
        //lastBumperIsBumper1 = true;
        lastBumperIndex = randomBumperIndex;

        if (spawnPlace == SpawnEntity.Empty)
        {
            SpawnBumper(randomBumperIndex, lastBumperIsBumper1);
        }
        else
        {
            GenerateBumper(false);
        }
    }

    private bool IsValidBumper(BorderSide bumperSide, Direction ballDirection, bool isBumper1)
    {
        switch (bumperSide)
        {
            case BorderSide.Top:
                return isBumper1 ? ballDirection != Direction.Right : ballDirection != Direction.Left;
            case BorderSide.Bottom:
                return isBumper1 ? ballDirection != Direction.Left : ballDirection != Direction.Right;
            case BorderSide.Right:
                return isBumper1 ? ballDirection != Direction.Up : ballDirection != Direction.Down;
            case BorderSide.Left:
                return isBumper1 ? ballDirection != Direction.Down : ballDirection != Direction.Up;
            case BorderSide.Left | BorderSide.Bottom: //top right corner
                return isBumper1 ? ballDirection == Direction.Up && ballDirection == Direction.Right : ballDirection == Direction.Down && ballDirection == Direction.Left;
            case BorderSide.Left | BorderSide.Top: //bottom right corner
                return isBumper1 ? ballDirection == Direction.Up && ballDirection == Direction.Left : ballDirection == Direction.Down && ballDirection == Direction.Right;
            case BorderSide.Right | BorderSide.Bottom://top left corner
                return isBumper1 ? ballDirection == Direction.Down && ballDirection == Direction.Right : ballDirection == Direction.Up && ballDirection == Direction.Left;
            case BorderSide.Right | BorderSide.Top://bottom left corner
                return isBumper1 ? ballDirection == Direction.Down && ballDirection == Direction.Left : ballDirection == Direction.Up && ballDirection == Direction.Right;
            default:
                return false;
        }
    }

    BorderSide GetBumperSide()
    {
        if (randomBumperIndex.y == 1)
        {
            return BorderSide.Bottom;
        }
        else if (randomBumperIndex.x == 1)
        {
            return BorderSide.Left;
        }
        else if (randomBumperIndex.x == board.GetLength(0) - 2)
        {
            return BorderSide.Right;
        }
        else if (randomBumperIndex.y == board.GetLength(0) - 2)
        {
            return BorderSide.Top;
        }
        return BorderSide.Nothing;
    }


    private void SpawnBumper(Vector2 randomBumperIndex, bool bumperDirection)
    {
        board[(int)randomBumperIndex.x, (int)randomBumperIndex.y] = SpawnEntity.Bumper;
        Vector3 position = GetSpawnPositionByIndex((int)randomBumperIndex.x, (int)randomBumperIndex.y);

        //Calculate rotation
        Quaternion rotation = Quaternion.Euler(new Vector3(0, -45, 0));
        if (bumperDirection != true)
            rotation = Quaternion.Euler(new Vector3(0, 45, 0));

        GameObject instantiatedBumper = Instantiate(bumpers[bumperDirection ? 0 : 1], transform);
        spawnedBumpers.Add(instantiatedBumper);
        Debug.Log(instantiatedBumper);
        instantiatedBumper.transform.position = position;
        instantiatedBumper.transform.rotation = rotation;
    }

    void BumperVisibility()
    {
        for (int i = 0; i < spawnedBumpers.Count; i++)
        {
            spawnedBumpers[i].GetComponent<MeshRenderer>().enabled = false;
           // Debug.Log("MeshOff");
            timerOn = false;
        }
    }

    private Vector2 GenerateRandomBumperIndex()
    {
        return new Vector2(Random.Range(1, board.GetLength(0) - 2), Random.Range(1, board.GetLength(0) - 2));
    }

    private void RandomizeBallPosition()
    {
        int randomDirection = Random.Range(0, 4);
        int randomBallPosition = Random.Range(1, 4);

        switch (randomDirection)
        {
            case 0:
                ballBoardIndex = new Vector2(0, randomBallPosition);
                board[0, randomBallPosition] = SpawnEntity.Ball;
                b_s.SetUpcomingBallPosition(GetSpawnPositionByIndex(0, randomBallPosition), Vector3.up * 90, transform);
                break;
            case 1:
                ballBoardIndex = new Vector2(board.GetLength(0) - 1, randomBallPosition);
                board[4, randomBallPosition] = SpawnEntity.Ball;
                b_s.SetUpcomingBallPosition(GetSpawnPositionByIndex(board.GetLength(0) - 1, randomBallPosition), Vector3.up * -90, transform);
                break;
            case 2:
                ballBoardIndex = new Vector2(randomBallPosition, 0);
                board[randomBallPosition, 0] = SpawnEntity.Ball;
                b_s.SetUpcomingBallPosition(GetSpawnPositionByIndex(randomBallPosition, 0), Vector3.zero, transform);
                break;
            case 3:
                ballBoardIndex = new Vector2(randomBallPosition, board.GetLength(0) - 1);
                board[randomBallPosition, board.GetLength(0) - 1] = SpawnEntity.Ball;
                b_s.SetUpcomingBallPosition(GetSpawnPositionByIndex(randomBallPosition, board.GetLength(0) - 1), Vector3.up * 180, transform);
                break;
        }

    }

    public Vector3 GetSpawnPositionByIndex(int x, int y)
    {
        Vector3 verticalOffset = Vector3.forward * verticalSpacing * y;
        Vector3 horizontalOffset = Vector3.right * horizontalSpacing * x;
        return transform.position + verticalOffset + horizontalOffset;
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Vector3 targetPos = transform.position + Vector3.right * horizontalSpacing * i +
                Vector3.forward * verticalSpacing * j;
                Gizmos.DrawCube(targetPos, Vector3.one * .5f);
            }
        }
    }
}
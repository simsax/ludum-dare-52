using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    Transform groundPrefab;

    [SerializeField]
    Transform playerPrefab;

    [SerializeField]
    Transform hayBalePrefab;

    [SerializeField]
    Transform rockPrefab;

    [SerializeField]
    GameOver gameOver;

    Transform[] ground;
    Transform player;
    List<Transform> hayBales;
    List<Transform> rocks;

    public Player playerScript;
    public float kmCounter = 0;

    float speed = 1f;

    bool left = false;
    bool right = false;
    public bool endGame = false;

    void GameOver()
    {
        endGame = true;
        gameOver.Setup(playerScript.bestHays);
    }

    void MovePlayerLeft()
    {
        Vector3 pos = player.localPosition;
        if (pos.x >= -0.5f)
        {
            pos.x -= 1f;
            player.localPosition = pos;
        }
    }

    void MovePlayerRight()
    {
        Vector3 pos = player.localPosition;
        if (pos.x <= 0.5f)
        {
            pos.x += 1f;
            player.localPosition = pos;
        }
    }

    bool ImpossibleConfiguration(int[,] field, int i, int j)
    {
        int rowCounter = 0;

        for (int x = 0; x < 4; x++)
            if (x != j && (field[i, x] == 1 || (i > 0 && field[i - 1, x] == 1) || (i > 1 && field[i - 2, x] == 1)))
                rowCounter++;
        if (rowCounter == 3)
            return true;

        // check main diag
        if (j == 0 && i < 5 && field[i + 1, 1] == 1 && field[i + 2, 2] == 1 && field[i + 3, 3] == 1 ||
            j == 1 && i < 6 && i > 0 && field[i - 1, 0] == 1 && field[i + 1, 2] == 1 && field[i + 2, 3] == 1 ||
            j == 2 && i < 7 && i > 1 && field[i - 2, 0] == 1 && field[i - 1, 1] == 1 && field[i + 1, 3] == 1 ||
            j == 3 && i > 2 && field[i - 3, 0] == 1 && field[i - 2, 1] == 1 && field[i - 1, 2] == 1 ||
            j == 3 && i < 5 && field[i + 1, 2] == 1 && field[i + 2, 1] == 1 && field[i + 3, 0] == 1 ||
            j == 2 && i < 6 && i > 0 && field[i - 1, 3] == 1 && field[i + 1, 1] == 1 && field[i + 2, 0] == 1 ||
            j == 1 && i < 7 && i > 1 && field[i - 2, 3] == 1 && field[i - 1, 2] == 1 && field[i + 1, 0] == 1 ||
            j == 0 && i > 2 && field[i - 3, 3] == 1 && field[i - 1, 1] == 1 && field[i - 2, 2] == 1)
            return true;

        return false;
    }

    void InstantiateEntities()
    {
        for (int i = 0; i < 32; i++)
        {
            Transform hayBale = Instantiate(hayBalePrefab);
            hayBale.gameObject.SetActive(false);
            hayBales.Add(hayBale);

            Transform rock = Instantiate(rockPrefab);
            rock.gameObject.SetActive(false);
            rocks.Add(rock);
        }
    }

    Transform GetInactiveRock()
    {
        for (int i = 0; i < rocks.Count; i++)
        {
            if (!rocks[i].gameObject.activeInHierarchy)
            {
                return rocks[i];
            }
        }
        return null;
    }

    Transform GetInactiveHayBale()
    {
        for (int i = 0; i < hayBales.Count; i++)
        {
            if (!hayBales[i].gameObject.activeInHierarchy)
            {
                return hayBales[i];
            }
        }
        return null;
    }

    void SpawnEntities()
    {
        int[,] field = new int[8, 4];
        for (int i = 4; i < 12; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Random.Range(0f, 1f) < 0.2)
                {
                    Transform hayBale = GetInactiveHayBale();
                    if (hayBale != null)
                    {
                        hayBale.localPosition = new Vector3(j - 1.5f, i + 0.5f, 0);
                        hayBale.gameObject.SetActive(true);
                    }
                }
                else if (Random.Range(0f, 1f) < 0.2 && !ImpossibleConfiguration(field, i - 4, j))
                {
                    field[i - 4, j] = 1;
                    Transform rock = GetInactiveRock();
                    if (rock != null)
                    {
                        rock.localPosition = new Vector3(j - 1.5f, i + 0.5f, 0);
                        rock.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    bool ScrollGround(float deltaTime)
    {
        bool newCycle = false;
        float deltaPos = speed * deltaTime;
        for (int i = 0; i < ground.Length; i++)
        {
            Vector3 pos = ground[i].localPosition;
            pos.y -= deltaPos;
            if (pos.y <= -8f)
            {
                int prev = i - 1;
                if (i == 0)
                    prev = 2;
                pos.y = 8f + ground[prev].localPosition.y - deltaPos;
                newCycle = true;
            }
            ground[i].localPosition = pos;
        }
        return newCycle;
    }

    void ScrollEntities(float deltaTime)
    {
        float deltaPos = speed * deltaTime;
        // haybales
        for (int i = 0; i < hayBales.Count; i++)
        {
            if (hayBales[i].gameObject.activeInHierarchy)
            {
                Vector3 pos = hayBales[i].localPosition;
                pos.y -= deltaPos;
                // deactivate if off screen
                if (pos.y < -4f)
                {
                    hayBales[i].gameObject.SetActive(false);
                }
                else
                    hayBales[i].localPosition = pos;
            }
        }
        // rocks
        for (int i = 0; i < rocks.Count; i++)
        {
            if (rocks[i].gameObject.activeInHierarchy)
            {
                Vector3 pos = rocks[i].localPosition;
                pos.y -= deltaPos;
                if (pos.y < -4f)
                {
                    rocks[i].gameObject.SetActive(false);
                }
                else
                    rocks[i].localPosition = pos;
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            left = true;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            right = true;
    }

    private void Awake()
    {
        // create ground
        ground = new Transform[3];
        for (int i = 0; i < ground.Length; i++)
        {
            ground[i] = Instantiate(groundPrefab);
            ground[i].localPosition = new Vector3(0f, 8f * i, 0f);
        }

        // create player
        player = Instantiate(playerPrefab);
        player.localPosition = new Vector3(-1.5f, -3.5f, 0f);

        playerScript = player.GetComponent<Player>();

        // create haybales
        hayBales = new List<Transform>();

        // create rocks
        rocks = new List<Transform>();
    }

    void Start()
    {
        InstantiateEntities();
        SpawnEntities();
    }

    void Update()
    {
        if (!endGame)
        {
            if (playerScript.hays < 0)
                GameOver();
            else
            {
                kmCounter += speed * Time.deltaTime * 0.01f;
                if (ScrollGround(Time.deltaTime))
                    SpawnEntities();
                ScrollEntities(Time.deltaTime);
                HandleInput();
            }
        }
    }

    private void FixedUpdate()
    {
        speed += 0.001f;
        if (left)
        {
            left = false;
            MovePlayerLeft();
        }
        if (right)
        {
            right = false;
            MovePlayerRight();
        }
    }
}

//////////////////////////////////////////////
//Assignment/Lab/Project: Breakout Game
//Name: Tony Zampino
//Section: SGD.213.0071
//Instructor: Aurore Locklear
//Date: 3/31/2026
/////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int lives = 3;
    public int bricks = 20;
    public float resetDelay = 1f;

    public TMP_Text livesText;
    public GameObject gameOver;
    public GameObject youWon;
    public GameObject[] brickPrefabs;   //using an array for the brick prefab so ever level is different
    public GameObject paddle;

    private GameObject clonePaddle;

    void Awake()
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded; 
    }
    else
    {
        Destroy(gameObject);
        return;
    }

    Setup(); // first level
}

void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    Setup(); //  runs every level change
}

    void Update()
    {
        // Skip level with N key
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (SceneManager.GetActiveScene().buildIndex < 2)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public void Setup()
    {
        clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity);
       int levelIndex = SceneManager.GetActiveScene().buildIndex;

GameObject newBricks = Instantiate(
    brickPrefabs[levelIndex],
    transform.position,
    Quaternion.identity
);


bricks = newBricks.transform.childCount;
    }

    void CheckGameOver()
    {
        if (bricks < 1)
        {
            if (SceneManager.GetActiveScene().buildIndex < 2)
            {
                lives++; // bonus life
                Invoke("NextLevel", resetDelay);
            }
            else
            {
                youWon.SetActive(true);
                Time.timeScale = .25f;
                Invoke("Reset", resetDelay);
            }
        }

        if (lives < 1)
        {
            gameOver.SetActive(true);
            Time.timeScale = .25f;
            Invoke("Reset", resetDelay);
        }
    }

    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Reset()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;
        Destroy(clonePaddle);
        Invoke("SetupPaddle", resetDelay);
        CheckGameOver();
    }

    void SetupPaddle()
    {
        clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity);
    }

    public void DestroyBrick()
    {
        bricks--;
        CheckGameOver();
    }
}
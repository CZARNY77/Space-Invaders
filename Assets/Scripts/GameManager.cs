using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    float initializeSpeedWorld;
    public float speedWorld;

    public GameObject[] shields;
    public int countShields = 0;
    public bool immortality = false;
    public bool endGame = false;
    public bool startGame = false;
    public bool inHole = false;
    [SerializeField] PlayerController player;

    [Header("UI")]
    [SerializeField] GameObject shieldPanel;
    [SerializeField] GameObject shieldPrefab;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI[] scoreTextEndPanel;
    [SerializeField] TextMeshProUGUI[] nameTextEndPanel;
    [SerializeField] TextMeshProUGUI youScoreText;
    float score;
    [SerializeField] GameObject endPanel;
    [SerializeField] GameObject namePanel;
    [SerializeField] TextMeshProUGUI inputName;
    [SerializeField] GameObject startPanel;

    void Awake()
    {
        if(instance == null) instance = this;
    }

    private void Start()
    {
        initializeSpeedWorld = speedWorld;
        shields = new GameObject[3];
        AddShields(2);
        startGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!endGame && startGame)
        {
            speedWorld += 0.01f * Time.deltaTime;
            UIUpdate();
        }
    }

    public void AddShields(int count)
    {
        for(int i = 0; i < count; i++)
        {
            if(countShields < 3)
            {
                shields[countShields] = Instantiate(shieldPrefab, Vector3.zero, Quaternion.identity, shieldPanel.transform);
                countShields++;
            }
        }
    }

    public void UseShield()
    {
        immortality = true;
        countShields--;
        
        if (countShields < 0)
        {
            EndGame();
            return;
        }
        Destroy(shields[countShields]);
    }

    void UIUpdate()
    {
        score += speedWorld * Time.deltaTime;
        scoreText.text = ((int)score).ToString();
    }

    public void NewGame()
    {
        endPanel.SetActive(false);
        namePanel.SetActive(false);
        player.AnimSetTrigger("newShield");

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach(GameObject obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }
        countShields = 0;
        score = 0f;
        AddShields(2);
        player.fireRate = 1f;
        player.startShoot();
        endGame = false;
        speedWorld = initializeSpeedWorld;
    }

    void EndGame()
    {
        endGame = true;
        speedWorld = 0f;
        player.stopShoot();
        if(PlayerPrefs.GetInt("ranking" + 4, 0) < (int)score)
            namePanel.SetActive(true);
        else
            NewHighScore();
    }

    public void StartGame()
    {
        if(!startGame)
        {
            startGame = true;
            startPanel.GetComponent<Animator>().SetTrigger("start");
            Invoke(nameof(DisableObject), 0.5f);
            player.startShoot();
        }
    }

    void DisableObject()
    {
        startPanel.SetActive(false);
    }

    public void NewHighScore()
    {
        int currentScore = (int)score;
        string currentName = SetName();
        int[] ranking = new int[5];
        string[] name = new string[5];
        for (int i = 0; i < 5; i++)
        {
            ranking[i] = PlayerPrefs.GetInt("ranking" + i, 0);
            name[i] = PlayerPrefs.GetString("name" + i, "Bezimienny");

        }

        if(ranking[4] < currentScore)
        {
            
            ranking[4] = currentScore;
            name[4] = currentName;
            for (int i = 3; i >= 0; i--)
            {
                if (ranking[i] < ranking[i+1])
                {
                    int temp = ranking[i];
                    ranking[i] = ranking[i+1];
                    ranking[i + 1] = temp;
                    string tempName = name[i];
                    name[i] = name[i + 1];
                    name[i + 1] = tempName;
                }
            }
        }

        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt("ranking" + i, ranking[i]);
            PlayerPrefs.SetString("name" + i, name[i]);
            scoreTextEndPanel[i].text = ranking[i].ToString();
            nameTextEndPanel[i].text = name[i];
        }
        youScoreText.text = currentScore.ToString();
        endPanel.SetActive(true);
        PlayerPrefs.Save();
    }

    string SetName()
    {
        string name = inputName.text != "" ? inputName.text : "bezimienny";
        return name;
    }
}

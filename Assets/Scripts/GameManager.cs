using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const int NUM_LEVELS = 3;
    private float gravity = 9.81f;


    public TextMeshProUGUI livesText;
    public TextMeshProUGUI pointsText;


    public GameObject buttonsPanel;

    public int level { get; private set; } = 1; // Nivel inicial
    public int lives { get; private set; } = 3;
    public int score { get; private set; } = 0;

    private bool isProcessingEvent = false; 

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        Debug.Log("GameManager started in scene: " + SceneManager.GetActiveScene().name);
    }
    private void UpdatePointsText()
    {
        if (pointsText != null)
        {
            pointsText.text = $"Points: {score}";
        }
    }

    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = $"Lives: {lives}";
        }
    }

    public void StartGame()
    {
        //Debug.Log("StartGame() called");  

        if (buttonsPanel != null)
        {
            buttonsPanel.SetActive(false);
        }

        NewGame();
    }

    private void NewGame()
    {
        isProcessingEvent = false; 
        lives = 3;
        score = 0;
        level = 1; // Asegura que comience en el primer nivel
        UpdateLivesText();
        UpdatePointsText();

        //Debug.Log("Starting game with gravity: " + gravity);
        LoadLevel(level);
    }

    private void LoadLevel(int level)
    {
        if (level > NUM_LEVELS)
        {
            LoadWinScene();
            return;
        }

        this.level = level;
        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.cullingMask = 0;
        }

        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }

    private void LoadScene()
    {
        SceneManager.LoadScene($"Level{level}");
    }

    public void LevelComplete()
    {
        if (isProcessingEvent) return; 
        isProcessingEvent = true;

        score += 1000;
        UpdatePointsText();

        Invoke(nameof(AdvanceToNextLevel), 1f); 
    }

    private void AdvanceToNextLevel()
    {
        isProcessingEvent = false;
        LoadLevel(level + 1);
    }

    public void LevelFailed()
    {
        if (isProcessingEvent) return; 
        isProcessingEvent = true;

        lives--;
        UpdateLivesText();
        score -= 250;
        UpdatePointsText();

        if (lives <= 0)
        {
            Invoke(nameof(NewGame), 1f);
        }
        else
        {
            Invoke(nameof(ReloadCurrentLevel), 1f); 
        }
    }

    private void ReloadCurrentLevel()
    {
        isProcessingEvent = false;
        LoadLevel(level);
    }

    public void SetGravity(float newGravity)
    {
        gravity = newGravity;
        Debug.Log("Gravity: " + gravity);
        Physics.gravity = new Vector3(0, -gravity, 0);
    }
}

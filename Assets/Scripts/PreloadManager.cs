using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PreloadManager : MonoBehaviour
{
    public Slider planetSlider;           // El slider para seleccionar el planeta
    public TextMeshProUGUI planetText;    // Texto que muestra el planeta seleccionado
    public TextMeshProUGUI gravityText;   // Texto que muestra la gravedad
    public Button startButton;            // Botón para iniciar el juego
    public Button exitButton;             // Botón para salir del juego
    public Canvas CanvasPreload;          // Referencia al CanvasPreload


    public Slider speedSlider;            // Slider para definir el speed
    public TextMeshProUGUI speedText;     // Texto para mostrar el valor de speed
    public Slider jumpingForceSlider;     // Slider para definir el jumping force
    public TextMeshProUGUI jumpingForceText; // Texto para mostrar el valor de jumping force


    private float gravity;                // Gravedad actual

    private readonly float[] planetGravities = new float[] {
        9.81f,    // Tierra
        3.7f,     // Marte
        24.8f,    // Júpiter
        8.6f,     // Saturno
        1.62f     // Luna
    };

    private readonly string[] planetNames = new string[] {
        "Earth",
        "Mars",
        "Jupiter",
        "Saturn",
        "Moon"
    };

    void Start()
    {
        // Valor inicial de la gravedad y el planeta
        UpdatePlanetInfo(planetSlider.value);  
        UpdateGravity(planetSlider.value);  
        UpdateSpeedText(speedSlider.value);
        UpdateJumpingForceText(jumpingForceSlider.value);

        // Asignar la función de cambio de valor del slider
        planetSlider.onValueChanged.AddListener((value) =>
        {
            UpdatePlanetInfo(value);
            UpdateGravity(value);
        });

        // Asignar las funciones de los botones
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    void UpdateSpeedText(float value)
    {
        speedText.text = "Speed: " + value.ToString("F1");
    }

    void UpdateJumpingForceText(float value)
    {
        jumpingForceText.text = "Jumping Force: " + value.ToString("F1");
    }

    void UpdatePlanetInfo(float value)
    {
        int planetIndex = Mathf.RoundToInt(value);  // Valor redondeado del slider

        planetIndex = Mathf.Clamp(planetIndex, 0, planetGravities.Length - 1);

        planetText.text = "Planet: " + planetNames[planetIndex];
        gravity = planetGravities[planetIndex];
        gravityText.text = "Gravity: " + gravity.ToString("F2") + " m/s²";
    }

    // Función para iniciar el juego

    public void UpdateGravity(float sliderValue)
    {
        int planetIndex = Mathf.RoundToInt(sliderValue);
        planetIndex = Mathf.Clamp(planetIndex, 0, planetGravities.Length - 1);

        gravity = planetGravities[planetIndex];
        Debug.Log($"Selected planet: {planetNames[planetIndex]} with gravity: {gravity}");
    }

    public void StartGame()
    {
        //Debug.Log("Starting game with gravity: " + gravity);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetGravity(gravity);
        }
        else
        {
            Debug.LogWarning("GameManager.Instance is null. Gravity not set."); 
        }

        // Desactivar el CanvasPreload
        if (CanvasPreload != null)
        {
            CanvasPreload.gameObject.SetActive(false); 
        }

        // Cargar la escena del juego

        GameManager.Instance.StartGame();

    }

    // Función para salir del juego
    void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}

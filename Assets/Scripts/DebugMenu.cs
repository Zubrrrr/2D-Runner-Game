using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ButtonSetting
{
    [Tooltip("The button component used for this setting.")]
    public Button Button;

    [Tooltip("The value associated with this button, like a target FPS or time scale.")]
    public float Value;
}

public class DebugMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Text field for displaying the current frames per second.")]
    private TMP_Text _fpsText;

    [SerializeField]
    [Tooltip("Text field for displaying the current game speed multiplier.")]
    private TMP_Text _speedText;

    [SerializeField]
    private TMP_Text _deadCountText;

    [SerializeField]
    [Tooltip("The audio source used for adjusting audio pitch based on the time scale.")]
    private AudioSource _music;

    [SerializeField]
    [Tooltip("Settings for FPS adjustment buttons.")]
    private ButtonSetting[] _fpsButtonSettings;

    [SerializeField]
    [Tooltip("Settings for game speed adjustment buttons.")]
    private ButtonSetting[] _speedButtonSettings;

    [SerializeField]
    [Tooltip("Console component for displaying and entering debug commands.")]
    private Console _console;

    private static int _targetFPS = 60;
    private static float _timeScale = 1f;

    private float _deltaTime = 0.0f;
    private bool _isPaused = false;
    private float _previousTimeScale = 1f;

    void Start()
    {
        SetTargetFPS(_targetFPS);
        SetTimeScale(_timeScale);
        UpdateButtonsState();
    }

    void Update()
    {
        FpsCounter();
        UpdateSpeedText();
        DeadCounter();
    }

    public void SetTargetFPS(int fps)
    {
        _targetFPS = fps;
        Application.targetFrameRate = fps;
        UpdateButtonsState();
    }

    public void SetTimeScale(float newTimeScale)
    {
        _timeScale = newTimeScale;
        UpdateButtonsState();

        if (!_isPaused)
        {
            Time.timeScale = newTimeScale;
        }

        Debug.Log("Setting time scale to: " + newTimeScale);

        _previousTimeScale = newTimeScale;
        _music.pitch = newTimeScale;
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            _music.Pause();

        }
        else
        {
            Time.timeScale = _previousTimeScale;
            _music.Play();
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // Если игра запущена в редакторе Unity, останавливаем игру в редакторе
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Если игра запущена в сборке, закрываем приложение
        Application.Quit();
#endif
    }

    public void ToggleConsoleDisplay()
    {
        if (_console != null)
        {
            _console.ToggleConsole();
        }
    }

    private void UpdateButtonsState()
    {
        foreach (var setting in _fpsButtonSettings)
        {
            setting.Button.interactable = (setting.Value != _targetFPS);
        }

        foreach (var setting in _speedButtonSettings)
        {
            bool isValueEqual = Mathf.Abs(setting.Value - _timeScale) < Mathf.Epsilon;
            setting.Button.interactable = !isValueEqual;
        }
    }

    private void FpsCounter()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        float fps = 1.0f / _deltaTime;
        _fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
    }

    private void UpdateSpeedText()
    {
        _speedText.text = "Speed: " + (_isPaused ? _previousTimeScale : Time.timeScale).ToString("0.00") + "x";
    }

    private void DeadCounter()
    {
        _deadCountText.text = "Dead counter: " + PlayerController.DeadCounter.ToString();
    }
}

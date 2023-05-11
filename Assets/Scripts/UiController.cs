using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    private bool isPaused = false;
    private StarterAssetsInputs _inputs;
    private GameObject _player;
    private CursorLockMode previousCursorLockMode;
    private GameObject _gameController;

    [SerializeField] private GameObject uiScreen;
    [SerializeField] private GameObject musicSlider;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _player = GameObject.FindGameObjectWithTag("Player");
        _inputs = _player.GetComponent<StarterAssetsInputs>();
        _gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    private void Start()
    {
        _gameController.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputs.pause)
        {
            if (isPaused)
            {
                PLayGame();
            }
            else
            {
                PauseGame();
            }
            _inputs.pause = false;
        }
        
    }

    public void PLayGame()
    {
        Time.timeScale = 1.0f;
        uiScreen.SetActive(false);
        isPaused = false;
        _player.GetComponent<ThirdPersonController>().enabled = true;
        //Cursor.visible = false;
        _player.GetComponent<ShooterController>().enabled = true;
        Cursor.lockState = previousCursorLockMode;
        _inputs.shoot = false;
    }


    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        uiScreen.SetActive(true);
        isPaused = true;
        _player.GetComponent<ThirdPersonController>().enabled = false;
        //Cursor.visible = true;
        _player.GetComponent<ShooterController>().enabled = false;
        previousCursorLockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        _inputs.shoot = false;

    }

    public void Sair()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MusicSlider()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.GetComponent<Slider>().value);
    }

    public void UpdateVolume()
    {
        _gameController.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }
}

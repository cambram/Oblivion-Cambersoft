using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Animator _fadeOutAnim;
    private GameManager _gameManager;
    private GameObject _environment;
    [SerializeField]
    private GameObject _pauseGame;
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _optionsMenu;
    [SerializeField]
    private GameObject _volumeMenu;
    [SerializeField]
    private GameObject _controlsMenu;
    [SerializeField]
    private AudioMixer _mainMixer;
    private bool _paused = false;
    [SerializeField]
    private Slider _masterSlider;
    [SerializeField]
    private Slider _environmentSlider;
    [SerializeField]
    private Slider _soundFXSlider;
    [SerializeField]
    private Slider _musicSlider;
    [SerializeField]
    private GameObject _feedbackVolume;

    private WaitForSeconds _delay;

    void Start(){
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _environment = GameObject.Find("Environment");
        SetVolumeSliders();
        _fadeOutAnim = GetComponent<Animator>();
        _pauseMenu.SetActive(false);
        _pauseGame.SetActive(false);
        _optionsMenu.SetActive(false);
        _controlsMenu.SetActive(false);
        _pauseGame.SetActive(false);
        _delay = new WaitForSeconds(1);
    }

    public void SetVolumeSliders() {
        float currentVolume;
        _mainMixer.GetFloat("masterVolume", out currentVolume);
        _masterSlider.value = Mathf.Pow(10, currentVolume/20);
        _mainMixer.GetFloat("environmentVolume", out currentVolume);
        _environmentSlider.value = Mathf.Pow(10, currentVolume / 20);
        _mainMixer.GetFloat("soundFXVolume", out currentVolume);
        _soundFXSlider.value = Mathf.Pow(10, currentVolume / 20);
        _mainMixer.GetFloat("musicVolume", out currentVolume);
        _musicSlider.value = Mathf.Pow(10, currentVolume / 20);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if (!_paused) {
                Pause();
            } else {
                Resume();
            }
        }
    }

    private void Pause() {
        _paused = !_paused;
        Cursor.visible = true;
        Time.timeScale = 0f;
        _pauseGame.SetActive(true);
        _pauseMenu.SetActive(true);
    }

    public void Resume() {
        _paused = !_paused;
        Cursor.visible = false;
        Time.timeScale = 1f;
        _pauseGame.SetActive(false);
    }

    public void Options() {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
        _volumeMenu.SetActive(false);
        _controlsMenu.SetActive(false);
    }

    public void VolumeMenu() {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        _volumeMenu.SetActive(true);
        _controlsMenu.SetActive(false);
    }

    public void ControlsMenu() {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        _volumeMenu.SetActive(false);
        _controlsMenu.SetActive(true);
    }
    public void BackFromControlsMenu() {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
        _volumeMenu.SetActive(false);
        _controlsMenu.SetActive(false);
    }

    public void BackFromOptions() {
        _pauseMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        _volumeMenu.SetActive(false);
        _controlsMenu.SetActive(false);
    }

    public void BackFromVolume() {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
        _volumeMenu.SetActive(false);
        _controlsMenu.SetActive(false);
    }

    public void SetMasterVolume(float volume) {
        _mainMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetEnvironmentVolume(float volume) {
        _mainMixer.SetFloat("environmentVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSoundFXVolume(float volume) {
        _mainMixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume) {
        _mainMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }

    public bool GetIsPaused() {
        return _paused;
    }

    /// <summary>
    /// Fades out from the game
    /// </summary>
    /// <param name="x">1 = fade to main menu; 2 = fade to load tutorial; 3 = fade to load level 1</param>
    /// <param name="y">true = restart, false = respawn</param>
    public void FadeOut(int x, bool y) {
        switch (x) {
            case 1:
                _fadeOutAnim.SetTrigger("FadeOut");
                _environment.GetComponent<Animator>().SetTrigger("FadeOut");
                StartCoroutine(BackToMainRoutine());
                break;
            case 2:
                _fadeOutAnim.SetTrigger("FadeOut");
                _environment.GetComponent<Animator>().SetTrigger("FadeOut");
                StartCoroutine(LoadTutorialRoutine(y));
                break;
            case 3:
                _fadeOutAnim.SetTrigger("FadeOut");
                _environment.GetComponent<Animator>().SetTrigger("FadeOut");
                StartCoroutine(LoadLevelOneRoutine(y));
                break;
            case 4:
                _fadeOutAnim.SetTrigger("FadeOut");
                _environment.GetComponent<Animator>().SetTrigger("FadeOut");
                StartCoroutine(LoadLevelTwoRoutine(y));
                break;
            default: break;
        }
    }
    IEnumerator BackToMainRoutine() {
        yield return _delay;
        _gameManager.BackToMainMenu();
    }

    IEnumerator LoadTutorialRoutine(bool y) {
        yield return _delay;
        if (y) {
            _gameManager.RestartTutorial();
        } else {
            _gameManager.RespawnTutorial();
        }
    }

    IEnumerator LoadLevelOneRoutine(bool y) {
        yield return _delay;
        if (y) {
            _gameManager.RestartLevelOne();
        } else {
            _gameManager.RespawnLevelOne();
        }
    }

    IEnumerator LoadLevelTwoRoutine(bool y) {
        yield return _delay;
        if (y) {
            _gameManager.RestartLevelTwo();
        } else {
            _gameManager.RespawnLevelTwo();
        }
    }

    public void MouseUpMasterVolume() {
        _feedbackVolume.GetComponent<AudioSource>().volume = _masterSlider.value / 2f;
        _feedbackVolume.GetComponent<AudioSource>().Play();
    }

    public void MouseUpEnvironmentVolume() {
        _feedbackVolume.GetComponent<AudioSource>().volume = _environmentSlider.value / 2f;
        _feedbackVolume.GetComponent<AudioSource>().Play();
    }

    public void MouseUpSoundFXVolume() {
        _feedbackVolume.GetComponent<AudioSource>().volume = _soundFXSlider.value / 2f;
        _feedbackVolume.GetComponent<AudioSource>().Play();
    }

    public void MouseUpMusicVolume() {
        _feedbackVolume.GetComponent<AudioSource>().volume = _musicSlider.value / 2f;
        _feedbackVolume.GetComponent<AudioSource>().Play();
    }

    public void Fullscreen(bool fullscreen) {
        Screen.fullScreen = fullscreen;
    }
}

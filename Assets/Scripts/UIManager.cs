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
    private AudioMixer _mainMixer;
    private bool _paused = false;
    [SerializeField]
    private Slider _master;



    void Start(){
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _environment = GameObject.Find("Environment");
        SetVolumeSliders();
        _fadeOutAnim = GetComponent<Animator>();
        _pauseMenu.SetActive(false);
        _pauseGame.SetActive(false);
        _optionsMenu.SetActive(false);
    }

    public IEnumerator SetVolumeSliders() {
        yield return new WaitForEndOfFrame();
        float currentVolume;
        _mainMixer.GetFloat("masterVolume", out currentVolume);
        _master.value = currentVolume;
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
    }

    public void VolumeMenu() {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        _volumeMenu.SetActive(true);
    }

    public void BackFromOptions() {
        _pauseMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        _volumeMenu.SetActive(false);
    }

    public void BackFromVolume() {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
        _volumeMenu.SetActive(false);
    }

    public void SetMasterVolume(float volume) {
        _mainMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
    }

    public bool GetIsPaused() {
        return _paused;
    }

    //SceneManager.GetActiveScene().buildIndex

    /// <summary>
    /// Fades out from the game
    /// </summary>
    /// <param name="x">1 = fade to main menu; 2 = fade to load tutorial; 3 = fade to load level 1</param>
    public void FadeOut(int x) {
        switch (x) {
            case 1:
                _fadeOutAnim.SetTrigger("FadeOut");
                _environment.GetComponent<Animator>().SetTrigger("FadeOut");
                StartCoroutine(BackToMainRoutine());
                break;
            case 2:
                _fadeOutAnim.SetTrigger("FadeOut");
                _environment.GetComponent<Animator>().SetTrigger("FadeOut");
                StartCoroutine(LoadTutorialRoutine());
                break;
            case 3:
                _fadeOutAnim.SetTrigger("FadeOut");
                _environment.GetComponent<Animator>().SetTrigger("FadeOut");
                StartCoroutine(LoadLevelOneRoutine());
                break;
            default: break;
        }
    }
    IEnumerator BackToMainRoutine() {
        yield return new WaitForSeconds(1);
        _gameManager.BackToMainMenu();
    }

    IEnumerator LoadTutorialRoutine() {
        yield return new WaitForSeconds(1);
        _gameManager.RestartTutorial();
    }

    IEnumerator LoadLevelOneRoutine() {
        yield return new WaitForSeconds(1);
        _gameManager.RestartLevelOne();
    }
}

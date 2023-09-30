using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _mainMenuButtons;
    [SerializeField]
    private GameObject _selectLevel;
    [SerializeField]
    private GameObject _optionsMenu;
    [SerializeField]
    private GameObject _volumeMenu;
    [SerializeField]
    private AudioMixer _mainMixer;
    [SerializeField]
    private Slider _masterSlider;
    [SerializeField]
    private Slider _environmentSlider;
    [SerializeField]
    private Slider _soundFXSlider;
    [SerializeField]
    private Slider _musicSlider;

    public void Start() {
        _mainMenuButtons.SetActive(true);
        _selectLevel.SetActive(false);
        _optionsMenu.SetActive(false);
        _volumeMenu.SetActive(false);
        SetVolumeSliders();
        Cursor.visible = true;
    }

    public void StartTutorial() {
        _animator.SetTrigger("FadeOut");
        StartCoroutine(StartGameRoutine(1));
    }

    public void StartLevelOne() {
        _animator.SetTrigger("FadeOut");
        StartCoroutine(StartGameRoutine(2));
    }

    public void StartLevelTwo() {
        _animator.SetTrigger("FadeOut");
        StartCoroutine(StartGameRoutine(3));
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SelectLevelButton() {
        _mainMenuButtons.SetActive(false);
        _selectLevel.SetActive(true);
        _optionsMenu.SetActive(false);
        _volumeMenu.SetActive(false);
    }

    public void BackButton() {
        _mainMenuButtons.SetActive(true);
        _selectLevel.SetActive(false);
        _optionsMenu.SetActive(false);
        _volumeMenu.SetActive(false);
    }

    public void BackFromVolumeButton() {
        _mainMenuButtons.SetActive(false);
        _selectLevel.SetActive(false);
        _optionsMenu.SetActive(true);
        _volumeMenu.SetActive(false);
    }

    public void OptionsButton() {
        _mainMenuButtons.SetActive(false);
        _selectLevel.SetActive(false);
        _optionsMenu.SetActive(true);
        _volumeMenu.SetActive(false);
    }

    public void VolumeButton() {
        _mainMenuButtons.SetActive(false);
        _selectLevel.SetActive(false);
        _optionsMenu.SetActive(false);
        _volumeMenu.SetActive(true);
    }

    IEnumerator StartGameRoutine(int level) {
        yield return new WaitForSeconds(4.5f);
        SceneManager.LoadScene(level);
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

    public void SetVolumeSliders() {
        float currentVolume;
        _mainMixer.GetFloat("masterVolume", out currentVolume);
        _masterSlider.value = Mathf.Pow(10, currentVolume / 20);
        _mainMixer.GetFloat("environmentVolume", out currentVolume);
        _environmentSlider.value = Mathf.Pow(10, currentVolume / 20);
        _mainMixer.GetFloat("soundFXVolume", out currentVolume);
        _soundFXSlider.value = Mathf.Pow(10, currentVolume / 20);
        _mainMixer.GetFloat("musicVolume", out currentVolume);
        _musicSlider.value = Mathf.Pow(10, currentVolume / 20);
    }
}

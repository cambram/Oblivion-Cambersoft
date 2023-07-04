using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _initialInstructionText;
    [SerializeField]
    private Text _controlsText;
    private bool _isControlsTextOn;
    [SerializeField]
    private Text _deathText;
    private bool _isDead;

    [SerializeField]
    private Sprite [] _batterySprites;
    [SerializeField]
    private Image _batteryImage;

    private GameManager _gameManager;

    void Start()
    {
        _initialInstructionText.gameObject.SetActive(true);
        _controlsText.gameObject.SetActive(false);
        _deathText.gameObject.SetActive(false);
        _batteryImage.sprite = _batterySprites[4];
        _isDead = false;
        _isControlsTextOn = false;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        StartCoroutine(InitialInstructionTextRoutine());
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Backspace) && !_isControlsTextOn) {
            _controlsText.gameObject.SetActive(true);
            _isControlsTextOn = true;
        } else if(Input.GetKeyDown(KeyCode.Backspace) && _isControlsTextOn) {
            _controlsText.gameObject.SetActive(false);
            _isControlsTextOn = false;
        }

        if(_isDead && Input.GetKeyDown(KeyCode.R)) {
            _gameManager.RestartGame();
        }
    }

    IEnumerator InitialInstructionTextRoutine() {
        yield return new WaitForSeconds(4f);
        _initialInstructionText.gameObject.SetActive(false);
    }

    public void DisplayDeath() {
        _deathText.gameObject.SetActive(true);
        _isDead = true;
    }

    /// <summary>
    /// Updates the battery UI element.
    /// </summary>
    /// <param name="percent">Index for battery sprite. 0 = 0%; 1 = 25%; 2 = 50%; 3 = 75%; 4 = 100%</param>
    public void UpdateBattery(int percent) {
        _batteryImage.sprite = _batterySprites[percent];
    }
}

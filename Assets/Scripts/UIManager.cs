using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _deathText;
    private bool _isDead;

    private GameManager _gameManager;

    void Start()
    {
        _deathText.gameObject.SetActive(false);
        _isDead = false;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    private void Update() {
        if(_isDead && Input.GetKeyDown(KeyCode.R)) {
            _gameManager.RestartGame();
        }
    }

    public void DisplayDeath() {
        _deathText.gameObject.SetActive(true);
        _isDead = true;
    }
}

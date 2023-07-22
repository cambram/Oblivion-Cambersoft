using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _A;
    [SerializeField]
    private GameObject _D;
    [SerializeField]
    private GameObject _E;
    [SerializeField]
    private GameObject _space;
    [SerializeField]
    private GameObject _rightClick;
    [SerializeField]
    private GameObject _leftClick;

    void Start() {
        _A.SetActive(false);
        _E.SetActive(false);
        _D.SetActive(false);
        _space.SetActive(false);
        _rightClick.SetActive(false);
        _leftClick.SetActive(false);
    }

    void Update() {
        
    }
}

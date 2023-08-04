using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private float _speed = 4f, _distance;

    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        if(_player != null) {
            _distance = Vector3.Distance(this.transform.position, _player.transform.position);
            Vector3 _direction = _player.transform.position - this.transform.position; 
            if (_distance < 22) {
                if (!_player.GetIsFlashlightActive()) { // if player flashlight is off, enemy approaches
                    _speed = 4f;
                    this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
                    _spriteRenderer.flipX = true;
                } else { // if player flashlight is on, enemy runs away...
                    if(_direction.x < 0 && _player.GetDirection()) { // ... only if the player is pointing the flashlight in the correct direction
                        _speed = 2f;
                        this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position + new Vector3(13, transform.position.y, 0), _speed * Time.deltaTime);
                        _spriteRenderer.flipX = true;
                    } else {
                        _speed = 4f;
                        _spriteRenderer.flipX = false;
                        this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
                    }
                }
                if (_distance < 13 && _player.GetIsFlashCameraActive()) { // change to 7
                    if (_direction.x < 0 && _player.GetDirection())
                        Destroy(this.gameObject);
                }
                if (_distance < 2) {
                    _player.KillPlayer();
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    private float coolDown = 50f;
    private float speed;
    private Vector3 pos;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    [Header("PowerUp data")]
    public float maxSpeed = 2f;
    public Vector2 initialPos;
    public float speedDecayFactor = 0.1f;
    public float frequency = 20f;
    public float magnitude = 0.5f;
    public Color color1;
    public Color color2;
    public Color color3;

    // Start is called before the first frame update
    void Start()
    {
        speed = maxSpeed;
        transform.position = initialPos * -1f;
        pos = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        StartCoroutine(flicker());
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            speed -= speedDecayFactor;
            pos += transform.right * Time.deltaTime * speed;
            transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
        }
        else
        {
            coolDown = 50f;
            speed = maxSpeed;
            transform.position = initialPos;
            pos = transform.position;
            _spriteRenderer.enabled = true;
            _collider.enabled = true;
        }
    }

    IEnumerator flicker()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        while (gameObject.activeInHierarchy)
        {
            spriteRenderer.color = color1;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = color2;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = color3;
            yield return new WaitForSeconds(0.2f);

        }
    }
}

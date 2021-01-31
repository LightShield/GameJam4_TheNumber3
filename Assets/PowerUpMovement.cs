using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    public float coolDown = 50f;
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
    public bool collected;

    [Header("Fade Settings")]
    public float cycleLength = 50f;
    public float tolerance = 1f;
    public float ttl = 10f;
    public float fadeInTime = 5;
    private Vector3 originalScale;
    private bool alreadyExists = false;
    

    // Start is called before the first frame update
    void Start()
    {
        //speed = maxSpeed;
        //transform.position = initialPos * -1f;
        //pos = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        originalScale = transform.localScale;
        collected = false;
        StartCoroutine(exist());

    }

    // Update is called once per frame
    void Update()
    {
        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            //speed -= speedDecayFactor;
            //pos += transform.right * Time.deltaTime * speed;
            //transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
        }
        else
        {
            if (!alreadyExists)
            {
                StartCoroutine(exist());
            }
            //speed = maxSpeed;
            //transform.position = initialPos;
            pos = transform.position;

        }
    }

    IEnumerator flicker()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("Started Flicker, collect = " + collected);
        while (!collected)
        {
            spriteRenderer.color = color1;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = color2;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = color3;
            yield return new WaitForSeconds(0.2f);

        }
        Debug.Log("Finished Flicker");
    }

    IEnumerator fadeIn()
    {
        transform.localScale = Vector3.zero;
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
        while ((transform.localScale.x < originalScale.x)&& !collected)
        {
            transform.localScale += originalScale * Time.deltaTime / fadeInTime;
            yield return null;
        }
        transform.localScale = originalScale;
        Debug.Log("Finished FadeIn");
    }

    IEnumerator fadeOut()
    {
        float countdown = ttl + tolerance;   
 
        while ((countdown > tolerance) && !collected)
        {
            transform.localScale = originalScale * countdown / (ttl + tolerance);
            countdown -= Time.deltaTime;
            yield return null;
        }


        Debug.Log("Finished FadeOut");
    }

    public IEnumerator exist()
    {
        alreadyExists = true;
        StartCoroutine(fadeIn());
        StartCoroutine(flicker());
        yield return new WaitForSeconds(fadeInTime);
        StartCoroutine(fadeOut());

        //reset power up
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        transform.localScale = originalScale;
        collected = false;
        alreadyExists = false;
        coolDown = cycleLength;
        yield return null;
    }
}

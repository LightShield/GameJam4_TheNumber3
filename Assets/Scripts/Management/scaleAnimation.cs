using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 edgeOfScreen = Camera.main.ViewportToWorldPoint(new Vector2(1f, 1f));
        Vector3 edgeOfAnim = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        float x = (edgeOfScreen.x / edgeOfAnim.x) + 0.2f;
        float y =(edgeOfScreen.y / edgeOfAnim.y) + 0.2f;
        Vector3 newScale = new Vector3(x, y, transform.localScale.z);
        transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

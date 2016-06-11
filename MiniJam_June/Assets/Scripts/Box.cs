using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Box : MonoBehaviour
{
    public SpriteRenderer sr;
    List<Box> touchedBoxes;

    BoxCollider2D bc;

    public bool placed = false;

    public bool movingToCenter;

    public float theta, radius;
    public float inc = 0.01f;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        touchedBoxes = new List<Box>();
        bc = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!placed)
        {
            transform.position = Center.controller.transform.position;
        }
    }
    public void SetColor(int color)
    {
        sr.color = GameController.controller.colors[color];
    }

    public void SetParams(float t, float r)
    {
        theta = t;
        radius = r;
    }

    public IEnumerator MoveToCenter()
    {
        Debug.Log("Moving to center");
        movingToCenter = true;
        while (movingToCenter && radius > 0)
        {
            transform.position = new Vector2(Mathf.Cos(theta) * radius, Mathf.Sin(theta) * radius);
            radius -= inc;
            yield return null;
        }
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        //rb2d.isKinematic = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!placed) return;
        Debug.Log(other.gameObject.tag);
        Center center = other.gameObject.GetComponentInParent<Center>();
        if (center != null)
        {
            center.radius += center.adjustRadius;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Box")
        {
            Debug.Log("Hit another box");
            movingToCenter = false;
        }
        if (!placed) return;

    }
}

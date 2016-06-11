using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Center : MonoBehaviour
{
    public static Center controller;
    public Vector2 centerPoint;
    public float radius = 1f;
    public float angle = 0;
    public float inc = 0.01f;
    public bool playingGame = true;
    public float adjustRadius = 2f;

    public List<Box> boxes;
    List<Vector2> usedPositions;

    Vector2 lastPosition;
    void Awake()
    {
        if (controller == null)
        {
            controller = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (controller != null)
        {
            Destroy(gameObject);
        }
        usedPositions = new List<Vector2>();
        centerPoint = transform.position;
        StartCoroutine(Move());
    }


    void Update()
    {

    }

    IEnumerator Move()
    {
        while (playingGame)
        {
            angle += inc;
            if (angle >= Mathf.PI * 2) angle = 0;
            Vector2 pos = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            transform.position = pos;
            yield return null;
        }
        yield return null;
    }

    public void PlaceBox(Box box)
    {
        float radius = this.radius;
        Vector2 position = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);

        box.placed = true;
        AudioController.controller.PlaySound(SoundType.Place);

        //box.transform.position = position;
        box.SetParams(angle, radius);
        StartCoroutine(box.MoveToCenter());
        usedPositions.Add(position);
        lastPosition = position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShifterHandle : MonoBehaviour
{
    private int currentGear = 0;
    private Vector2 currentPosition = new Vector2(0, 0);
    private Vector2 previousPosition = new Vector2(0, 0);
    private float moveFactor = 1; // 0-1

    private Transform handleTransform;

    private void Start()
    {
        handleTransform = transform.Find("Handle");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveFactor = moveFactor + Time.deltaTime * 6;
        if (moveFactor >= 1) moveFactor = 1;
        handleTransform.localPosition = Vector2.Lerp(previousPosition, currentPosition, moveFactor);
    }

    public void Shift(int gear)
    {
        if (gear == currentGear) return;
        currentGear = gear;
        previousPosition = new Vector2(currentPosition.x, currentPosition.y);
        if (gear == 0) currentPosition = new Vector2(0, 0);
        else if (gear == 1) currentPosition = new Vector2(-23, 38);
        else if (gear == 2) currentPosition = new Vector2(-23, -38);
        else if (gear == 3) currentPosition = new Vector2(0, 38);
        else if (gear == 4) currentPosition = new Vector2(0, -38);
        else if (gear == 5) currentPosition = new Vector2(24, 38);
        else if (gear == 6) currentPosition = new Vector2(24, -38);
        moveFactor = 0;
    }
}

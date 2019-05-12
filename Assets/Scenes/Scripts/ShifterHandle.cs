using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShifterHandle : MonoBehaviour
{
    private int currentGear = 0;
    private Vector2 currentPosition = new Vector2(0, 0);
    private Vector2 previousPosition = new Vector2(0, 0);
    private float moveFactor = 1; // 0-1

    // Update is called once per frame
    void FixedUpdate()
    {
        moveFactor = moveFactor + Time.deltaTime * 6;
        if (moveFactor >= 1) moveFactor = 1;
        transform.localPosition = Vector2.Lerp(previousPosition, currentPosition, moveFactor);
        
    }

    public void Shift(int gear)
    {
        if (gear == currentGear) return;
        currentGear = gear;
        previousPosition = new Vector3(currentPosition.x, currentPosition.y);
        if (gear == 0) currentPosition = new Vector3(0, 0, 0);
        else if (gear == 1) currentPosition = new Vector3(-23, 38, 0);
        else if (gear == 2) currentPosition = new Vector3(-23, -38, 0);
        else if (gear == 3) currentPosition = new Vector3(0, 38, 0);
        else if (gear == 4) currentPosition = new Vector3(0, -38, 0);
        else if (gear == 5) currentPosition = new Vector3(23, 38, 0);
        else if (gear == 6) currentPosition = new Vector3(23, -38, 0);
        moveFactor = 0;
    }
}

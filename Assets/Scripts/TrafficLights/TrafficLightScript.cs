using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrafficLightScript : MonoBehaviour
{

    //0 = red, 1 = yellow, 2 = green
    [SerializeField]
    private int state;

    //time that the traffic light stays yellow
    [SerializeField]
    private int yellowTime;

    private float timer;

    public bool flipSwitch;

    [SerializeField]
    private Image spr;

    private void Update()
    {
        if (flipSwitch)
        {
            flipSwitch = false;
            flipState();
        }
        if (state == 1)
        {
            if (timer >= yellowTime)
            {
                SetRed();
            }
            timer += Time.deltaTime;
        }
    }

    public Vector2 queryState()
    {
        return new Vector2(state, timer / yellowTime);
    }

    #region FLIPPINGLIGHTS
    public void flipState()
    {
        if (state == 1)
        {
            return;
        }
        if (state == 0)
        {
            SetGreen();
        }
        else if (state == 2)
        {
            timer = 0;
            SetYellow();
        }
    }

    void SetGreen()
    {
        state = 2;
        spr.color = Color.green;
    }

    void SetYellow()
    {
        state = 1;
        spr.color = Color.yellow;
    }

    void SetRed()
    {
        state = 0;
        spr.color = Color.red;
    }
    #endregion
}

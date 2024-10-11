using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Slider slideTimer;
    public float time = 30;
    public bool startTimer;

    // Start is called before the first frame update
    void Start()
    {
        slideTimer.maxValue = time;
        slideTimer.value = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer == true) 
        {
            time -= Time.deltaTime;
            slideTimer.value = time;
        }

        if (time <= 0)
            startTimer = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdozn : MonoBehaviour
{
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        Reset();
        SetText();
    }

    private void SetText() {
        text.text = time.ToString("mm':'ss':'ff");
    }

    public void Reset() {
        time = new TimeSpan(0, 20, 0);
    }

    private TimeSpan time;

    // Update is called once per frame
    void Update()
    {
        time = time.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
        SetText();
    }
}

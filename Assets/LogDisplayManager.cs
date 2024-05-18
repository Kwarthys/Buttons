using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogDisplayManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshPro display;
    [SerializeField]
    private float displayTime = 10f;

    private List<LogElement> logs = new List<LogElement>();

    private bool needsUpdate = false;

    public static LogDisplayManager instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(logs.Count == 0)
            return;

        for(int i = logs.Count - 1; i >= 0; --i)
        {
            if(logs[i].realTime + displayTime < Time.realtimeSinceStartup)
            {
                logs.RemoveAt(i);
                needsUpdate = true;
            }
        }

        if(needsUpdate)
            rebuildString();
    }

    public void log(string message)
    {
        logs.Add(new LogElement(Time.realtimeSinceStartup, message));
        needsUpdate = true;
    }

    private void rebuildString()
    {
        display.text = "";
        foreach(LogElement log in logs)
            display.text += log.message + "\n";
    }

    struct LogElement
    {
        public float realTime;
        public string message;

        public LogElement(float time, string text) { realTime = time; message = text; }
    }
}

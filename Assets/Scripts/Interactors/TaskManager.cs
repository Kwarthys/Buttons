using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    //Singleton
    public static TaskManager instance = null;
    private void Awake() { TaskManager.instance = this; }

    [SerializeField]
    private TMPro.TextMeshPro prompts;
    [SerializeField]
    private int maxActiveTask = 1;
    [SerializeField]
    private List<BaseInstrument> activeInstruments = new List<BaseInstrument>();

    private BaseInstrument[] allInstruments;

    private void Start()
    {
        allInstruments = GameObject.FindObjectsOfType<BaseInstrument>();
    }

    private void Update()
    {
        //Check number of activeTasks
        if(activeInstruments.Count >= maxActiveTask || activeInstruments.Count == allInstruments.Length)
            return;

        //make an instrument generate a task:
        //-find a random instrument
        BaseInstrument chosenInstrument = null;
        while(chosenInstrument == null)
        {
            int id = (int)(Random.value * allInstruments.Length);
            chosenInstrument = allInstruments[id];
            if(activeInstruments.Contains(chosenInstrument))
                chosenInstrument = null;
        }
        //make it generate a task
        chosenInstrument.generateNewTask();
        //-retrieve and display the prompt
        activeInstruments.Add(chosenInstrument);
        updatePrompts();
    }

    public void notifyTaskComplete(BaseInstrument instrument)
    {
        //happy
        if(activeInstruments.Contains(instrument))
        {
            Debug.Log("GGs");
            activeInstruments.Remove(instrument);
            updatePrompts();
        }
        else
        {
            Debug.LogWarning(instrument + " notified task complete while not being active");
        }
    }

    private void updatePrompts()
    {
        prompts.text = "";
        bool first = true;
        foreach(BaseInstrument i in activeInstruments)
        {
            if(first)
                first = false;
            else
                prompts.text += "\n";
            prompts.text += i.getPrompt();
        }
    }
}

public class Task<T>
{
    public string prompt;
    public T targetValue;
}

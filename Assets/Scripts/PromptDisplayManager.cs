using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptDisplayManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshPro promptDisplay;

    private List<string> prompts = new();
    private List<int> IDs = new();

    private bool needsUpdate = false;

    private void Update()
    {
        if(needsUpdate == false)
            return;
        
        updateDisplay();
        if(prompts.Count != IDs.Count)
            LogDisplayManager.instance.log("PROMPT_MANAGER : prompts count (" + prompts.Count + ") != than IDs count(" + IDs.Count + ")");
    }

    public void updateDisplay()
    {
        promptDisplay.text = "";
        foreach(string p in prompts)
            promptDisplay.text += p + "\n";
        needsUpdate = false;
    }

    public void addPrompt(int ID, string prompt)
    {
        IDs.Add(ID);
        prompts.Add(prompt);

        needsUpdate = true;
    }

    public void removePrompt(int ID)
    {
        int toRemove = IDs.IndexOf(ID);
        IDs.RemoveAt(toRemove);
        prompts.RemoveAt(toRemove);

        needsUpdate = true;
    }
}

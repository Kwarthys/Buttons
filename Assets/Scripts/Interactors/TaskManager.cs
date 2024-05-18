using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class TaskManager : MonoBehaviour
{
    //Singleton
    public static TaskManager instance = null;
    private void Awake() { TaskManager.instance = this; }

    [SerializeField]
    private int maxActiveTask = 1;
    [SerializeField]
    private List<int> activeInstruments = new List<int>();
    [SerializeField]
    private InstrumentGenerator nameGenerator;

    private Dictionary<NetworkConnectionToClient, int[]> instrumentsByClient = new Dictionary<NetworkConnectionToClient, int[]>();

    private Dictionary<int, string> promptsByUID = new Dictionary<int, string>();

    private int UIDCounter = 1;
    private int getNextUID() { return UIDCounter++; }

    private void Update()
    {
        //Check number of activeTasks
        if(activeInstruments.Count >= maxActiveTask * instrumentsByClient.Keys.Count)
            return;

        //make an instrument generate a task:
        //-find a random instrument of a random player

        List<NetworkConnectionToClient> players = Enumerable.ToList(instrumentsByClient.Keys);
        NetworkConnectionToClient player = players[(int)(Random.value * instrumentsByClient.Keys.Count)];

        int chosenInstrument = -1;
        while(chosenInstrument == -1)
        {
            int index = (int)(Random.value * instrumentsByClient[player].Length);
            chosenInstrument = instrumentsByClient[player][index];
            if(activeInstruments.Contains(chosenInstrument))
            {
                player = players[(int)(Random.value * instrumentsByClient.Keys.Count)];
                chosenInstrument = -1;
            }
        }

        //make it generate a task through network
        Communicator.instance.TargetAskCreateTask(player, chosenInstrument);
        //will retrieve and display the prompt later on a random client in 'onPromptCreated'
    }

    public void notifyTaskComplete(int UID)
    {
        //happy
        if(activeInstruments.Contains(UID))
        {
            LogDisplayManager.instance.log("GGs");
            activeInstruments.Remove(UID);
        }
        else
        {
            LogDisplayManager.instance.log(UID + " notified task complete while not being active");
        }
    }

    public void onPromptCreated(int UID, string prompt)
    {
        LogDisplayManager.instance.log(UID + " created prompt " + prompt);
        //then send prompt to random player
    }

    public void onClientConnects(NetworkConnectionToClient client)
    {
        //temp fixed 3 instruments (button knob slider)
        int[] clientUIDs = { getNextUID(), getNextUID(), getNextUID() };
        instrumentsByClient.Add(client, clientUIDs);

        List<string> names = nameGenerator.generateInstruments(3, 1);

        Debug.Log("onClientConnects " + client);
        LogDisplayManager.instance.log("onClientConnects " + client);
        Communicator.instance.TargetAskInstrumentCreation(client, names.ToArray(), clientUIDs);

        Communicator.instance.sayHello();
    }

    public void onClientDisconnects(NetworkConnectionToClient client)
    {
        //TODO : unplug existing active tasks

        instrumentsByClient.Remove(client);
    }
}

public class Task<T>
{
    public string prompt;
    public T targetValue;
}

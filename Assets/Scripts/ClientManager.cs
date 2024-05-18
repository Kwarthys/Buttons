using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public static ClientManager instance { get; private set; }
    public void Awake() { instance = this; }

    public BaseInstrument buttonInstrument;
    public BaseInstrument knobInstrument;
    public BaseInstrument sliderInstrument; //temporary fixed instruments

    public Dictionary<int, BaseInstrument> instruments = new Dictionary<int, BaseInstrument>();

    public void instantiateInstruments(string[] instrumentNames, int[] UIDS)
    {
        //should create instrument, for now it just initializes existing ones
        if(instrumentNames.Length < 3 || UIDS.Length < 3)
            return;

        buttonInstrument.setName(instrumentNames[0]);
        knobInstrument.setName(instrumentNames[1]);
        sliderInstrument.setName(instrumentNames[2]);

        instruments.Add(UIDS[0], buttonInstrument);
        instruments.Add(UIDS[1], knobInstrument);
        instruments.Add(UIDS[2], sliderInstrument);
    }

    public bool askCreateTaskForInstrument(int UID, out string prompt)
    {
        prompt = "";

        if(!instruments.ContainsKey(UID))
        {
            LogDisplayManager.instance.log("tried to acces non existing instrument " + UID.ToString());
            return false;
        }

        //send prompt back to manager
        prompt = instruments[UID].generateNewTask();
        return true;
    }

    public void notifyTaskComplete(BaseInstrument instrument)
    {
        int UID = findUIDOfInstrument(instrument);
        if(UID == -1)
        {
            LogDisplayManager.instance.log("non registered instrument finished task " + instrument.name);
            return;
        }

        //notifyserver with UID
        Communicator.instance.notifyServerInstrumentTaskComplete(UID);
    }

    private int findUIDOfInstrument(BaseInstrument i)
    {
        foreach(int UID in instruments.Keys)
        {
            if(instruments[UID] == i)
                return UID;
        }

        return -1;
    }
}

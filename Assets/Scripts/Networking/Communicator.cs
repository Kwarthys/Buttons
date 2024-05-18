using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Communicator : NetworkBehaviour
{
    public static Communicator instance;
    public void Awake() { instance = this; }

    public void notifyServerInstrumentTaskComplete(int instrumentUID)
    {
        CmdNotifyTaskComplete(instrumentUID);
    }

    [Command(requiresAuthority = false)]
    public void CmdNotifyTaskComplete(int instrumentID)
    {
        //TaskManager only exists server-side
        LogDisplayManager.instance.log(instrumentID + " notified task compeleted");
        TaskManager.instance.notifyTaskComplete(instrumentID);
    }

    [TargetRpc]
    public void TargetAskCreateTask(NetworkConnectionToClient target, int instrumentID)
    {
        if(ClientManager.instance.askCreateTaskForInstrument(instrumentID, out string prompt))
        {
            CmdTaskPromptCreated(instrumentID, prompt);
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdTaskPromptCreated(int UID, string prompt)
    {
        LogDisplayManager.instance.log(UID + ": prompt created : " + prompt);
    }

    [TargetRpc]
    public void TargetAskInstrumentCreation(NetworkConnectionToClient target, string[] names, int[] UIDS)
    {
        Debug.Log("TargetAskInstrumentCreation");
        LogDisplayManager.instance.log("TargetAskInstrumentCreation");
        ClientManager.instance.instantiateInstruments(names, UIDS);
    }


    //debugging hard
    [ClientRpc]
    public void RpcSayHello()
    {
        LogDisplayManager.instance.log("RpcSayHello");
        Debug.Log("RpcSayHello");
    }

    public void sayHello()
    {
        Debug.Log("sayHello");
        RpcSayHello();
    }
}

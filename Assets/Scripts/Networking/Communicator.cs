using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Communicator : MonoBehaviour
{
    public static Communicator instance;
    public void Awake() { instance = this; setupMessageCallbacks(); }

    public void notifyServerInstrumentTaskComplete(int instrumentUID)
    {
        CmdNotifyTaskComplete(instrumentUID);       
    }

    
    public void CmdNotifyTaskComplete(int instrumentID)
    {
        //TaskManager only exists server-side
        TaskCompleteMessage message = new() { IUID = instrumentID };
        NetworkClient.Send(message);
    }

    
    public void TargetAskCreateTask(NetworkConnectionToClient target, int instrumentID)
    {
        AskCreateTaskMessage message = new() { IUID = instrumentID };
        target.Send(message);
    }

    
    public void CmdTaskPromptCreated(int UID, string prompt)
    {
        TaskPromptCreatedMessage message = new() { IUID = UID, prompt = prompt };
        NetworkClient.Send(message);
    }

    
    public void TargetAskInstrumentCreation(NetworkConnectionToClient target, string[] names, int[] UIDS)
    {
        AskInstrumentCreationMessage message = new() { names = names, IUIDS = UIDS };
        target.Send(message);
    }

    public void TargetDisplayPrompt(NetworkConnectionToClient target, string prompt, int promptUID)
    {
        DisplayPromptMessage message = new() { UID = promptUID, prompt = prompt };
        target.Send(message);
    }

    public struct AskCreateTaskMessage : NetworkMessage
    {
        public int IUID;
    }

    public void onAskCreateTaskMessage(AskCreateTaskMessage message)
    {
        if(ClientManager.instance.askCreateTaskForInstrument(message.IUID, out string prompt))
        {
            CmdTaskPromptCreated(message.IUID, prompt);
        }
    }

    public struct TaskCompleteMessage : NetworkMessage
    {
        public int IUID;
    }

    public void onTaskCompleteMessage(NetworkConnectionToClient client, TaskCompleteMessage message)
    {
        TaskManager.instance.notifyTaskComplete(message.IUID);
    }

    public struct TaskPromptCreatedMessage : NetworkMessage
    {
        public int IUID;
        public string prompt;
    }

    public void onPromptCreatedMessage(NetworkConnectionToClient client, TaskPromptCreatedMessage message)
    {
        TaskManager.instance.onPromptCreated(message.IUID, message.prompt);
    }

    public struct AskInstrumentCreationMessage : NetworkMessage
    {
        public string[] names;
        public int[] IUIDS;
    }
    public void onInstrumentCreationMessage(AskInstrumentCreationMessage message)
    {
        LogDisplayManager.instance.log("TargetAskInstrumentCreation");
        ClientManager.instance.instantiateInstruments(message.names, message.IUIDS);
    }

    public struct DisplayPromptMessage : NetworkMessage
    {
        public int UID;//unique ID of the prompt, not of any instrument
        public string prompt;
    }

    public void onDisplayPromptMessage(DisplayPromptMessage message)
    {
        // TODO
    }

    public void setupMessageCallbacks()
    {
        NetworkClient.RegisterHandler<AskInstrumentCreationMessage>(onInstrumentCreationMessage);
        NetworkClient.RegisterHandler<AskCreateTaskMessage>(onAskCreateTaskMessage);
        NetworkClient.RegisterHandler<DisplayPromptMessage>(onDisplayPromptMessage);


        NetworkServer.RegisterHandler<TaskPromptCreatedMessage>(onPromptCreatedMessage);
        NetworkServer.RegisterHandler<TaskCompleteMessage>(onTaskCompleteMessage);
    }
}

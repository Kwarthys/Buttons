using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameNetworkManager : NetworkManager
{

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        //Called on the server when new client connects

        //New clients join
        //-generate instruments
        //-send instruments
        //increase number of concurrent tasks (should trigger tasks creation)
        TaskManager.instance.onClientConnects(conn);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        TaskManager.instance.onClientDisconnects(conn);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        //create servermanager
        //create communicator
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        //create ClientManager
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ENet;

public class Client : MonoBehaviour {
    private Host _client;
    private Peer _peer;

    private void Start() {
        Application.runInBackground = true;
        InitENet();
    }

    private void FixedUpdate() {
        UpdateENet();
    }

    private void OnDestroy() {
        _client.Dispose();
        ENet.Library.Deinitialize();
    }

    private void InitENet() {
        const string ip = "207.148.6.176";
        const ushort port = 35000;
        ENet.Library.Initialize();
        _client = new Host();
        Address address = new Address();

        address.SetHost(ip);
        address.Port = port;
        _client.Create();
        Debug.Log("Connecting");
        _peer = _client.Connect(address);
    }

    private void UpdateENet() {
        ENet.Event netEvent;

        if(_client.CheckEvents(out netEvent) <= 0) {
            if(_client.Service(15, out netEvent) <= 0)
                return;
        }

        switch(netEvent.Type) {
            case ENet.EventType.None:
                break;
            case ENet.EventType.Connect:
                Debug.Log("Client connected to server - ID: " + _peer.ID);
                break;
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

// Main entrypoint class to put in your scene.
namespace Hotel {

  public class HotelClient : MonoBehaviour {
    public string masterServerUrl;
    public string gameId;

    private ApiClient client;
    private TaskCompletionSource<bool> initializedCompletionSource = new TaskCompletionSource<bool>();

    public async Task<bool> WaitUntilInitialized() {
      return await initializedCompletionSource.Task;
    }

    public async Task<bool> HostServer(string name, int port, int maxPlayers) {
      // TODO: Figure out IP echo
      var server = new GameServer();
      server.name = name;
      server.host = "localhost";
      server.port = port;
      server.maxPlayers = maxPlayers;
      server.numPlayers = 0;
      server.gameId = gameId;
      await client.CreateServer(server);
      return true;
    }

    public async Task<GameServer[]> ListServers() {
      return await client.ListServers(gameId);
    }

    async void Start() {
      client = new ApiClient(masterServerUrl);
      await client.Initialize();
      Debug.Log("Hotel client initialized.");
      initializedCompletionSource.SetResult(true);
    }

    void Awake() {
      DontDestroyOnLoad(gameObject);
    }

    private static HotelClient _instance;

    public static HotelClient Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<HotelClient>();
        }
        return _instance;
      }
    }
  }

} // namespace Hotel
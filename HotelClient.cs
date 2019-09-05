using System.Collections;
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
    public int masterServerPingIntervalSeconds = 15;

    private ApiClient client;
    private TaskCompletionSource<bool> initializedCompletionSource = new TaskCompletionSource<bool>();

    public async Task<bool> WaitUntilInitialized() {
      return await initializedCompletionSource.Task;
    }

    public async Task<RegisteredGameServer> HostServer(string name, int port, int maxPlayers) {
      // TODO: Figure out IP echo
      var gameServer = new GameServer();
      gameServer.name = name;
      gameServer.host = "localhost";
      gameServer.port = port;
      gameServer.maxPlayers = maxPlayers;
      gameServer.numPlayers = 0;
      gameServer.gameId = gameId;
      var newServer = await client.CreateServer(gameServer);
      return new RegisteredGameServer(client, newServer, masterServerPingIntervalSeconds);
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

/// Main entrypoint class to put in your scene.
public class HotelClient : MonoBehaviour
{
  public string masterServerUrl;
  public string gameId;
  
  [Range(1, 60)]
  public int alivePingIntervalSeconds = 20;

  private ApiClient client;
  private TaskCompletionSource<bool> initializedCompletionSource = new TaskCompletionSource<bool>();
  private HostedGameServer activeHostedServer;

  /// Returns true once the hotel client is fully initialized.
  public async Task<bool> WaitUntilInitialized() {
    return await initializedCompletionSource.Task;
  }

  /// Informs the master server that this instance of unity is acting as a server host for this game.
  public async Task<HostedGameServer> StartHostingServer(string host, int port, int maxPlayers, string serverName) {
    var gameServerData = new Api.GameServer();
    gameServerData.host = host;
    gameServerData.port = port;
    gameServerData.name = serverName;
    gameServerData.maxPlayers = maxPlayers;
    gameServerData.numPlayers = 0;
    gameServerData.gameId = gameId;
    gameServerData = await client.CreateServer(gameServerData);
    activeHostedServer = new HostedGameServer(gameServerData, client, alivePingIntervalSeconds);
    return activeHostedServer;
  }

  /// Host the server with an implicit hostname based on public IP.
  public async Task<HostedGameServer> StartHostingServer(int port, int maxPlayers, string serverName) {
    return await StartHostingServer(null, port, maxPlayers, serverName);
  }

  /// Lists the currently running servers for this game.
  public async Task<Api.GameServer[]> ListGameServers() {
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
        _instance = GameObject.FindObjectOfType<HotelClient>();
      }
      return _instance;
    }
  }
}

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
  public async Task<HostedGameServer> HostServer(string name, int port, int maxPlayers) {
    var gameServerData = new Api.GameServer();
    gameServerData.name = name;
    gameServerData.port = port;
    gameServerData.maxPlayers = maxPlayers;
    gameServerData.numPlayers = 0;
    gameServerData.gameId = gameId;
    gameServerData = await client.CreateServer(gameServerData);
    activeHostedServer =  new HostedGameServer(gameServerData, client, alivePingIntervalSeconds);
    return activeHostedServer;
  }

  /// Lists the currently running servers for this game.
  public async Task<Api.GameServer[]> ListServers() {
    return await client.ListServers(gameId);
  }

  async void Start()
  {
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

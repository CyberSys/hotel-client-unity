using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace Hotel {

  /// Main entrypoint class to put in your scene.
  public class HotelClient : MonoBehaviour {
    public string masterServerUrl;
    public string gameId;

    [Range(1, 60)]
    public int alivePingIntervalSeconds = 20;

    private const string HOTEL_MASTER_ADDRESS_FLAG = "--hotel_master_address";

    private ApiClient client;
    private TaskCompletionSource<bool> initializedCompletionSource = new TaskCompletionSource<bool>();
    private RegisteredGameServer activeHostedServer;

    /// Returns true once the hotel client is fully initialized.
    public async Task<bool> WaitUntilInitialized() {
      return await initializedCompletionSource.Task;
    }

    /// Informs the master server that this instance of unity is acting as a server host for this game.
    public async Task<RegisteredGameServer> StartHostingServer(string host, int port, int maxPlayers, string serverName) {
      var gameServerData = new GameServer();
      gameServerData.host = host;
      gameServerData.port = port;
      gameServerData.name = serverName;
      gameServerData.maxPlayers = maxPlayers;
      gameServerData.numPlayers = 0;
      gameServerData.gameId = gameId;
      gameServerData = await client.CreateServer(gameServerData);
      activeHostedServer = new RegisteredGameServer(client, gameServerData, alivePingIntervalSeconds);
      return activeHostedServer;
    }

    /// Host the server with an implicit hostname based on public IP.
    public async Task<RegisteredGameServer> StartHostingServer(int port, int maxPlayers, string serverName) {
      return await StartHostingServer(null, port, maxPlayers, serverName);
    }

    /// Lists the currently running servers for this game.
    public async Task<GameServer[]> ListGameServers() {
      return await client.ListServers(gameId);
    }

    /// Requests that a new server be spawned.
    public async Task<GameServer> RequestServerSpawn() {
      return await client.SpawnServer(gameId);
    }

    async void Start() {
      var masterUrl = ResolveMasterServerUrl();
      client = new ApiClient(masterUrl);
      Debug.Log($"Contacting hotel master server at {masterUrl}");
      await client.Initialize();
      Debug.Log("Hotel client initialized.");
      initializedCompletionSource.SetResult(true);
    }

    void Awake() {
      DontDestroyOnLoad(gameObject);
    }

    private string ResolveMasterServerUrl() {
      var flagUrl = Util.GetFlagValue(HOTEL_MASTER_ADDRESS_FLAG);
      if (string.IsNullOrEmpty(flagUrl)) {
        return masterServerUrl;
      }
      return flagUrl;
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

} // namespace Hotel

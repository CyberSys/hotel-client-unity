using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hotel {

  [RequireComponent(typeof(ServerNetworkController))]
  [RequireComponent(typeof(ClientNetworkController))]
  public class GameController : MonoBehaviour {
    public ServerListUI serverListUi;

    private ServerNetworkController serverNetController;
    private ClientNetworkController clientNetController;

    public async void HandleHostLocal() {
      Debug.Log("Hosting local server.");
      serverNetController.StartServer();
      var hostedServer = await HotelClient.Instance.StartHostingServer("localhost", serverNetController.GetServerPort(), 16, "Test");
      if (hostedServer != null) {
        SceneManager.LoadScene("Game");
        Debug.Log("Listening for connections");
      } else {
        Debug.LogError("Unable to host server.");
      }
    }

    public void HandleJoinServer(string host) {
      Debug.Log("Attempting to join server.");
      clientNetController.JoinServer(host, serverNetController.GetServerPort());
    }

    public void HandleSpawnRemote() {
      Debug.Log("Spawning remote server.");
    }

    public void HandleRefreshList() {
      Debug.Log("Refreshing server list.");
      RefreshServerList();
    }

    public void HandleQuit() {
      Application.Quit();
    }

    async void Start() {
      serverNetController = GetComponent<ServerNetworkController>();
      clientNetController = GetComponent<ClientNetworkController>();

      await HotelClient.Instance.WaitUntilInitialized();
      RefreshServerList();
    }

    void Awake() {
      DontDestroyOnLoad(gameObject);
    }

    async private void RefreshServerList() {
      var servers = await HotelClient.Instance.ListGameServers();
      serverListUi.DisplayServers(servers);
    }
  }

} // namespace Hotel

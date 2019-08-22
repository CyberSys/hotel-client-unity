using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleGameController : MonoBehaviour
{
  public ExampleServerListUI serverListUi;

  public async void HandleHostLocal() {
    Debug.Log("Hosting local server.");
    var hostedServer = await HotelClient.Instance.HostServer("Test", 3030, 16);
    if (hostedServer != null) {
      SceneManager.LoadScene("Game");
    } else {
      Debug.LogError("Unable to host server.");
    }
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
    await HotelClient.Instance.WaitUntilInitialized();
    RefreshServerList();
  }

  void Awake() {
    DontDestroyOnLoad(gameObject);
  }

  async private void RefreshServerList() {
    var servers = await HotelClient.Instance.ListServers();
    serverListUi.DisplayServers(servers);
  }
}

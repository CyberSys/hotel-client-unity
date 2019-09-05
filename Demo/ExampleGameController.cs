using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExampleGameController : MonoBehaviour
{
  public ExampleServerListUI serverListUi;

  public async void HandleHostLocal() {
    Debug.Log("Hosting local server.");
    var ok = await Hotel.HotelClient.Instance.HostServer("Test", 3030, 16);
    Debug.Log("Server registered.");
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
    await Hotel.HotelClient.Instance.WaitUntilInitialized();
    RefreshServerList();
  }

  async private void RefreshServerList() {
    var servers = await Hotel.HotelClient.Instance.ListServers();
    serverListUi.DisplayServers(servers);
  }
}

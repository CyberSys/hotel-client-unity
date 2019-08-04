using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleGameController : MonoBehaviour
{
  public ExampleServerListUI serverListUi;

  async void Start() {
    var hotel = HotelClient.Instance;
    await hotel.WaitUntilInitialized();
    Debug.Log("Hotel initialized!");
    await hotel.HostServer("test", 3030, 12);
    var servers = await hotel.ListServers();
    serverListUi.DisplayServers(servers);
  }
}

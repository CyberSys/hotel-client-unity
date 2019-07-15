using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class HotelClient : MonoBehaviour
{
  public string masterServerUrl;
  public string gameId;

  private ApiClient client;

  // Start is called before the first frame update
  async void Start()
  {
    client = new ApiClient(masterServerUrl);
    await client.Initialize();

    var testServer = new Api.Server();
    testServer.name = "a";
    testServer.host = "www.google.com";
    testServer.port = 2000;
    testServer.maxPlayers = 4;
    testServer.numPlayers = 2;
    testServer.gameId = gameId;
    await client.CreateServer(testServer);
    var result = await client.ListServers(gameId);
    Debug.Log(result.Length);
  }
}

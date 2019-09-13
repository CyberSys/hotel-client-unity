using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class ApiClient
{
  private JsonHttpClient client;
  private string baseHost;

  public ApiClient(string baseHost) {
    this.client = new JsonHttpClient(new System.Uri(baseHost));
    this.baseHost = baseHost;
  }

  public async Task Initialize() {
    var response = await client.Post<Api.IdentifyResponse>("/identify");
    if (response.token.Length < 1) {
      Debug.LogError("Failed to initialize Hotel session!");
    } else {
      client.SetSessionToken(response.token);
    }
  }

  public async Task<Api.GameServer[]> ListServers(string gameId) {
    var url = string.Format("/servers?gameId={0}", gameId);
    var response = await client.Get<Api.ListServersResponse>(url);
    return response.servers;
  }

  public async Task<Api.GameServer> GetServer(string serverId) {
    var url = string.Format("/servers/{0}", serverId);
    var response = await client.Get<Api.GameServer>(url);
    return response;
  }

  public async Task<Api.GameServer> CreateServer(Api.GameServer gameServerData) {
    var url = "/servers";
    var response = await client.Post<Api.GameServer>(url, gameServerData);
    return response;
  }

  public async Task<bool> UpdateServer(Api.GameServer gameServerData) {
    var url = string.Format("/servers/{0}", gameServerData.id);
    var response = await client.Put<Api.GameServer>(url, gameServerData);
    return true;
  }

  public async Task<bool> PingServerAlive(int gameServerId) {
    var url = string.Format("/servers/{0}", gameServerId);
    var response = await client.Put<Api.GameServer>(url, new Api.GameServer());
    return true;
  }
}

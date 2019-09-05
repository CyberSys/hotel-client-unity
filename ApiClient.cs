using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Hotel {

  internal class ApiClient {
    private JsonHttpClient client;
    private string baseHost;

    public ApiClient(string baseHost) {
      this.client = new JsonHttpClient(new System.Uri(baseHost));
      this.baseHost = baseHost;
    }

    public async Task Initialize() {
      var response = await client.Post<IdentifyResponse>("/identify");
      if (response.token.Length < 1) {
        Debug.LogError("Failed to initialize Hotel session!");
      } else {
        client.SetSessionToken(response.token);
      }
    }

    public async Task<GameServer[]> ListServers(string gameId) {
      var url = string.Format("/servers?gameId={0}", gameId);
      var response = await client.Get<ListServersResponse>(url);
      return response.servers;
    }

    public async Task<GameServer> GetServer(string serverId) {
      var url = string.Format("/servers/{0}", serverId);
      var response = await client.Get<GameServer>(url);
      return response;
    }

    public async Task<GameServer> CreateServer(GameServer server) {
      var url = "/servers";
      var response = await client.Post<GameServer>(url, server);
      return response;
    }

    public async Task<bool> UpdateServer(GameServer server) {
      var url = string.Format("/servers/{0}", server.id);
      var response = await client.Put<GameServer>(url, server);
      return true;
    }

    public async Task<bool> PingServer(GameServer server) {
      var url = string.Format("/servers/{0}/alive", server.id);
      var response = await client.Post<GameServer>(url);
      return true;
    }
  }

} // namespace Hotel
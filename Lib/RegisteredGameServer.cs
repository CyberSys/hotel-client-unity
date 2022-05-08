using System;
using System.Threading.Tasks;

namespace Hotel {

  /// A reference to an active game server which is registered with the hotel
  /// master server.
  public class RegisteredGameServer {
    private bool alive = true;
    private ApiClient apiClient;
    private GameServer gameServerData;
    private DateTime lastUpdateTimestamp = DateTime.MinValue;

    internal RegisteredGameServer(ApiClient apiClient, GameServer gameServerData, int pingIntervalSeconds) {
      this.apiClient = apiClient;
      this.gameServerData = gameServerData;
      lastUpdateTimestamp = DateTime.Now;

      StartUpdateLoop(pingIntervalSeconds);
    }

    public async void UpdateNumPlayers(int numPlayers) {
      gameServerData.numPlayers = numPlayers;
      await SendUpdate();
    }

    // Unregister the server. This instance should not be used after this.
    public async void Destroy() {
      await apiClient.DeleteServer(gameServerData);
      alive = false;
    }

    private async Task SendUpdate() {
      UnityEngine.Debug.Log("Sending master server ping");
      var ok = await apiClient.UpdateServer(gameServerData);
      if (ok) {
        lastUpdateTimestamp = DateTime.Now;
      }
    }

    private async void StartUpdateLoop(int pingIntervalSeconds) {
      while (alive) {
        if (DateTime.Now.Subtract(lastUpdateTimestamp).Seconds > pingIntervalSeconds) {
          await SendUpdate();
        }

        // Wait 2 seconds before checking again.
        await Task.Delay(2000);
      }
    }
  }

} // namespace Hotel

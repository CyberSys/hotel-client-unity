using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// A representation of a game server hosted by this unity instance which is registered with
/// the master server.
public class HostedGameServer
{
  private const int PING_LOOP_DELAY = 1000;

  private Api.GameServer gameServerData;
  private ApiClient apiClient;
  private int alivePingIntervalSeconds;
  private DateTime lastServerPingTime;

  public HostedGameServer(Api.GameServer gameServerData, ApiClient apiClient, int alivePingIntervalSeconds) {
    this.gameServerData = gameServerData;
    this.apiClient = apiClient;
    this.alivePingIntervalSeconds = alivePingIntervalSeconds;

    StartServerPingLoop();
  }

  private async void StartServerPingLoop() {
    while (true) {
      if (lastServerPingTime == null || DateTime.Now.Subtract(lastServerPingTime).Seconds >= alivePingIntervalSeconds) {
        apiClient.PingServerAlive(gameServerData.id);
        lastServerPingTime = DateTime.Now;
        Debug.Log("Pinging server alive!");
      }
      await Task.Delay(PING_LOOP_DELAY);
    }
  }
}

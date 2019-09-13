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
  private DateTime lastMasterServerUpdate;
  private bool active;

  public HostedGameServer(Api.GameServer gameServerData, ApiClient apiClient, int alivePingIntervalSeconds) {
    this.gameServerData = gameServerData;
    this.apiClient = apiClient;
    this.alivePingIntervalSeconds = alivePingIntervalSeconds;
    active = true;

    StartServerPingLoop();
  }

  public void SetNumPlayers(int numPlayers) {
    gameServerData.numPlayers = numPlayers;
    UpdateMasterServerData();
  }

  public void Shutdown() {
    // No need to add an explicit delete to the server for now, the reaper will cleanup servers that aren't pinging.
    active = false;
  }

  private async void UpdateMasterServerData() {
    await apiClient.UpdateServer(gameServerData);
    lastMasterServerUpdate = DateTime.Now;
  }

  private async void PingMasterServer() {
    await apiClient.PingServerAlive(gameServerData.id);
    lastMasterServerUpdate = DateTime.Now;
  }

  private async void StartServerPingLoop() {
    while (active) {
      if (lastMasterServerUpdate == null || DateTime.Now.Subtract(lastMasterServerUpdate).Seconds >= alivePingIntervalSeconds) {
        PingMasterServer();
      }
      await Task.Delay(PING_LOOP_DELAY);
    }
  }
}

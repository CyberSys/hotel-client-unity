using LiteNetLib;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ServerNetworkController : MonoBehaviour, INetEventListener
{
  public static int SERVER_PORT = 10770;

  private NetManager netManager;
  private int lastLatency;

  public void StartServer() {
    netManager.Start(GetServerPort());
  }

  public int GetServerPort() {
    return SERVER_PORT;
  }

  void Start() {
    netManager = new NetManager(this) {
      AutoRecycle = true
    };
  }

  void Update() {
    netManager.PollEvents();
  }

  /// LiteNet network event methods

  public void OnConnectionRequest(ConnectionRequest request) {
    // TODO: Key exchange here.
    request.Accept();
  }

  public void OnPeerConnected(NetPeer peer) {
    Debug.Log("Peer connected: " + peer.EndPoint);
  }

  public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
    Debug.Log("Peer disconnected: " + peer.EndPoint);
  }

  public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) {
    Debug.LogWarning("Network error: " + socketError);
  }

  public void OnNetworkLatencyUpdate(NetPeer peer, int latency) {
    lastLatency = latency;
  }

  public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) {
    Debug.Log("Network recv");
  }

  public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) {
    // NOP.
  }
}

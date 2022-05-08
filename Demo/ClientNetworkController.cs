using LiteNetLib;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Hotel {

  public class ClientNetworkController : MonoBehaviour, INetEventListener {
    private NetManager netManager;
    private int lastLatency;

    public void JoinServer(string host, int port) {
      netManager.Connect(host, port, "test");
    }

    private void Start() {
      netManager = new NetManager(this) {
        AutoRecycle = true
      };
      netManager.Start();
    }

    void Update() {
      netManager.PollEvents();
    }

    /// LiteNet network event methods

    public void OnConnectionRequest(ConnectionRequest request) {
      // Client socket can never accept connections.
      request.Reject();
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

} // namespace Hotel

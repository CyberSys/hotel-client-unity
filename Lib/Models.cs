using System;
using System.Net;

namespace Hotel {

  [Serializable]
  public class IdentifyResponse {
    public string token;
  }

  [Serializable]
  public class ListServersResponse {
    public GameServer[] servers;
  }

  [Serializable]
  public class GameServer {
    public int id;
    public string gameId;
    public string name;
    public string host;
    public int port;
    public int numPlayers;
    public int maxPlayers;

    public IPEndPoint ResolveIPEndPoint() {
      var addresses = Dns.GetHostAddresses(host);
      if (addresses.Length < 1) {
        throw new ArgumentException("Unable to resolve host " + host);
      }
      return new IPEndPoint(addresses[0], port);
    }

    public override string ToString() {
      return $"GameServer: {name}, {host}:{port}, {numPlayers}/{maxPlayers}";
    }
  }

} // namespace Hotel
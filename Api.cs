﻿using System;

/// C# Implementation of structs in api.go
namespace Api
{
  [Serializable]
  public class IdentifyResponse
  {
     public string token;
  }

  [Serializable]
  public class ListServersResponse
  {
    public GameServer[] servers;
  }

  [Serializable]
  public class GameServer
  {
    public int id;
    public string gameId;
    public string name;
    public string host;
    public int port;
    public int numPlayers;
    public int maxPlayers;
  }
}

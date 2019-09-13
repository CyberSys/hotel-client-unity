using System;
using System.Collections;
using System.Collections.Generic;
using Api;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ServerListUI : MonoBehaviour
{
  [Serializable]
  public class JoinServerEvent : UnityEvent<string, int> { }

  private const string DYN_BTN_NAME = "Dynamic btn";

  // Text in scene to use as the prototype for factories.
  public Button prototypeJoinButton;

  // Event handler for joining servers.
  public JoinServerEvent joinEventHandler;

  public void DisplayServers(GameServer[] servers) {
    CleanUp();
    foreach (var server in servers) {
      AddJoinButton(server.host, server.port, string.Format(
          "Join \"{0}\" - {1}:{2} - {3}/{4}",
          server.name, server.host, server.port, server.numPlayers, server.maxPlayers));
    }
  }

  async void Start() {
    prototypeJoinButton.gameObject.SetActive(false);
    var buttons = GetComponentsInChildren<Button>();
    await HotelClient.Instance.WaitUntilInitialized();
    foreach (var button in buttons) {
      button.interactable = true;
    }
  }

  private void CleanUp() {
    // A recycle pattern would be better here.
    var children = GetComponentsInChildren<Button>();
    foreach (var child in children) {
      if (child.name == DYN_BTN_NAME) {
        Destroy(child.gameObject);
      }
    }
  }

  private void AddJoinButton(string host, int port, string label) {
    var obj = Instantiate(prototypeJoinButton.gameObject, transform);
    obj.SetActive(true);
    obj.name = DYN_BTN_NAME;
    var btn = obj.GetComponent<Button>();
    var text = obj.GetComponentInChildren<Text>();
    text.text = label;
    btn.onClick.AddListener(() => {
      joinEventHandler.Invoke(host, port);
    });
  }
}

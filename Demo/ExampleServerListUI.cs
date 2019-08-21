using System.Collections;
using System.Collections.Generic;
using Api;
using UnityEngine;
using UnityEngine.UI;

public class ExampleServerListUI : MonoBehaviour
{
  private const string DYN_TEXT_NAME = "Dynamic text";

  // Text in scene to use as the prototype for factories.
  public Text prototypeText;

  public void DisplayServers(Server[] servers) {
    CleanUp();
    foreach (var server in servers) {
      AddText(string.Format(
          "{0} - {1}:{2} - {3}/{4}", server.name, server.host, server.port, server.numPlayers, server.maxPlayers));
    }
  }

  async void Start() {
    prototypeText.gameObject.SetActive(false);
    var buttons = GetComponentsInChildren<Button>();
    await HotelClient.Instance.WaitUntilInitialized();
    foreach (var button in buttons) {
      button.interactable = true;
    }
  }

  private void CleanUp() {
    // A recycle pattern would be better here.
    var textChildren = GetComponentsInChildren<Text>();
    foreach (var child in textChildren) {
      if (child.name == DYN_TEXT_NAME) {
        Destroy(child.gameObject);
      }
    }
  }

  private Text AddText(string label) {
    var obj = Instantiate(prototypeText.gameObject, transform);
    obj.SetActive(true);
    obj.name = DYN_TEXT_NAME;
    var text = obj.GetComponent<Text>();
    text.text = label;
    return text;
  }
}

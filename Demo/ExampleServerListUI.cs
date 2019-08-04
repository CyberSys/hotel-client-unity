using System.Collections;
using System.Collections.Generic;
using Api;
using UnityEngine;
using UnityEngine.UI;

public class ExampleServerListUI : MonoBehaviour
{
  // Text in scene to use as the prototype for factories.
  public Text prototypeText;

  public void DisplayServers(Server[] servers) {
    CleanUp();
    foreach (var server in servers) {
      AddText(string.Format(
          "{0} - {1}:{2} - {3}/{4}", server.name, server.host, server.port, server.numPlayers, server.maxPlayers));
    }
  }

  void Start() {
    prototypeText.gameObject.SetActive(false);
  }

  private void CleanUp() {
    // A recycle pattern would be better here.
    var textChildren = GetComponentsInChildren<Text>();
    foreach (var child in textChildren) {
      Destroy(child.gameObject);
    }
  }

  private Text AddText(string label) {
    var obj = Instantiate(prototypeText.gameObject, transform);
    obj.SetActive(true);
    obj.name = "Dynamic text";
    var text = obj.GetComponent<Text>();
    text.text = label;
    return text;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hotel {

  [RequireComponent(typeof(Text))]
  public class ConsoleLog : MonoBehaviour {
    private Text textComponent;

    void Start() {
      textComponent = GetComponent<Text>();
    }

    void OnEnable() {
      Application.logMessageReceived += HandleLogMessage;
    }

    void OnDisable() {
      Application.logMessageReceived -= HandleLogMessage;
    }

    private void HandleLogMessage(string log, string stack, LogType type) {
      if (textComponent == null) {
        return;
      }

      textComponent.text = textComponent.text + "\n" + log;
    }
  }

} // namespace Hotel

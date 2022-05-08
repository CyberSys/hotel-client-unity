namespace Hotel {

  public static class Util {
    public static string GetFlagValue(string flagName) {
      var args = System.Environment.GetCommandLineArgs();
      for (int i = 0; i < args.Length; i++) {
        if (args[i].StartsWith(flagName)) {
          var chunks = args[i].Split('=');
          if (chunks.Length > 1) {
            return chunks[1];
          }
        }
      }
      return "";
    }
  }


} // namespace Hotel

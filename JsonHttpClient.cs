using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class JsonHttpClient
{
  private static readonly HttpClient client = new HttpClient();
  private string sessionToken;

  public JsonHttpClient(Uri baseHost) {
    client.BaseAddress = baseHost;
  }

  public void SetSessionToken(string sessionToken) {
    this.sessionToken = sessionToken;
  }

  public async Task<T> Get<T>(string url) {
    return await Send<T>(HttpMethod.Get, url, null);
  }

  public async Task<T> Post<T>(string url) {
    return await Post<T>(url, null);
  }

  public async Task<T> Post<T>(string url, System.Object request) {
    return await Send<T>(HttpMethod.Post, url, request);
  }

  public async Task<T> Put<T>(string url, System.Object request) {
    return await Send<T>(HttpMethod.Put, url, request);
  }

  public async Task<T> Send<T>(HttpMethod method, string url, System.Object request) {
    var httpRequest = new HttpRequestMessage(method, url);
    if (sessionToken != null) {
      httpRequest.Headers.TryAddWithoutValidation("X-Session-Token", sessionToken);
    }
    if (request != null) {
      httpRequest.Content = new StringContent(JsonUtility.ToJson(request));
    }
    var httpResponse = await client.SendAsync(httpRequest);
    if (httpResponse.StatusCode != HttpStatusCode.OK) {
      Debug.LogWarning(string.Format("Unexpected status code for path {0}: {1}", url, httpResponse.StatusCode));
    }
    var content = await httpResponse.Content.ReadAsStringAsync();
    return JsonUtility.FromJson<T>(content);
  }
}

using Assets.Up2Unity.Runtime;
using System.Collections;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Up2JamManager
{
    public Up2JamManager(string clientName, string gameSlug)
    {
        _clientName = clientName;
        _gameSlug = gameSlug;
    }

    private string _clientName;
    private string _gameSlug;

    private string _deviceCode;
    private string _token;

    public string Token { set; get; }

    public IEnumerator ConnectCoroutine(System.Action<LoginData> onSuccess)
    {
        using UnityWebRequest request = new("https://d2jam.com/api/v1/device/code", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(
            JsonConvert.SerializeObject(new
            {
                clientName = _clientName,
                gameSlug = _gameSlug
            })
        );
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        var res = JsonConvert.DeserializeObject<Up2JamResponse<LoginData>>(request.downloadHandler.text);

        if (res.Success)
        {
            _deviceCode = res.Data.DeviceCode;
            onSuccess?.Invoke(res.Data);
        }
        else throw new Up2JamException(res.Error);
    }
}

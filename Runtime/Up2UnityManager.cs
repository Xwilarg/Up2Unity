using Assets.Up2Unity.Runtime;
using System.Collections;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Up2UnityManager
{
    /// <param name="clientName">Chosen name for your client</param>
    /// <param name="gameSlug">URL part that is unique to your project name</param>
    public Up2UnityManager(string clientName, string gameSlug)
    {
        _clientName = clientName;
        _gameSlug = gameSlug;
    }

    private string _clientName;
    private string _gameSlug;

    private string _deviceCode;
    private string _token;

    public string Token { set; get; }

    /// <summary>
    /// Connect to the API and get a connection URL for the user
    /// </summary>
    public async Awaitable<LoginData> Connect()
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
        await request.SendWebRequest();

        var res = JsonConvert.DeserializeObject<Up2UnityResponse<LoginData>>(request.downloadHandler.text);

        if (res.Success)
        {
            _deviceCode = res.Data.DeviceCode;
            return res.Data;
        }
        throw new Up2UnityException(res.Error);
    }

    public async Awaitable<bool> ValidateToken()
    {
        using UnityWebRequest request = new("https://d2jam.com/api/v1/device/token", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(
            JsonConvert.SerializeObject(new
            {
                deviceCode = _deviceCode
            })
        );
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        await request.SendWebRequest();

        var res = JsonConvert.DeserializeObject<Up2UnityResponse<TokenData>>(request.downloadHandler.text);

        if (res.Success)
        {
            if (res.Data.Status == "approved")
            {
                _token = res.Data.Token;
                return true;
            }
            return false;
        }
        throw new Up2UnityException(res.Error);
    }
}

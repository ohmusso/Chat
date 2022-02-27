using Grpc.Core;
using MagicOnion.Client;
using Chat.Shared.Services;
using UnityEngine;

namespace Chat
{
    public class ChatController : MonoBehaviour
    {
        private Channel _channel;
        private IMyFirstService _service;

        void Awake()
        {
            _channel = new Channel("localhost", 5000, ChannelCredentials.Insecure);
            _service = MagicOnionClient.Create<IMyFirstService>(_channel);
        }

        async void Start()
        {
            var x = Random.Range(0, 1000);
            var y = Random.Range(0, 1000);
            var result = await _service.SumAsync(x, y);
            Debug.Log($"Result: {result}");
        }

        async void OnDestroy()
        {
            if (_channel != null)
            {
                await _channel.ShutdownAsync();
            }
        }
    }
}

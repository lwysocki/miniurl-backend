using Grpc.Core;
using Microsoft.Extensions.Options;
using MiniUrl.KeyManager.Domain.Models;
using System;
using System.Threading.Tasks;

namespace GrpcKeysManager
{
    public class KeysManagerService(IOptions<KeysManagerService.KeysManagerSettings> settings, IKeysManagerRepository repository) : KeysManager.KeysManagerBase
    {
        public class KeysManagerSettings
        {
            public const string Section = "KeysManager";

            public int Threshold { get; set; }
        }

        public KeysManagerSettings Settings { get; private set; } = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        public readonly IKeysManagerRepository _repository = repository;

        public override async Task<KeyIdReply> GetAvailableKeyId(KeyIdRequest request, ServerCallContext context)
        {
            var keysCount = await _repository.CountAvailableKeys();

            Random rand = new();
            int skipRowsCount = rand.Next(keysCount);

            var key = await _repository.GetAvailableKeyAsync(skipRowsCount);

            return new KeyIdReply() { Id = key.Id };
        }
    }
}

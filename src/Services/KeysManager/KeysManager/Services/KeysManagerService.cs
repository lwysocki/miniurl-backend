using Microsoft.Extensions.Options;
using MiniUrl.KeyManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Services
{
    public class KeysManagerService : IKeysManagerService
    {
        public class KeysManagerSettings
        {
            public const string Section = "KeysManager";

            public int Threshold { get; set; }
        }

        public KeysManagerSettings Settings { get; private set; }
        public readonly IKeysManagerRepository _repository;

        public KeysManagerService(IOptions<KeysManagerSettings> settings, IKeysManagerRepository repository)
        {
            Settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<long> GetAvailableKeyIdAsync()
        {
            var keysCount = await _repository.CountAvailableKeys();

            Random rand = new();
            int skipRowsCount = rand.Next(keysCount);

            var key = await _repository.GetAvailableKeyAsync(skipRowsCount);

            return key.Id;
        }
    }
}

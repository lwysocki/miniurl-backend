using System;
using System.Threading.Tasks;
using dotnet_etcd;
using Etcdserverpb;

namespace MiniUrl.Association.Infrastructure;

public class MachineIdResolver(string connectionString)
{
    private readonly EtcdClient _client = new(connectionString);
    private const string KeyPrefix = "/miniurl/association/machine-ids/";
    private const int MaxMachineId = 1023;

    public async Task<int> ClaimMachineIdAsync()
    {
        var leaseGrantRequest = new LeaseGrantRequest { TTL = 30 };
        var leaseResponse = await _client.LeaseGrantAsync(leaseGrantRequest);
        long leaseId = leaseResponse.ID;

        _ = Task.Run(async () =>
        {
            try
            {
                await _client.LeaseKeepAlive(leaseId, new System.Threading.CancellationToken());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Etcd KeepAlive stopped: {ex.Message}");
            }
        });

        int startId = Random.Shared.Next(0, MaxMachineId + 1);
        string instanceValue = Environment.MachineName;

        for (int i = 0; i <= MaxMachineId; i++)
        {
            int candidateId = (startId + i) % (MaxMachineId + 1);
            string key = $"{KeyPrefix}{candidateId}";

            var txnRequest = new TxnRequest();
            txnRequest.Compare.Add(new Compare
            {
                Key = Google.Protobuf.ByteString.CopyFromUtf8(key),
                Result = Compare.Types.CompareResult.Equal,
                Target = Compare.Types.CompareTarget.Version,
                Version = 0
            });

            txnRequest.Success.Add(new RequestOp
            {
                RequestPut = new PutRequest
                {
                    Key = Google.Protobuf.ByteString.CopyFromUtf8(key),
                    Value = Google.Protobuf.ByteString.CopyFromUtf8(instanceValue),
                    Lease = leaseId
                }
            });

            var txnResponse = await _client.TransactionAsync(txnRequest);

            if (txnResponse.Succeeded)
            {
                return candidateId;
            }
        }

        throw new Exception("CRITICAL: No available Machine IDs left in etcd (Cluster capacity full at 1024 instances).");
    }
}

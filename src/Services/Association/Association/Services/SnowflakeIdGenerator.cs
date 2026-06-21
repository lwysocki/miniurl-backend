using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniUrl.Association.Services;

public class SnowflakeIdGenerator : IIdGenerator
{
    private const int MachineIdBits = 10;
    private const int SequenceBits = 12;
    private const int MachineIdShift = SequenceBits;
    private const int TimestampShift = SequenceBits + MachineIdBits;
    private const int SequenceMask = -1 ^ (-1 << SequenceBits); // 4095 (0xFFF)
    private const long Epoch = 1420041600000L; // Custom Epoch (e.g. 2015-01-01)

    private readonly long _machineId;
    private readonly Lock _lock = new();

    private long _lastTimestamp = -1L;
    private int _counter = 0;

    public SnowflakeIdGenerator(int machineId)
    {
        long maxMachineId = -1L ^ (-1L << MachineIdBits);

        if (machineId < 0 || machineId > maxMachineId)
        {
            throw new ArgumentException($"Machine ID must be between 0 and {maxMachineId}");
        }

        _machineId = machineId;
    }

    public Task<long> GenerateIdAsync()
    {
        lock (_lock)
        {
            return Task.FromResult(GenerateId());
        }
    }

    private long GenerateId()
    {
        long timestamp = GetCurrentTimestamp();

        if (timestamp < _lastTimestamp)
        {
            throw new Exception($"Clock moved backwards. Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");
        }

        if (timestamp == _lastTimestamp)
        {
            _counter = (_counter + 1) & SequenceMask;

            if (_counter == 0)
            {
                timestamp = WaitNextMillis(_lastTimestamp);
            }
        }
        else
        {
            _counter = 0;
        }

        _lastTimestamp = timestamp;

        return ((timestamp - Epoch) << TimestampShift) |
                (_machineId << MachineIdShift) |
                (long)_counter;
    }

    private long WaitNextMillis(long lastTimestamp)
    {
        long timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }

    private long GetCurrentTimestamp()
    {
        return DateTime.UtcNow.Ticks / 10000; // milliseconds
    }
}

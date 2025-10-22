using System;

public static class SnowflakeGenerator
{
    private const int WorkerIdBits = 5;
    private const int DatacenterIdBits = 5;
    private const int SequenceBits = 12;

    private const long MaxWorkerId = (1L << WorkerIdBits) - 1;
    private const long MaxDatacenterId = (1L << DatacenterIdBits) - 1;

    private const int TimestampShift = SequenceBits + WorkerIdBits + DatacenterIdBits;
    private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
    private const int WorkerIdShift = SequenceBits;

    private const long SequenceMask = (1L << SequenceBits) - 1;

    private static readonly DateTime Epoch = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private static readonly object _lock = new object();

    private static readonly long _workerId;
    private static readonly long _datacenterId;

    private static long _lastTimestamp = -1L;
    private static long _sequence = 0L;

    static SnowflakeGenerator()
    {
        long datacenterId = 5;
        long workerId = 1;
        if (datacenterId > MaxDatacenterId || datacenterId < 0)
            throw new ArgumentException($"Datacenter ID must be between 0 and {MaxDatacenterId}");

        if (workerId > MaxWorkerId || workerId < 0)
            throw new ArgumentException($"Worker ID must be between 0 and {MaxWorkerId}");

        _datacenterId = datacenterId;
        _workerId = workerId;
    }

    public static long NextId()
    {
        lock (_lock)
        {
            long timestamp = GetCurrentTimestamp();

            if (timestamp < _lastTimestamp)
                throw new InvalidOperationException("Clock moved backwards. Refusing to generate ID");

            if (timestamp == _lastTimestamp)
            {
                _sequence = (_sequence + 1) & SequenceMask;
                if (_sequence == 0)
                {
                    timestamp = WaitNextMillis(timestamp);
                }
            }
            else
            {
                _sequence = 0L;
            }

            _lastTimestamp = timestamp;

            return (timestamp << TimestampShift)
                   | (_datacenterId << DatacenterIdShift)
                   | (_workerId << WorkerIdShift)
                   | _sequence;
        }
    }

    private static long WaitNextMillis(long currentTimestamp)
    {
        long timestamp = GetCurrentTimestamp();
        while (timestamp <= currentTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }

    private static long GetCurrentTimestamp()
    {
        return (long)(DateTime.UtcNow - Epoch).TotalMilliseconds;
    }
}
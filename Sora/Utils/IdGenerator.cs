using System.Diagnostics;

namespace Sora.Utils;

public class IdGenerator
{
    private const long Epoch = 1756684800000L;
    private const int SequenceBits = 12;
    private const int MachineIdBits = 5;
    private const int DataCentreIdBits = 5;
    private const int MaxSequence = (1 << SequenceBits) - 1;
    private const int MaxMachineId = (1 << MachineIdBits) - 1;
    private const int MaxDataCentreId = (1 << DataCentreIdBits) - 1;
    private readonly long _machineId;
    private readonly long _dataCentreId;
    private long _lastTimestamp;
    private long _sequence;
    private readonly Lock _lock = new();

    public IdGenerator(long machineId, long dataCentreId)
    {
        if (machineId is < 0 or > MaxMachineId)
        {
            throw new ArgumentOutOfRangeException(nameof(machineId), $"Machine id must be between 0 and {MaxMachineId}");
        }

        if (dataCentreId is < 0 or > MaxDataCentreId)
        {
            throw new ArgumentOutOfRangeException(nameof(dataCentreId), $"Data Centre id must be between 0 and {MaxDataCentreId}");
        }
        
        _machineId = machineId;
        _dataCentreId = dataCentreId;
    }

    public long Next()
    {
        lock (_lock)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (_lastTimestamp > timestamp)
            {
                throw new InvalidOperationException("Clock moved backwards. Refusing to generate id.");
            }

            if (timestamp == _lastTimestamp)
            {
                _sequence = (_sequence + 1) & MaxSequence;
                if (_sequence == 0)
                {
                    var sw = Stopwatch.StartNew();
                    while (sw.ElapsedMilliseconds < 1)
                    {
                        Thread.SpinWait(1);
                    }
                }
            }
            else
            {
                _sequence = 0;
            }
            
            _lastTimestamp = timestamp;
            var id = ((timestamp - Epoch) << (SequenceBits + MachineIdBits + DataCentreIdBits))
                | (_dataCentreId << (SequenceBits + MachineIdBits))
                | (_machineId << SequenceBits)
                | _sequence;

            return id;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSoft.SkyNet.Azure.Postgres;

public class SkyNetDbHttpClientOptions
{
    public int SemaphoreInitCount { get; init; } = 64;

    public TimeSpan SemaphoreWaitTimeout { get; init; } = TimeSpan.FromMilliseconds(1000);
}
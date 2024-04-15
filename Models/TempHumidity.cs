﻿using System;
using System.Collections.Generic;

namespace TempHumidityBackend.Models;

public partial class TempHumidity
{
    public int Id { get; set; }

    public float? TempC { get; set; }

    public float? RelHumidity { get; set; }

    public long? Timestamp { get; set; }
}

﻿using System;
using System.Collections.Generic;

namespace BuildingSource.Models;

public partial class ServiceType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Service> Services { get; } = new List<Service>();
}

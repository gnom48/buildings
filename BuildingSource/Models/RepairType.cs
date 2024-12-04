using System;
using System.Collections.Generic;

namespace BuildingSource.Models;

public partial class RepairType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? M2price { get; set; }

    public virtual ICollection<Service> Services { get; } = new List<Service>();

    public virtual ICollection<UsersRequest> UsersRequests { get; } = new List<UsersRequest>();
}

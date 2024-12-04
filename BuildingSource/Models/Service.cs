using System;
using System.Collections.Generic;

namespace BuildingSource.Models;

public partial class Service
{
    public int Id { get; set; }

    public int? RepairType { get; set; }

    public int? ServiceId { get; set; }

    public decimal? Price { get; set; }

    public virtual RepairType? RepairTypeNavigation { get; set; }

    public virtual ICollection<RequestOption> RequestOptions { get; } = new List<RequestOption>();

    public virtual ServiceType? ServiceNavigation { get; set; }
}

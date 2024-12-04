using System;
using System.Collections.Generic;

namespace BuildingSource.Models;

public partial class UsersRequest
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ObjectTypeId { get; set; }

    public int? RepairTypeId { get; set; }

    public float? Square { get; set; }

    public virtual ObjectType? ObjectType { get; set; }

    public virtual RepairType? RepairType { get; set; }

    public virtual ICollection<RequestOption> RequestOptions { get; } = new List<RequestOption>();

    public virtual User? User { get; set; }
}

using System;
using System.Collections.Generic;

namespace BuildingSource.Models;

public partial class ObjectType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UsersRequest> UsersRequests { get; } = new List<UsersRequest>();
}

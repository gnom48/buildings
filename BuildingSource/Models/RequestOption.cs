using System;
using System.Collections.Generic;

namespace BuildingSource.Models;

public partial class RequestOption
{
    public int UserRequestId { get; set; }

    public int ServiceId { get; set; }

    public byte[]? Datetime { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual UsersRequest UserRequest { get; set; } = null!;
}

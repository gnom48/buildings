using System;
using System.Collections.Generic;

namespace BuildingSource.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Fname { get; set; }

    public string? Surname { get; set; }

    public string? Patronumic { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Passport { get; set; }

    public string? Address { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<UsersRequest> UsersRequests { get; } = new List<UsersRequest>();
}

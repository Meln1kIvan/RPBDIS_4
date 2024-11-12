using System;
using System.Collections.Generic;

namespace RPBDIS_4.Models;

public partial class MaintenanceType
{
    public int MaintenanceTypeId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<CompletedWork> CompletedWorks { get; set; } = new List<CompletedWork>();

    public virtual ICollection<MaintenanceSchedule> MaintenanceSchedules { get; set; } = new List<MaintenanceSchedule>();
}

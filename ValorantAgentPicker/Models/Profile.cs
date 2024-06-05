using System;
using System.Collections.Generic;

namespace ValorantAgentPicker.Models;

public partial class Profile
{
    public int ProfileId { get; set; }

    public int? FirstMx { get; set; }

    public int? FirstMy { get; set; }

    public int? LastMx { get; set; }

    public int? LastMy { get; set; }

    public int? AgentId { get; set; }

    public virtual Agent? Agent { get; set; }
}

using System;
using System.Collections.Generic;

namespace ValorantAgentPicker.Models;

public partial class Agent
{
    public int AgentId { get; set; }

    public string? AgentName { get; set; }

    public string? AgentPhoto { get; set; }

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}

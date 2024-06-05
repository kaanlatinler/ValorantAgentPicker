using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValorantAgentPicker.Models;

namespace ValorantAgentPicker
{
    public class Database
    {
        private readonly AgentPickerContext _context;

        public Database(AgentPickerContext context)
        {
            _context = context;
        }

        public List<Agent> GetAgents()
        {
            List<Agent> list = _context.Agents.ToList();

            return list;
        }

        public Agent? GetAgentByName(string name)
        {
            var selected = _context.Agents.FirstOrDefault(a=>a.AgentName == name);

            return selected;
        }

        public bool SaveNewProfile(Profile profile)
        {
            _context.Profiles.Add(profile);
            _context.SaveChanges();

            var saved = GetAgentByName(profile.Agent.AgentName);
            if(saved == null) { return false; }
            return true;
        }

        public List<Profile> GetProfiles()
        {
            List<Profile> list = _context.Profiles.ToList();

            return list;
        }

        public Profile? GetProfileByID(int id)
        {
            var profile = _context.Profiles.FirstOrDefault(p => p.ProfileId == id);
            if (profile == null) { return null; }
            return profile;
        }

        public bool UpdateProfile(Profile profile)
        {
            _context.Profiles.Update(profile);
            _context.SaveChanges();

            return true;
        }

        public bool DeleteProfile(int profileID)
        {
            var profile = GetProfileByID(profileID);

            _context.Profiles.Remove(profile);
            _context.SaveChanges();

            return true;
        }
    }
}

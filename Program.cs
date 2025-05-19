using System;
using System.Collections.Generic;
using static System.Net.WebRequestMethods;

namespace EmergencyOperation
{
    // סוגי מקרי חירום
    public enum EmergencyType
    {
        Flood,
        Injury,
        Blockage,
        Shortage
    }

    // דיווח חירום
    internal class EmergencyReport
    {
        public EmergencyType Type;
        public string Zone;
        public int SeverityLevel;
        public float Duration;
        public string Description;

        public EmergencyReport(EmergencyType type, string zone, int severityLevel, float duration, string description)
        {
            Type = type;
            Zone = zone;
            SeverityLevel = severityLevel;
            Duration = duration;
            Description = description;
        }
    }

    //צוות מופשט
    internal abstract class Team
    {
        public string Name;
        public string Zone;
        public bool IsAvailable;

        protected Team(string name, string zone)
        {
            Name = name;
            Zone = zone;
            IsAvailable = true;
        }

        public virtual bool IsMatch(EmergencyReport report)
        {
            return false;
        }

        public virtual void HandleReport(EmergencyReport report)
        {
            Console.WriteLine($"{Name}: cannot respond to {report.Description}");
        }
    }


    internal class Flood : Team
    {
        public Flood(string name, string zone) : base(name, zone) { }

        public override bool IsMatch(EmergencyReport report)
        {
            return IsAvailable && report.Zone == Zone && report.Type == EmergencyType.Flood;
        }

        public override void HandleReport(EmergencyReport report)
        {
            Console.WriteLine($"{Name} is handling FLOOD: {report.Description}");
            IsAvailable = false;
        }
    }

    internal class Injury : Team
    {
        public Injury(string name, string zone) : base(name, zone) { }

        public override bool IsMatch(EmergencyReport report)
        {
            return IsAvailable && report.Zone == Zone &&
                   report.Type == EmergencyType.Injury &&
                   report.SeverityLevel >= 3;
        }

        public override void HandleReport(EmergencyReport report)
        {
            Console.WriteLine($"{Name} is treating INJURY: {report.Description}");
            IsAvailable = false;
        }
    }

    
    internal class Blockage : Team
    {
        public Blockage(string name, string zone) : base(name, zone) { }

        public override bool IsMatch(EmergencyReport report)
        {
            return IsAvailable && report.Zone == Zone &&
                   report.Type == EmergencyType.Blockage &&
                   report.Duration > 2;
        }

        public override void HandleReport(EmergencyReport report)
        {
            Console.WriteLine($"{Name} is clearing BLOCKAGE: {report.Description}");
            IsAvailable = false;
        }
    }

    //בסיי 
    internal class DispatchCenter
    {
        private List<Team> teams;

        public DispatchCenter(List<Team> teams)
        {
            this.teams = teams;
        }

        public void Dispatch(EmergencyReport report)
        {
            foreach (Team team in teams)
            {
                if (team.IsMatch(report))
                {
                    team.HandleReport(report);
                    return;
                }
            }

            Console.WriteLine("No available team for this report.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Team> teams = new List<Team>
            {
                new Flood("Flood Team A", "North"),
                new Injury("Medic One", "North"),
                new Blockage("RoadClear Squad", "North")
            };

            DispatchCenter center = new DispatchCenter(teams);

            EmergencyReport report1 = new EmergencyReport(EmergencyType.Flood, "North", 2, 5, "Flood near river");
            center.Dispatch(report1);

            EmergencyReport report2 = new EmergencyReport(EmergencyType.Injury, "North", 4, 1, "Car accident injury");
            center.Dispatch(report2);

            EmergencyReport report3 = new EmergencyReport(EmergencyType.Blockage, "North", 1, 3, "Tree blocking road");
            center.Dispatch(report3);

            EmergencyReport report4 = new EmergencyReport(EmergencyType.Injury, "South", 5, 1, "Remote injury");
            center.Dispatch(report4);
        }
    }

}

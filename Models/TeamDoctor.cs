using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task3_9.Models
{
    public class TeamDoctor : IDoctor
    {
        public string Name { get; set; }
        public string Specialization { get; } = "Team Doctor";
        public double TreatmentEffectiveness { get; } = 0.8;

        public TeamDoctor(string name)
        {
            Name = name;
        }

        public async Task TreatAthleteAsync(Athlete athlete)
        {
            Console.WriteLine($"{Name} is treating {athlete.Name}");
            // Simulate treatment time
            await Task.Delay(1000);
            athlete.RecoverFromInjury(TreatmentEffectiveness);
        }
    }
}
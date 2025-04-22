using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task3_9.Models
{
    public class PhysiotherapistDoctor : IDoctor
    {
        public string Name { get; set; }
        public string Specialization { get; } = "Physiotherapist";
        public double TreatmentEffectiveness { get; } = 0.7;

        public PhysiotherapistDoctor(string name)
        {
            Name = name;
        }

        public async Task TreatAthleteAsync(Athlete athlete)
        {
            Console.WriteLine($"{Name} is providing physiotherapy to {athlete.Name}");
            // Simulate physiotherapy treatment time
            await Task.Delay(1500);
            athlete.RecoverFromInjury(TreatmentEffectiveness);
        }
    }
}
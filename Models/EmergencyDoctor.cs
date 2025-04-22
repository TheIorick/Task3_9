using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task3_9.Models
{
    public class EmergencyDoctor : IDoctor
    {
        public string Name { get; set; }
        public string Specialization { get; } = "Emergency Specialist";
        public double TreatmentEffectiveness { get; } = 0.9;

        public EmergencyDoctor(string name)
        {
            Name = name;
        }

        public async Task TreatAthleteAsync(Athlete athlete)
        {
            Console.WriteLine($"{Name} is providing emergency care to {athlete.Name}");
            // Simulate emergency treatment time
            await Task.Delay(800);
            athlete.RecoverFromInjury(TreatmentEffectiveness);
        }
    }
}
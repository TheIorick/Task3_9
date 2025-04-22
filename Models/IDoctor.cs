using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task3_9.Models
{
    public interface IDoctor
    {
        string Name { get; }
        string Specialization { get; }
        Task TreatAthleteAsync(Athlete athlete);
        double TreatmentEffectiveness { get; }
    }
}
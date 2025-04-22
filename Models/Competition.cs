using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Task3_9.Models
{
    public class Competition : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Basic properties
        public string Name { get; set; }
        public string Sport { get; set; }
        public double Distance { get; set; } // The total distance of the race
        public double InjuryProbability { get; set; } = 0.05; // 5% chance of injury by default

        // Collection of athletes
        public ObservableCollection<Athlete> Athletes { get; private set; }

        // Collection of doctors
        private List<IDoctor> _doctors;
        public IReadOnlyList<IDoctor> Doctors => _doctors.AsReadOnly();

        // Competition state
        private bool _isCompetitionRunning;
        public bool IsCompetitionRunning
        {
            get => _isCompetitionRunning;
            private set
            {
                if (_isCompetitionRunning != value)
                {
                    _isCompetitionRunning = value;
                    OnPropertyChanged();
                }
            }
        }

        private Athlete? _winner;
        public Athlete? Winner
        {
            get => _winner;
            private set
            {
                if (_winner != value)
                {
                    _winner = value;
                    OnPropertyChanged();
                }
            }
        }

        // Events
        public delegate void CompetitionEventHandler(Competition competition);
        public delegate void AthleteEventHandler(Athlete athlete);
        public event CompetitionEventHandler? CompetitionStarted;
        public event CompetitionEventHandler? CompetitionEnded;
        public event AthleteEventHandler? AthleteInjured;
        public event AthleteEventHandler? AthleteTreated;
        public event AthleteEventHandler? AthleteWon;

        // Random generator
        private readonly Random _random = new Random();
        
        // Cancellation token for stopping the competition
        private CancellationTokenSource? _cancellationTokenSource;

        public Competition(string name, string sport, double distance)
        {
            Name = name;
            Sport = sport;
            Distance = distance;
            Athletes = new ObservableCollection<Athlete>();
            _doctors = new List<IDoctor>();
            IsCompetitionRunning = false;
            Winner = null;
        }

        public void AddAthlete(Athlete athlete)
        {
            Athletes.Add(athlete);
            athlete.InjuryOccurred += OnAthleteInjured;
            athlete.InjuryHealed += OnAthleteHealed;
        }

        public void AddDoctor(IDoctor doctor)
        {
            _doctors.Add(doctor);
        }

        private void OnAthleteInjured(Athlete athlete)
        {
            AthleteInjured?.Invoke(athlete);
            
            // Find available doctor and treat
            if (_doctors.Count > 0)
            {
                Task.Run(async () =>
                {
                    // Choose a random doctor
                    var doctor = _doctors[_random.Next(_doctors.Count)];
                    await doctor.TreatAthleteAsync(athlete);
                });
            }
        }

        private void OnAthleteHealed(Athlete athlete)
        {
            AthleteTreated?.Invoke(athlete);
        }

        public async Task StartCompetitionAsync()
        {
            if (IsCompetitionRunning || Athletes.Count == 0)
                return;

            // Reset athlete positions
            foreach (var athlete in Athletes)
            {
                athlete.Progress = 0;
                athlete.IsInjured = false;
            }

            Winner = null;
            IsCompetitionRunning = true;
            CompetitionStarted?.Invoke(this);

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            var tasks = Athletes.Select(athlete => SimulateAthleteAsync(athlete, token)).ToArray();

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                // Competition was cancelled
            }
            finally
            {
                IsCompetitionRunning = false;
                CompetitionEnded?.Invoke(this);
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }

        public void StopCompetition()
        {
            _cancellationTokenSource?.Cancel();
        }

        private async Task SimulateAthleteAsync(Athlete athlete, CancellationToken cancellationToken)
        {
            while (athlete.Progress < Distance)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();

                if (!athlete.IsInjured)
                {
                    // Calculate movement based on athlete's speed and stamina
                    double step = athlete.Speed * (0.8 + 0.4 * athlete.Stamina / 100) / 10;
                    athlete.Progress += step;

                    // Check for potential injury
                    athlete.CheckForInjury(_random, InjuryProbability);
                }

                // If athlete reached the end and there's no winner yet
                if (athlete.Progress >= Distance && Winner == null)
                {
                    Winner = athlete;
                    AthleteWon?.Invoke(athlete);
                }

                await Task.Delay(100, cancellationToken);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
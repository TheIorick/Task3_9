using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Task3_9.Models;
using CommunityToolkit.Mvvm.Input;

namespace Task3_9.ViewModels
{
    public class CompetitionViewModel : ViewModelBase
    {
        private Competition _competition;
        private string _competitionStatus;
        private string _eventLog;

        public Competition Competition => _competition;
        
        public ObservableCollection<Athlete> Athletes => _competition.Athletes;
        
        public double TrackLength => _competition.Distance;
        
        public double TrackHeight => 400; // Фиксированная высота для визуализации трека
        
        public string CompetitionStatus
        {
            get => _competitionStatus;
            set => SetProperty(ref _competitionStatus, value);
        }
        
        public string EventLog
        {
            get => _eventLog;
            set => SetProperty(ref _eventLog, value);
        }
        
        public string CompetitionName => _competition.Name;

        public ICommand StartCompetitionCommand { get; }
        public ICommand StopCompetitionCommand { get; }
        
        
        public CompetitionViewModel()
        {
            // Create a new competition instance
            _competition = new Competition("Race Championship", "Running", 1000);
            _competitionStatus = "Ready";
            _eventLog = "Competition created.\n";
            
            // Create doctors
            var teamDoctor = new TeamDoctor("Dr. Smith");
            var emergencyDoctor = new EmergencyDoctor("Dr. Johnson");
            var physiotherapist = new PhysiotherapistDoctor("Dr. Williams");
            
            // Add doctors to competition
            _competition.AddDoctor(teamDoctor);
            _competition.AddDoctor(emergencyDoctor);
            _competition.AddDoctor(physiotherapist);
            
            // Create some athletes
            AddAthlete("Alex", "USA", 28, 85, 95);
            AddAthlete("Boris", "Russia", 26, 90, 87);
            AddAthlete("Carlos", "Brazil", 24, 82, 93);
            AddAthlete("Dimitri", "France", 29, 88, 89);
            
            // Initialize Y positions for athletes
            InitializeAthletePositions();
            
            // Subscribe to competition events
            _competition.CompetitionStarted += OnCompetitionStarted;
            _competition.CompetitionEnded += OnCompetitionEnded;
            _competition.AthleteInjured += OnAthleteInjured;
            _competition.AthleteTreated += OnAthleteTreated;
            _competition.AthleteWon += OnAthleteWon;
            
            // Initialize commands
            StartCompetitionCommand = new RelayCommand(async () => await StartCompetitionAsync());
            StopCompetitionCommand = new RelayCommand(StopCompetition);
        }
        
        private void AddAthlete(string name, string country, int age, double speed, double stamina)
        {
            var athlete = new Athlete(name, "Running", age, country, speed, stamina);
            _competition.AddAthlete(athlete);
        }
        
        private void InitializeAthletePositions()
        {
            double laneHeight = TrackHeight / Math.Max(1, Athletes.Count);
            
            for (int i = 0; i < Athletes.Count; i++)
            {
                Athletes[i].Position = new Point(0, i * laneHeight + laneHeight / 2);
            }
        }

        private async Task StartCompetitionAsync()
        {
            if (_competition.IsCompetitionRunning)
                return;
                
            CompetitionStatus = "Running";
            AddToLog("Competition started!");
            
            try
            {
                await _competition.StartCompetitionAsync();
            }
            catch (Exception ex)
            {
                AddToLog($"Error: {ex.Message}");
            }
        }
        
        private void StopCompetition()
        {
            if (!_competition.IsCompetitionRunning)
                return;
                
            _competition.StopCompetition();
            CompetitionStatus = "Stopped";
            AddToLog("Competition stopped manually.");
        }
        
        private void OnCompetitionStarted(Competition competition)
        {
            AddToLog("The race has begun!");
        }
        
        private void OnCompetitionEnded(Competition competition)
        {
            CompetitionStatus = "Finished";
            AddToLog("The race has ended!");
        }
        
        private void OnAthleteInjured(Athlete athlete)
        {
            AddToLog($"{athlete.Name} got injured!");
        }
        
        private void OnAthleteTreated(Athlete athlete)
        {
            AddToLog($"{athlete.Name} has been treated and continues the race.");
        }
        
        private void OnAthleteWon(Athlete athlete)
        {
            AddToLog($"{athlete.Name} from {athlete.Country} has won the race!");
            AddToLog("Award ceremony is starting...");
            AddToLog($"Gold medal presented to {athlete.Name}!");
        }
        
        private void AddToLog(string message)
        {
            EventLog = $"[{DateTime.Now:HH:mm:ss}] {message}\n{EventLog}";
        }
        
        public void UpdateAthletePositions()
        {
            if (!_competition.IsCompetitionRunning)
                return;
                
            foreach (var athlete in Athletes)
            {
                if (!athlete.IsInjured)
                {
                    // Update X position based on progress
                    double x = athlete.Progress / TrackLength * TrackLength;
                    athlete.Position = new Point(x, athlete.Position.Y);
                }
            }
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Task3_9.Models;

namespace Task3_9.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<CompetitionViewModel> _competitions;
        private CompetitionViewModel _selectedCompetition;
        private Timer _updateTimer;
        
        public string Greeting => "Welcome to Sports Competition Simulator!";
        
        public ObservableCollection<CompetitionViewModel> Competitions
        {
            get => _competitions;
            set => this.RaiseAndSetIfChanged(ref _competitions, value);
        }
        
        public CompetitionViewModel SelectedCompetition
        {
            get => _selectedCompetition;
            set => this.RaiseAndSetIfChanged(ref _selectedCompetition, value);
        }
        
        public ICommand AddCompetitionCommand { get; }
        public ICommand RemoveCompetitionCommand { get; }
        
        public MainWindowViewModel()
        {
            _competitions = new ObservableCollection<CompetitionViewModel>();
            
            // Add an initial competition
            var initialCompetition = new CompetitionViewModel();
            _competitions.Add(initialCompetition);
            _selectedCompetition = initialCompetition;
            
            // Set up commands
            AddCompetitionCommand = ReactiveCommand.Create(AddCompetition);
            RemoveCompetitionCommand = ReactiveCommand.Create(RemoveSelectedCompetition);
            
            // Set up update timer
            _updateTimer = new Timer(16); // ~60fps
            _updateTimer.Elapsed += (s, e) => UpdatePositions();
            _updateTimer.AutoReset = true;
            _updateTimer.Start();
        }
        
        private void AddCompetition()
        {
            var newCompetition = new CompetitionViewModel();
            _competitions.Add(newCompetition);
            SelectedCompetition = newCompetition;
        }
        
        private void RemoveSelectedCompetition()
        {
            if (SelectedCompetition != null && Competitions.Count > 1)
            {
                int index = Competitions.IndexOf(SelectedCompetition);
                Competitions.Remove(SelectedCompetition);
                
                // Select another competition
                if (index >= Competitions.Count)
                    index = Competitions.Count - 1;
                    
                SelectedCompetition = Competitions[index];
            }
        }
        
        private void UpdatePositions()
        {
            foreach (var competition in Competitions)
            {
                competition.UpdateAthletePositions();
            }
        }
    }
}
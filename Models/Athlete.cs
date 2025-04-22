using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Task3_9.Models
{
    public class Athlete : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name { get; set; }
        public string Sport { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }

        private double _speed;
        public double Speed
        {
            get => _speed;
            set
            {
                if (_speed != value)
                {
                    _speed = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _stamina;
        public double Stamina
        {
            get => _stamina;
            set
            {
                if (_stamina != value)
                {
                    _stamina = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isInjured;
        public bool IsInjured
        {
            get => _isInjured;
            set
            {
                if (_isInjured != value)
                {
                    _isInjured = value;
                    OnPropertyChanged();
                    OnInjuryStatusChanged(this);
                }
            }
        }
        
        private Point _position;
        public Point Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _progress;
        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged();
                }
            }
        }

        // Event for injury
        public delegate void InjuryEventHandler(Athlete athlete);
        public event InjuryEventHandler? InjuryOccurred;
        public event InjuryEventHandler? InjuryHealed;

        public Athlete(string name, string sport, int age, string country, double speed, double stamina)
        {
            Name = name;
            Sport = sport;
            Age = age;
            Country = country;
            Speed = speed;
            Stamina = stamina;
            IsInjured = false;
            Progress = 0;
            Position = new Point(0, 0);
        }

        public void CheckForInjury(Random random, double injuryProbability)
        {
            if (!IsInjured && random.NextDouble() < injuryProbability)
            {
                IsInjured = true;
            }
        }

        public void RecoverFromInjury(double effectiveness)
        {
            if (IsInjured)
            {
                IsInjured = false;
            }
        }

        protected virtual void OnInjuryStatusChanged(Athlete athlete)
        {
            if (IsInjured)
            {
                InjuryOccurred?.Invoke(this);
            }
            else
            {
                InjuryHealed?.Invoke(this);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
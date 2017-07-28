using System;
using TiQWpfUtils.AbstractClasses;

namespace CoralTravelAnalyzer.Classes
{
    public class ListFilter : Notified
    {
        private Predicate<object> _filter;
        public Predicate<object> Filter {
            get => _filter;
            set
            {
                _filter = value;
                OnPropertyChanged();
            }
        }
    }
}

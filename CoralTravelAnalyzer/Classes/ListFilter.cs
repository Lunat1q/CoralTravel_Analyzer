using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiQWpfUtils.AbstractClasses;

namespace CoralTravelAnalyzer.Classes
{
    public class ListFilter : Notified
    {
        private const string FilterPropery = "Filter";
        private Predicate<object> _filter;
        public Predicate<object> Filter {
            get => _filter;
            set
            {
                _filter = value;
                OnPropertyChanged();
            }
        }

        public void TriggerFilter()
        {
            OnPropertyChanged(FilterPropery);
        }

    }
}

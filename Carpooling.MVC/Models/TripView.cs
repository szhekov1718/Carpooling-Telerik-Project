using System.Collections.Generic;

namespace Carpooling.MVC.Models
{
    public class TripView
    {
        public TripView()
        {
            tripViewModels = new List<TripViewModel>();
        }

        public TripView(TripView tripView)
        {
            tripViewModels = tripView.tripViewModels;
        }

        public TripView(ICollection<TripViewModel> tripViewModels)
        {
            this.tripViewModels = tripViewModels;
        }
        public ICollection<TripViewModel> tripViewModels { get; set; }
        public string SearchAttribute { get; set; }
        public string Value { get; set; }
    }
}

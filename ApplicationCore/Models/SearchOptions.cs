using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class SearchOptions
    {
        public string? Keyword { get; set; }
        public string SelectedOrderBy { get; set; }
        public string? SelectedPrice { get; set; }
        public string? SelectedTime { get; set; }
        public List<int>? SelectedThemes { get; set; }
        public List<string>? SelectedPlaces { get; set; }
        public int CurrentPage { get; set; }
    }
}

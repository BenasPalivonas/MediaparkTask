using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Entities
{
    public class HolidayType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Country_HolidayType> Country_HolidayTypes { get; set; }
        public List<Holiday> Holiday { get; set; }
    }
}

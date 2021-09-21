using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepartureBoard.Models
{
    public class DepartureBoardRecord
    {
        public DateTime Timestamp { get; set; }        
        public string Time { get; set; }
        public string Destination { get; set; }
        public string Train { get; set; }
        public string Track { get; set; }
        public string Status { get; set; }
        public string ScheduleId { get; set; }

    }
    class DepartureRecordEqualityComparer : IEqualityComparer<DepartureBoardRecord>
    {
        public bool Equals(DepartureBoardRecord r1, DepartureBoardRecord r2)
        {
            if (r2 == null && r1 == null)
                return true;
            else if (r1 == null || r2 == null)
                return false;
            else if (r1.ScheduleId == r2.ScheduleId)
                return true;
            else
                return false;
        }

        public int GetHashCode(DepartureBoardRecord r)
        {
            int hCode = (int) r.Timestamp.Ticks;
            return hCode.GetHashCode();
        }
    }
}

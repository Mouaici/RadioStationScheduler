using System;
using System.Collections.Generic;

namespace RadioStationScheduler.Data
{
    public class ScheduledEvent
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<string> Hosts { get; set; } = new();
        public List<string> Guests { get; set; } = new();
    }

    public class RescheduleRequest
    {
        public DateTime NewStartTime { get; set; }
        public DateTime NewEndTime { get; set; }
    }

    public class HostRequest
    {
        public string Host { get; set; } = "";
    }

    public class GuestRequest
    {
        public string Guest { get; set; } = "";
    }
}


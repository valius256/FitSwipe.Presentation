using System;

namespace FitSwipe.Shared.Dtos.Trainings
{
  public class GetUserRequestedScheduleDto
  {
    public Guid Id { get; set; } // Unique identifier for the schedule

    public DateTime StartDate { get; set; } // Start date of the schedule
    public DateTime EndDate { get; set; }   // End date of the schedule

    public int TotalSessions { get; set; }  // Total number of sessions
    public int TotalDuration { get; set; }  // Total duration in hours

    public string DateRange => $"{StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}";
  }
}

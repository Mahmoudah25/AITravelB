using AITravelB.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITravelB.Domain.Entities
{
    public class Activity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public ActivityType ActivityType { get; private set; }
        public decimal EstimatedCost { get; private set; }
        public string TimeSlot { get; private set; } = string.Empty;
        public string PlaceId { get;private set; } = string.Empty;
        public Guid TripId { get; private set; }
        public Trip Trip { get; private set; } = null!;
        private Activity() { }
        public Activity(string name, ActivityType activityType, decimal estimatedCost, string timeSlot, string placeId, Guid tripId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }
            if (estimatedCost < 0)
            {
                throw new ArgumentException("Estimated cost cannot be negative.");
            }
            if (string.IsNullOrWhiteSpace(timeSlot))
            {
                throw new ArgumentException("Time slot cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(placeId))
            {
                throw new ArgumentException("Place ID cannot be null or empty.");
            }
            if (tripId == Guid.Empty)
                throw new ArgumentException("TripId cannot be empty.", nameof(tripId));
            Id = Guid.NewGuid();
            Name = name;
            ActivityType = activityType;
            EstimatedCost = estimatedCost;
            TimeSlot = timeSlot;
            PlaceId = placeId;
            TripId = tripId;
        }
    }
}

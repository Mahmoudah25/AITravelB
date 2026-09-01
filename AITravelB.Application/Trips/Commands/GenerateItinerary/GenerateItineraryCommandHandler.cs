using AITravelB.Application.Common.DTOs;
using AITravelB.Application.Common.Interfaces;
using AITravelB.Domain.Entities;
using AITravelB.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AITravelB.Application.Trips.Commands.GenerateItinerary
{
    public class GenerateItineraryCommandHandler : IRequestHandler<GenerateItinerary, ItineraryResult>
    {
        private readonly IItineraryAiService apiservice;
        private readonly IWeatherService weatherService;
        private readonly IApplicationDbContext context;

        public GenerateItineraryCommandHandler(IItineraryAiService apiservice, IWeatherService weatherService, IApplicationDbContext context)
        {
            this.apiservice = apiservice;
            this.weatherService = weatherService;
            this.context = context;
        }

        public async Task<ItineraryResult> Handle(GenerateItinerary request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Handle called for TripId: {request.TripId} at {DateTime.UtcNow:HH:mm:ss.fff}");
            // 1. جيب الطقس الأول (لو فشل، نكمل من غيره)
            List<WeatherForecastDto>? weatherForecast = null;
            try
            {
                weatherForecast = await weatherService.GetForecastAsync(request.Destination, request.Days);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Weather service failed: {ex.Message}");
            }

            // 2. نادي الـ AI مرة واحدة بس، بالطقس (لو موجود)
            var itinerary = await apiservice.GenerateItineraryAsync(
                request.Destination, request.Days, request.Budget, weatherForecast);

            // 3. جيب الرحلة من الداتابيز
            var trip = await context.Trips
                .AnyAsync(t => t.Id == request.TripId, cancellationToken);

            if (!trip)
                throw new InvalidOperationException($"Trip with ID {request.TripId} not found.");

            // 4. احفظ كل نشاط، وحدّث الـ ActivityId في الـ response
            foreach (var day in itinerary.Days)
            {
                foreach (var activityPlan in day.Activities)
                {
                    var mappedType = MapToActivityType(activityPlan.Type);

                    var newActivity = new Activity(
                        activityPlan.PlaceName,
                        mappedType,
                        activityPlan.EstimatedCost,
                        activityPlan.Time,
                        "ai-generated",
                        request.TripId);

                    context.Activities.Add(newActivity);
                    activityPlan.ActivityId = newActivity.Id;
                }
            }

            // 5. احفظ مرة واحدة بعد كل الـ loops
            await context.SaveChangesAsync(cancellationToken);

            return itinerary;
        }

        private ActivityType MapToActivityType(string rawType)
        {
            return rawType.ToLower() switch
            {
                "food" or "meal" or "restaurant" => ActivityType.Restaurant,
                "hotel" => ActivityType.Hotel,
                _ => ActivityType.Attraction
            };
        }
    }
}
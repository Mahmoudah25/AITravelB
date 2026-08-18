using AITravelB.Application.Common.DTOs;
using AITravelB.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITravelB.Application.Trips.Commands.GenerateItinerary
{
    public class GenerateItineraryCommandHandler : IRequestHandler<GenerateItinerary, ItineraryResult>
    {
        private readonly IItineraryAiService apiservice;
        private readonly IWeatherService weatherService;
        public GenerateItineraryCommandHandler(IItineraryAiService apiservice, IWeatherService weatherService)
        {
            this.apiservice = apiservice;
            this.weatherService = weatherService;
        }
        public async Task<ItineraryResult> Handle(GenerateItinerary request, CancellationToken cancellationToken)
        {
            var weatherForecast = await weatherService.GetForecastAsync(request.Destination, request.Days);
            return await apiservice.GenerateItineraryAsync
                (
                request.Destination, request.Days,
                request.Budget ,weatherForecast);
        }
    }
}

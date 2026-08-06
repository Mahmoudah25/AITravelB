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
        public GenerateItineraryCommandHandler(IItineraryAiService apiservice)
        {
            this.apiservice = apiservice;
        }
        public async Task<ItineraryResult> Handle(GenerateItinerary request, CancellationToken cancellationToken)
        {

            return await apiservice.GenerateItineraryAsync(request.Destination, request.Days, request.Budget);
        }
    }
}

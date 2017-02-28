using System;

namespace Coolector.Services.Statistics.Shared.Dto
{
    public class RemarkStateDto
    {
        public string State { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public LocationDto Location { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
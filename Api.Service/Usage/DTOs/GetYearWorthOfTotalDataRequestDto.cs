namespace Api.Service.Usage.DTOs
{
    public class GetYearWorthOfTotalDataRequestDto
    {
        public Guid EnvironmentId { get; set; }
        public int year { get; set; }
    }
}

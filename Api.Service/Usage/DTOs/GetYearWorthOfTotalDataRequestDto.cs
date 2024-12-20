namespace Api.Service.Usage.DTOs
{
    public class GetYearWorthOfTotalDataRequestDto
    {
        public string Alias { get; set; }
        public int year { get; set; }
        public int Month { get; set; }
    }
}

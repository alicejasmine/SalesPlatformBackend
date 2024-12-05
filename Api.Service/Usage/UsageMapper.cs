using Api.Service.DTOs;
using Api.Service.Usage.DTOs;
using Domain.Entities;

namespace Api.Service.Controllers;

internal static class UsageMapper
{
    internal static UsageResponse MapEntityToResponse(UsageEntity entity, int year, int month)
    {
        return new UsageResponse
        {
            EnvironmentId = entity.EnvironmentId,
            Year = year,
            Month = month,
            TotalMonthlyBandwidth = entity.TotalMonthlyBandwidth,
            TotalMonthlyMedia = entity.TotalMonthlyMedia,
            DailyUsages = entity.Days.Select(d => new UsageResponse.DailyUsageResponse
            {
                BandwidthInBytes = d.Value.BandwidthInBytes,
                Hostnames = d.Value.Hostnames,
                ContentNodes = d.Value.ContentNodes,
                MediaSizeInBytes = d.Value.MediaSizeInBytes
            }).ToList()
        };
    }
}
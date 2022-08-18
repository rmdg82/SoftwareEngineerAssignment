using SoftwareEngineerAssignment.Api.Models;

namespace SoftwareEngineerAssignment.Api.Services;

public interface IAdviceSlipService
{
    Task<GiveMeAdviceResponse> GetAdviceSlip(GiveMeAdviceRequest request);
}
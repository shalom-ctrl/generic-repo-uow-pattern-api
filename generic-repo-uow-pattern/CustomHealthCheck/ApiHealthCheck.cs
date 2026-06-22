using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace generic_repo_uow_pattern.CustomHealthCheck
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;

        public ApiHealthCheck(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
           var response = await _httpClient.GetAsync("https://localhost:7024/api/ProductWithGenericRepo");

            if (response.IsSuccessStatusCode)
            {
                return await Task.FromResult(new HealthCheckResult(
                    status: HealthStatus.Healthy,
                    description: "The API is running"));
            }
            return await Task.FromResult(new HealthCheckResult(
                status: HealthStatus.Unhealthy,
                description: "The API is down."));
        }
    }
}

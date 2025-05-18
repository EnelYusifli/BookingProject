using Microsoft.AspNetCore.Http;

namespace BookingProject.Application.Services.Interfaces;

public interface ICloudinaryService
{
	Task<bool> FileDeleteAsync(string filePath);
	Task<string> FileCreateAsync(IFormFile file);
}

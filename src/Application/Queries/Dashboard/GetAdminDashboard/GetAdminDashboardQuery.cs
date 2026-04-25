using Application.DTOs.Dashboard;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Dashboard.GetAdminDashboard;

public class GetAdminDashboardQuery : IRequest<BaseResponse<AdminDashboardResponse>>
{
    public int Year { get; set; } = DateTime.UtcNow.Year;
}
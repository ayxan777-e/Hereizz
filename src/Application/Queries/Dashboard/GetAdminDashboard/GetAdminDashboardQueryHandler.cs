using Application.DTOs.Dashboard;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Application.Queries.Dashboard.GetAdminDashboard;

public class GetAdminDashboardQueryHandler
    : IRequestHandler<GetAdminDashboardQuery, BaseResponse<AdminDashboardResponse>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAdminDashboardQueryHandler(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        ICurrentUserService currentUserService)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponse<AdminDashboardResponse>> Handle(
        GetAdminDashboardQuery request,
        CancellationToken ct)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return BaseResponse<AdminDashboardResponse>.Fail(
                "Unauthorized",
                new List<string> { "User is not authenticated." },
                ErrorType.Unauthorized);
        }

        if (!_currentUserService.IsInRole("Admin"))
        {
            return BaseResponse<AdminDashboardResponse>.Fail(
                "Forbidden",
                new List<string> { "Only admins can access dashboard." },
                ErrorType.Forbidden);
        }
        var paymentsQuery = _paymentRepository
            .GetQueryable()
            .AsNoTracking()
            .Where(x => x.CreatedAt.Year == request.Year);

        var ordersQuery = _orderRepository
            .GetQueryable()
            .AsNoTracking()
            .Where(x => x.CreatedAt.Year == request.Year);
        var totalRevenue = await paymentsQuery
            .Where(x =>
             x.Status == PaymentStatus.Paid &&
             x.PaidAt.HasValue &&
             x.PaidAt.Value.Year == request.Year)
            .SumAsync(x => x.Amount, ct);

        var totalOrders = await ordersQuery.CountAsync(ct);

        var totalPayments = await paymentsQuery.CountAsync(ct);

        var paidPayments = await paymentsQuery
            .CountAsync(x => x.Status == PaymentStatus.Paid, ct);

        var pendingPayments = await paymentsQuery
            .CountAsync(x => x.Status == PaymentStatus.Pending, ct);

        var failedPayments = await paymentsQuery
            .CountAsync(x => x.Status == PaymentStatus.Failed, ct);

        var monthlyRevenueFromDb = await paymentsQuery
            .Where(x =>
                x.Status == PaymentStatus.Paid &&
                x.PaidAt.HasValue &&
                x.PaidAt.Value.Year == request.Year)
            .GroupBy(x => x.PaidAt!.Value.Month)
            .Select(g => new
            {
                Month = g.Key,
                Revenue = g.Sum(x => x.Amount)
            })
            .ToListAsync(ct);

        var monthlyRevenue = Enumerable.Range(1, 12)
            .Select(month =>
            {
                var item = monthlyRevenueFromDb.FirstOrDefault(x => x.Month == month);

                return new MonthlyRevenueResponse
                {
                    Month = month,
                    MonthName = CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(month),
                    Revenue = item?.Revenue ?? 0
                };
            })
            .ToList();

        var response = new AdminDashboardResponse
        {
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders,
            TotalPayments = totalPayments,
            PaidPayments = paidPayments,
            PendingPayments = pendingPayments,
            FailedPayments = failedPayments,
            MonthlyRevenue = monthlyRevenue
        };

        return BaseResponse<AdminDashboardResponse>.Ok(response, "Dashboard fetched successfully");
    }
}
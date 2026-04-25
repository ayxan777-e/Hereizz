namespace Application.DTOs.Dashboard;

public class AdminDashboardResponse
{
    public decimal TotalRevenue { get; set; }

    public int TotalOrders { get; set; }

    public int TotalPayments { get; set; }

    public int PaidPayments { get; set; }

    public int PendingPayments { get; set; }

    public int FailedPayments { get; set; }

    public List<MonthlyRevenueResponse> MonthlyRevenue { get; set; } = new();
}

public class MonthlyRevenueResponse
{
    public int Month { get; set; }

    public string MonthName { get; set; } = string.Empty;

    public decimal Revenue { get; set; }
}
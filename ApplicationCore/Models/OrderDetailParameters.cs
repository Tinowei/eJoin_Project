namespace ApplicationCore.Models;

public class OrderDetailParameters
{
    public int MemberId { get; set; }

    const int maxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 3;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
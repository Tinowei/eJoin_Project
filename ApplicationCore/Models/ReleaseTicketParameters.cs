namespace ApplicationCore.Models;

public class ReleaseTicketParameters
{
    public int MemberId { get; set; }

    const int maxPageSize = 10;
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
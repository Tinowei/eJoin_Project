namespace Admin.Models;

public static class GetStatusDescription
{
    public static string Description(int status)
    {
        switch (status)
        {
            case 1:
                return "草稿";
            case 2:
                return "上架";
            case 3:
                return "結束";
            default:
                return "下架";
        }
    }

    public static int ChangeDesToNumber(string status)
    {
        switch (status)
        {
            case "草稿":
                return 1;
            case "上架":
                return 2;
            case "結束":
                return 3;
            default:
                return 4;
        }
    }
}
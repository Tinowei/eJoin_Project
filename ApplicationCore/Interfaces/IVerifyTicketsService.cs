namespace ApplicationCore.Interfaces;

public interface IVerifyTicketsService
{
    
    //判斷是否有登錄
    bool IsLogin();
    //判斷是否是主辦方
    bool IsStaff(int hostId);
    //判斷庫存是否足夠扣
    bool IsTicketsEnough(int count);
    
}
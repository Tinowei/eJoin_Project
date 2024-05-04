using System.Transactions;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Services;

namespace Infrastructure.Data;

public class EfReleaseTicketRepository : EfRepository<ReleaseTicket>,IReleaseTicketRepository
{
    public EfReleaseTicketRepository(EJoinContext dbContext) : base(dbContext)
    {
    }

    public async Task<(List<ReleaseTicket>, List<string> ErrorMessages)> VerifyTickets(List<string> ticketNumbers)
    {
        
        var verifiedTickets = new List<ReleaseTicket>();
        var errorMessages = new List<string>();
        using var tran = DbContext.Database.BeginTransaction();
        try
        {
            var nowTime = CustomUtcNow.CurrentTime;
            foreach (var ticketNumber in ticketNumbers)
            {
                try
                {
                    var releaseTicket = DbContext.ReleaseTickets
                        .Include(rt => rt.Event)
                        .ThenInclude(e => e.Member)
                        .FirstOrDefault(rt => rt.ReleaseTicketNumber == ticketNumber && rt.Status == 0);

                    if (releaseTicket == null)
                    {
                        throw new KeyNotFoundException($"票券編號 {ticketNumber} 不存在或已使用！");
                    }

                    var hostName = releaseTicket.Event.Member.Name;

                    if (string.IsNullOrEmpty(hostName))
                    {
                        throw new InvalidOperationException($"找不到該活動主辦方");
                    }

                    // 進行變更
                    releaseTicket.Status = 1;
                    releaseTicket.ChangedTime = nowTime;
                    releaseTicket.Staff = hostName;

                    verifiedTickets.Add(releaseTicket);
                }
                catch (Exception ex)
                {
                    // 收集錯誤訊息
                    errorMessages.Add($"票券編號 {ticketNumber} 驗證失敗：{ex.Message}");
                }
            }

            // 一次更新所有有效的票券
            DbContext.ReleaseTickets.UpdateRange(verifiedTickets);
            await DbContext.SaveChangesAsync();
            tran.Commit();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            throw;
        }

        return (verifiedTickets, errorMessages);
    }

    public ReleaseTicket VerifyTicket(string ticketNumber)
    {
        using var tran = DbContext.Database.BeginTransaction();
        try
        {
            var releaseTicket = DbContext.ReleaseTickets
                .Include(rt => rt.Event)
                .ThenInclude(e => e.Member)
                .FirstOrDefault(rt => rt.ReleaseTicketNumber == ticketNumber && rt.Status == 0);
            //如果該票券編號已使用過，releaseTicket會是Null
            
            if (releaseTicket == null)
            { 
                throw new KeyNotFoundException("票券不存在或已使用！");
            }
            
            var hostName = releaseTicket.Event.Member.Name;
            
            if (string.IsNullOrEmpty(hostName))
            { 
                throw new InvalidOperationException("找不到該活動主辦方");
            }
            //進行變更
            releaseTicket.Status = 1;
            releaseTicket.ChangedTime= CustomUtcNow.CurrentTime;
            releaseTicket.Staff = hostName;
            
            DbContext.ReleaseTickets.Update(releaseTicket);
            DbContext.SaveChanges();
            tran.Commit();
            return releaseTicket;
        }
        catch(Exception ex)
        {
            tran.Rollback();
            throw ;
        }
    }

}
namespace ApplicationCore.Extensions;

public interface IQrCodeHelper
{
    byte[] GenerateQrCodeByQuantity(IEnumerable<string> releaseTicketNumbers);
}
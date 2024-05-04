using QRCoder;
using System.IO;
using ApplicationCore.Extensions;

namespace Infrastructure.Services;

public class QrCodeHelper : IQrCodeHelper
{
    private readonly QRCodeGenerator _qrCodeGenerator;
    public QrCodeHelper()
    {
        _qrCodeGenerator = new QRCodeGenerator();
    }
    public byte[] GenerateQrCodeByQuantity(IEnumerable<string> releaseTicketNumbers)
    {
        
        var qrCodeData = $"{string.Join("$",releaseTicketNumbers)}";
        
        QRCodeData qrCodedata = _qrCodeGenerator.CreateQrCode(qrCodeData,QRCodeGenerator.ECCLevel.Q);
        //方法結束自動釋放
        using PngByteQRCode qrCode = new PngByteQRCode(qrCodedata);
        //GetGraphic方法用來產出圖片
        byte[] image = qrCode.GetGraphic(30);
        
        return image;
    }
    
}
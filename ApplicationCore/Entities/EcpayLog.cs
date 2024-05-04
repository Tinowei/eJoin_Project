using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class EcpayLog
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 對應到的OrderId
    /// </summary>
    public int RelateOrderId { get; set; }

    /// <summary>
    /// 傳送給綠界，特店交易編號
    /// </summary>
    public string MerchantTradeNo { get; set; } = null!;

    /// <summary>
    /// 傳送給綠界，交易金額
    /// </summary>
    public int TotalAmount { get; set; }

    /// <summary>
    /// 傳送給綠界，商品名稱
    /// </summary>
    public string ItemName { get; set; } = null!;

    /// <summary>
    /// 傳送給綠界，特店交易時間
    /// </summary>
    public DateTime MerchantTradeDate { get; set; }

    /// <summary>
    /// 傳送給綠界，檢查碼
    /// </summary>
    public string CheckMacValue { get; set; } = null!;

    /// <summary>
    /// 綠界回傳值，交易狀態
    /// </summary>
    public int? RtnCode { get; set; }

    /// <summary>
    /// 綠界回傳值，交易訊息
    /// </summary>
    public string? RtnMsg { get; set; }

    /// <summary>
    /// 綠界回傳值，綠界交易編號
    /// </summary>
    public string? TradeNo { get; set; }

    /// <summary>
    /// 綠界回傳值，付款時間
    /// </summary>
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// 綠界回傳值，訂單成立時間
    /// </summary>
    public DateTime? TradeDate { get; set; }
}

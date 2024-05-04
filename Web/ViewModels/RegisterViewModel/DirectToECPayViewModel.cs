namespace Web.ViewModels.RegisterViewModel
{
    public class DirectToEcpayViewModel
    {
        public string MerchantID { get; set; }
        public string MerchantTradeNo { get; set; }
        public string MerchantTradeDate { get; set; }
        public string PaymentType { get; set; }
        public int TotalAmount { get; set; }
        public string TradeDesc { get; set; }
        public string ItemName { get; set; }
        public string ReturnURL { get; set; }
        public string ChoosePayment { get; set; }
        public string CheckMacValue { get; set; }
        public int EncryptType { get; set; }
        public string OrderResultURL { get; set; }
        public string CustomField1 { get; set; }
    }
}

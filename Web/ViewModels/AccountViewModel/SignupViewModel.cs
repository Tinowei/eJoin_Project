namespace Web.ViewModels.AccountViewModel
{
    public class SignupViewModel
    {
        public string Email { get; set; }
        public string verificationCode { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Phonenumber { get; set; }
        public int Status { get; set; } = 0;
    }
}

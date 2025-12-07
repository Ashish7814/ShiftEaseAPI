namespace ShiftEaseAPI.Service.Account.DTOs
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }        // encoded token from email link
        public string NewPassword { get; set; }
    }
}

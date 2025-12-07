namespace ShiftEaseAPI.Service.Account.DTOs
{
    public class LoginResponseDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Role { get; set; }
        public bool Succeeded { get; set; }
        public string UserId { get; set; }
    }
}

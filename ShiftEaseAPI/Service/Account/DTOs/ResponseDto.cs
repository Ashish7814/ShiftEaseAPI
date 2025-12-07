namespace ShiftEaseAPI.Service.Account.DTOs
{
    public class ResponseDto
    {
        public string Status { get; set; }        // "Success" / "Error"
        public string Message { get; set; }
        public bool Succeeded { get; set; }
        public string UserId { get; set; }        // created user's id
        public string EncodedToken { get; set; }
    }
}

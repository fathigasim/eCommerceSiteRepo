namespace efcoreApi.Models
{
    public class Register
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string role { get; set; }

        public Register()
        {
            Id =Guid.NewGuid().ToString().Substring(1,10);
        }
    }
}

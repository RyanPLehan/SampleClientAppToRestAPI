using System.Text;

namespace Sample.Client.App.Domain.DTO
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Id: {Id} - Name: {Name} - Email: {Email}");
            return sb.ToString();
        }
    }
}

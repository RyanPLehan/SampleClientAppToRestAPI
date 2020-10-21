using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Client.App.Domain.DTO
{
    public class CustomerStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Id: {Id} - Name: {Name}");
            return sb.ToString();
        }
    }
}

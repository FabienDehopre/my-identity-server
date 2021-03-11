namespace Dehopre.Sso.Domain.Models
{
    using System;
    using Dehopre.Domain.Core.Models;

    public class Template : Entity
    {
        public Template() { }

        public Template(string content, string subject, string name, string username)
        {
            this.Id = Guid.NewGuid();
            this.Content = content;
            this.Subject = subject;
            this.Name = name;
            this.Username = username;
        }

        public string Subject { get; set; }
        public string Content { get; private set; }
        public string Name { get; private set; }

        public string Username { get; private set; }
        public DateTime Created { get; private set; } = DateTime.UtcNow;
        public DateTime Updated { get; private set; } = DateTime.UtcNow;

        public void UpdateTemplate(string content, string subject, string name, string username)
        {
            this.Subject = subject;
            this.Content = content;
            this.Name = name;
            this.Username = username;
            this.Updated = DateTime.UtcNow;
        }
    }
}

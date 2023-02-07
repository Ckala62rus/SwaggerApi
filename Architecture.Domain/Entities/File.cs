using System;

namespace Architecture.Domain.Entities
{
    public class File
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string AbsolutePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

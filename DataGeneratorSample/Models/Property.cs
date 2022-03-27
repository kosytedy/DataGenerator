using System;
using System.ComponentModel.DataAnnotations;

namespace DataGeneratorSample.Models
{
    public class Property
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public Property()
        {
        }
    }
}

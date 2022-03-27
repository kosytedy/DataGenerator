using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataGeneratorSample.Models
{
    public class Model
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Endpoint { get; set; }

        public List<Model> Models { get; set; }

        public List<Property> Properties { get; set; }

        public Model()
        {
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZealandIdScanner.Models
{
    public class Sensors
    {
        [Key]
        [Required]
        public int SensorId { get; set; }
        [Required]
        [StringLength(50)]
        public string Navn { get; set; }

        public Sensors(string navn, int lokaleId)
        {
            Navn = navn;
        }

        public Sensors()
        {

        }

        public void ValidateNavn()
        {
            if (Navn == null)
            {
                throw new ArgumentNullException("Navn må ikke være null");
            }
            if (Navn.Length < 5)
            {
                throw new ArgumentOutOfRangeException("Navn skal mindst være på 5 karakterer");
            }
        }

        public void Validate()
        {
            ValidateNavn();
        }

        public override string ToString()
        {
            return $"{{{nameof(SensorId)}={SensorId.ToString()}, {nameof(Navn)}={Navn}={Navn.ToString()}}}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Sensors sensors &&
                   SensorId == sensors.SensorId &&
                   Navn == sensors.Navn;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}

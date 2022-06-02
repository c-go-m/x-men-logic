
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    [Table("testDNA")]
    [Serializable]
    public class DnaMutantEntity: EntityBase
    {
        public string Data { get; set; }
        public bool IsMutant { get; set; }

    }
}

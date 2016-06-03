namespace ASW.Framework.Core
{
    using Orleans;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Threading.Tasks;
    using System.Data.Entity;
    using System.Linq;

    [Serializable]
    [Table("cards")]
    public partial class Card
    {
        [Key]
        [StringLength(20)]
        public string Id { get; set; }

        [StringLength(50)]
        public string Holder { get; set; }

        [Required]
        [StringLength(1)]
        public string CardType { get; set; }

        [StringLength(30)]
        public string CardNumber { get; set; }

        [StringLength(2)]
        public string ExpiryMonth { get; set; }

        [StringLength(2)]
        public string ExpiryYear { get; set; }

        [StringLength(3)]
        public string SecureCode { get; set; }

        [NotMapped]
        public string CardTypeName
        {
            get{
                if ("M".Equals(CardType))
                {
                    return "MASTERCARD";
                }else
                {
                    return "VISA";
                }
            }
        }
    }
}

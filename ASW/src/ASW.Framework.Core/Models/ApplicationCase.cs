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
    [Table("application_cases")]
    public partial class ApplicationCase
    {
        public class SchemeTypes
        {
            private SchemeTypes()
            {

            }

            public const string WHVCN = "WHVCN";
            public const string SFV = "SFV";
        }


        [Key]
        [StringLength(50)]
        public string Id { get; set; }

        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(255)]
        public string AppId { get; set; }

        [StringLength(50)]
        public string InitState { get; set; }

        [StringLength(50)]
        public string InitPayState { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string PayState { get; set; }

        [Column("Scheme")]
        [StringLength(20)]
        public string SchemeType { get; set; }

        public string CardId { get; set; }

        public bool Enabled { get; set; }

        public DateTime UpdatedTime { get; set; }

        [NotMapped]
        public Card Card{get;set;}

        //Incomplete, Completed pending submission, Submitted
        //Pending, Received

        [NotMapped]
        public bool HasStatus
        {
            get
            {
                return !string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(PayState);
            }
        }

        [NotMapped]
        public bool CanSubmit
        {
            get
            {
                return "Completed pending submission".Equals(State);
            }
        }

        [NotMapped]
        public bool HasSubmitted
        {
            get
            {
                return "Submitted".Equals(PayState);
            }
        }

        [NotMapped]
        public bool CanPay
        {
            get
            {
                return "Submitted".Equals(State) && "Pending".Equals(PayState);
            }
        }

        [NotMapped]
        public bool HasPaid
        {
            get
            {
                return "Received".Equals(PayState);
            }
        }
    }
}

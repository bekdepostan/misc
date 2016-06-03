namespace PowerBallNZ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Draw
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Number { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(20)]
        public string LottoStr { get; set; }

        public int Lotto1 { get; set; }

        public int Lotto2 { get; set; }

        public int Lotto3 { get; set; }

        public int Lotto4 { get; set; }

        public int Lotto5 { get; set; }

        public int Lotto6 { get; set; }

        [StringLength(12)]
        public string Bonus { get; set; }

        [StringLength(12)]
        public string StrikeStr { get; set; }

        public int? Strike1 { get; set; }

        public int? Strike2 { get; set; }

        public int? Strike3 { get; set; }

        public int? Strike4 { get; set; }

        public int? PowerBall { get; set; }

        public static Draw Create(int number, DateTime date, string lottoStr, int bonus, string strikeStr, int powerBall)
        {
            return new Draw
            {

            };
        }

        public override string ToString()
        {
            return string.Format("#{0}\t{1:dd MMM,yyyy} {2} | {3} | {4} | {5}", Number, Date, LottoStr, Bonus, StrikeStr, PowerBall);
        }
    }
}

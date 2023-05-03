namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using static System.Collections.Specialized.BitVector32;

    [Table("User")]
    public partial class User
    {
        public long ID { get; set; }

        [StringLength(50)]
        [Display(Name = "T\u00e0i kho\u1ea3n")]
        public string UserName { get; set; }

        [StringLength(32)]
        [Display(Name = "M\u1eadt kh\u1ea9u")]
        public string PassWord { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên")]
        public string Name { get; set; }
        [StringLength(20)]

        [Display(Name = "\u0050\u0068\u00e2\u006e \u0071\u0075\u0079\u1ec1\u006e")]
        public string GroupID { set; get; }

        [StringLength(50)]
        [Display(Name = "\u0110\u1ecba ch\u1ec9")]
        public string Address { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "S\u1ed1 \u0111i\u1ec7n tho\u1ea1i")]
        public string Phone { get; set; }
        public int? ProvinceID { get; set; }
        public int? DistrictID { get; set; }
        public int? PrecinctID { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Tr\u1ea1ng th\u00e1i")]
        public bool Status { get; set; }

    }
}

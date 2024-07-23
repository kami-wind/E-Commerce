using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Category; 

public class POST_PUT_Category
{
    [Required(ErrorMessage ="Name is Required")]
    public string Name { get; set; }
}

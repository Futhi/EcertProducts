using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            Name= string.Empty;
            CategoryCode= string.Empty;
        }
        public int Id { get; set; }
        [DisplayName("Category Name")]
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public DateTime UpdateDate { get; set; }

        [CodeAttributes(6,ErrorMessage ="Invalid category code")]
        [StringLength(6)]
        [DisplayName("Category Code")]
        [Required]      
        public string CategoryCode { get; set; }

    }


    public class CodeAttributes : ValidationAttribute
    {
        private readonly int _maxWords;
        public CodeAttributes(int maxWords) : base("{0} has too many characters.")
        {
            _maxWords = maxWords;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var textValue = value.ToString();

            if (!string.IsNullOrEmpty(textValue))
            { 
            int nums = 0, str = 0;
             foreach (char item in textValue)
            {
                if (char.IsDigit(item))
                {
                    nums++;
                    if (nums > 3)
                    {
                        return new ValidationResult("Incorrect Code, only 3 numbers allowed!");
                    }
                }
                else if (char.IsLetter(item))
                {
                    str++;
                    if (str > 3)
                    {
                        return new ValidationResult("Incorrect Code, only 3 letters allowed!");
                    }
                }
            }
            }
            

           

            return ValidationResult.Success;
        }
    }
}

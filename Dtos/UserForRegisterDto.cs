using System.ComponentModel.DataAnnotations;

namespace OnlineShopping.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength=4,ErrorMessage="You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
    }
}
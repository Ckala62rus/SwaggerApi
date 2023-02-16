using System.ComponentModel.DataAnnotations;

namespace Architecture.Models
{
    #region JsonProperties
    /// <summary>
    /// Json Properties
    /// </summary>
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    #endregion
}

using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record UserRequestDto : IRequestDto
    {
        [Required]
        [MinLength(3)]
        public string Username { get; init; }
        
        public string? FirstName { get; init; }
        
        public string? LastName { get; init; }

        public bool Enabled { get; init; }
        
        [Required]
        [EmailAddress]
        public string Email { get; init; }



    }
}

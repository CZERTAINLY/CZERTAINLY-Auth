using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Czertainly.Auth.Common.Models.Dto
{
    public interface IUpdateRequestDto : ICrudRequestDto
    {
        public long Id { get; }

        public Guid Uuid { get; }
    }
}

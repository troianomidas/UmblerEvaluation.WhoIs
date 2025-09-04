using AutoMapper;

namespace Umbler.WhoIs.Application.UseCases.Domain.Create;

/// <summary>
/// AutoMapper profile for CreateDomainCommand and related DTOs.
/// </summary>
public sealed class CreateDomainCommandProfile : Profile
{
    /// <summary>
    /// Configures mappings between command, entity, and response DTO.
    /// </summary>
    public CreateDomainCommandProfile()
    {
        CreateMap<CreateDomainCommand, WhoIs.Domain.Entities.Domain>();
        CreateMap<WhoIs.Domain.Entities.Domain, CreateDomainCommandResponse>();
    }
}
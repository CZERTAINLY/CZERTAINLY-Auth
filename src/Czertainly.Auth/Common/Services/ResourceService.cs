using AutoMapper;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Models.Entities;
using Czertainly.Auth.Data.Contracts;

namespace Czertainly.Auth.Common.Services
{
    public abstract class ResourceService<TEntity, TResourceDto> : IResourceService<TResourceDto>
        where TEntity : class, IBaseEntity, new()
        where TResourceDto : IResourceDto, new()
    {
        protected readonly IMapper _mapper;
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IRepositoryManager _repositoryManager;

        public ResourceService(IRepositoryManager repositoryManager, IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _repositoryManager = repositoryManager;
        }
        public virtual async Task<PagedResponse<TResourceDto>> GetAsync(IQueryRequestDto dto)
        {
            var queryParams = _mapper.Map<QueryStringParameters>(dto);
            var users = await _repository.GetAllAsync(queryParams);

            return new PagedResponse<TResourceDto>
            {
                Data = _mapper.Map<List<TResourceDto>>(users),
                Links = _mapper.Map<PagingMetadata>(users),
            };
        }

        public virtual async Task<TResourceDto> CreateAsync(IRequestDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _repository.Create(entity);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<TResourceDto>(entity);
        }

        public virtual async Task<TResourceDto> GetDetailAsync(IEntityKey key)
        {
            var entity = await _repository.GetByIdAsync(key);

            return _mapper.Map<TResourceDto>(entity);
        }

        public virtual async Task<TResourceDto> UpdateAsync(IEntityKey key, IRequestDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            if (key.Uuid.HasValue) entity.Uuid = key.Uuid.Value;

            await _repository.Update(key, entity);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<TResourceDto>(entity);
        }

        public virtual async Task DeleteAsync(IEntityKey key)
        {
            await _repository.Delete(key);
            await _repositoryManager.SaveAsync();
        }
    }
}

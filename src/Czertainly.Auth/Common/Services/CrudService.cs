using AutoMapper;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Models.Entities;
using Czertainly.Auth.Data.Contracts;

namespace Czertainly.Auth.Common.Services
{
    public abstract class CrudService<TEntity, TResponseDto, TDetailResponseDto> : ICrudService<TResponseDto, TDetailResponseDto>
        where TEntity : class, IBaseEntity, new()
        where TResponseDto : ICrudResponseDto, new()
        where TDetailResponseDto : ICrudResponseDto, new()
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IRepositoryManager _repositoryManager;

        public CrudService(IRepositoryManager repositoryManager, IBaseRepository<TEntity> repository, IMapper mapper, ILogger logger)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
            _repositoryManager = repositoryManager;
        }
        public virtual async Task<PagedResponse<TResponseDto>> GetAsync(IQueryRequestDto dto)
        {
            var queryParams = _mapper.Map<QueryStringParameters>(dto);
            var users = await _repository.GetAllAsync(queryParams);

            return new PagedResponse<TResponseDto>
            {
                Data = _mapper.Map<List<TResponseDto>>(users),
                Links = _mapper.Map<PagingMetadata>(users),
            };
        }

        public virtual async Task<TDetailResponseDto> CreateAsync(ICrudRequestDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _repository.Create(entity);
            await _repositoryManager.SaveAsync();

            entity = await _repository.GetByKeyAsync(entity.Uuid);
            return _mapper.Map<TDetailResponseDto>(entity);
        }

        public virtual async Task<TDetailResponseDto> GetDetailAsync(Guid key)
        {
            var entity = await _repository.GetByKeyAsync(key);

            return _mapper.Map<TDetailResponseDto>(entity);
        }

        public virtual async Task<TDetailResponseDto> UpdateAsync(Guid key, ICrudRequestDto dto)
        {
            var entity = await _repository.GetByKeyAsync(key);
            _mapper.Map(dto, entity);

            //await _repository.UpdateAsync(key, entity);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<TDetailResponseDto>(entity);
        }

        public virtual async Task DeleteAsync(Guid key)
        {
            await _repository.DeleteAsync(key);
            await _repositoryManager.SaveAsync();
        }
    }
}

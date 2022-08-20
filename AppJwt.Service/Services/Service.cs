using AppJwt.Core.Repositories;
using AppJwt.Core.Services;
using AppJwt.Core.UnitOfWork;
using AppJwt.Service.Mappers;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppJwt.Service.Services
{
    public class Service<T, TDto> : IService<T, TDto> where T : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<T> _repository;

        public Service(IUnitOfWork unitOfWork, IRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            // Gelen dto nesnesi entity'e çevrilir
            var newEntity = ObjectMapper.Mapper.Map<T>(entity);
            await _repository.AddAsync(newEntity);

            await _unitOfWork.CommitAsync();

            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return Response<TDto>.Success(newDto, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            var dtoResult = ObjectMapper.Mapper.Map<List<TDto>>(result);
            return Response<IEnumerable<TDto>>.Success(dtoResult, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            // data yok ise
            if (entity == null)
            {
                return Response<TDto>.Fail("Not Found", 404, true);
            }
            var dtoEntity = ObjectMapper.Mapper.Map<TDto>(entity);
            return Response<TDto>.Success(dtoEntity, 200);

        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            var isExistEntity = await _repository.GetByIdAsync(id);
            if (isExistEntity == null)
            {
                return Response<NoDataDto>.Fail("id not found", 404, true);
            }
            _repository.Remove(isExistEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<NoDataDto>> Update(TDto entity, int id)
        {
            var isExistEntity = await _repository.GetByIdAsync(id);
            if (isExistEntity == null)
            {
                return Response<NoDataDto>.Fail("id not found", 404, true);
            }
            var updateEntity = ObjectMapper.Mapper.Map<T>(entity);
            _repository.Update(updateEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<T, bool>> predicate)
        {
            var data = await _repository.Where(predicate).ToListAsync();
            var TDto = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(data);
            return Response<IEnumerable<TDto>>.Success(TDto, 200);


        }
    }
}

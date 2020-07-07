using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReaderService : IReaderService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public ReaderService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task AddAsync(ReaderModel model)
        {
            var reader = _mapper.Map<Reader>(model);

            await _unit.ReaderRepository.AddAsync(reader);
            await _unit.SaveAsync();
        }

        public async Task UpdateAsync(int modelId, ReaderModel model)
        {
            var reader = _mapper.Map<Reader>(model);

            _unit.ReaderRepository.Update(reader);
            await _unit.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unit.ReaderRepository.DeleteById(modelId);
            await _unit.SaveAsync();
        }

        public async Task<ReaderModel> GetByIdAsync(int id)
        {
            var readers = await _unit.ReaderRepository.GetByIdWithDetails(id);
            return _mapper.Map<ReaderModel>(readers);
        }

        public IEnumerable<ReaderModel> GetAll()
        {
            var readers = _unit.ReaderRepository.FindAll().ToList();
            return _mapper.Map<IEnumerable<ReaderModel>>(readers);
        }

        public IEnumerable<ReaderModel> GetReadersThatDontReturnBooks()
        {
            var listOfCards = _unit.HistoryRepository
                .FindAll()
                .Where(history => history.ReturnDate == null)
                .Select(history => history.CardId);
            var listOfReaders = _unit.CardRepository
                .FindAll()
                .Join(listOfCards,
                    card => card.Id,
                    history => history,
                    (card, history) => card.ReaderId);
            return _mapper.Map<IEnumerable<ReaderModel>>(listOfReaders);
        }      
    }
}

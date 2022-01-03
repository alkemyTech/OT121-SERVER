using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Mapper;
using OngProject.Infrastructure.Repositories.IRepository;
using System.Threading.Tasks;
using OngProject.Core.Helper.Pagination;
using System;
using System.Linq;
using OngProject.Core.DTOs.NewsDTOs;

namespace OngProject.Core.Services
{
    public class NewsServices : INewsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUriService _uriService;
        private readonly IImageService _imageServices;
        private readonly EntityMapper _entityMapper;
        public NewsServices(IUnitOfWork unitOfWork, IImageService imageServices, IUriService uriService)
        {
            _unitOfWork = unitOfWork;
            _imageServices = imageServices;
            _uriService = uriService;
            _entityMapper = new EntityMapper();
        }

        public async Task<bool> NewsExistsById(int id)
        {
            var existsNews = await _unitOfWork.NewsRepository.GetById(id);
            if (existsNews != null)
                return true;
            return false;
        }
        public async Task<Comments[]> GetAllCommentsByNews(int id)
        {
            var news = _unitOfWork.NewsRepository.EntityExists(id);
            if (!news)
                return null;

            return (await _unitOfWork.CommentsRepository.GetAll())
                .Where(n => n.NewId == id).ToArray();
        }
        #region Get all data from news by Id
        public async Task<News> GetAllDataByIdAsync(int id)
        {
            var news = await _unitOfWork.NewsRepository.FindByCondition(x => x.Id == id, y => y.Category);
            return news.FirstOrDefault();
        }
        #endregion

        #region create news by Post
        public async Task<News> CreateAsync(NewsCreateDTO newsDto)
        {
            var urlImage = await _imageServices.SaveImageAsync($"{Guid.NewGuid()}_{newsDto.Image.FileName}",newsDto.Image);
            var news = _entityMapper.FromNewsCreateDTOtoNews(newsDto,urlImage);
            news = await _unitOfWork.NewsRepository.Insert(news);
            await _unitOfWork.SaveChangesAsync();
            return news;
        }
        #endregion

        #region Update all data from news by PUT
        public async Task<News> UpdatePutAsync(NewsUpdateDTO newsUpdateDto)
        {
            var news = await _unitOfWork.NewsRepository.GetById(newsUpdateDto.Id);
            if (news != null)
            {
                _unitOfWork.DiscardChanges();
                await _imageServices.Delete(news.Image);
                var urlImage = await _imageServices.SaveImageAsync($"{Guid.NewGuid()}_{newsUpdateDto.Image.FileName}", newsUpdateDto.Image);

                news = _entityMapper.FromNewsUpdateDTOtoNews(newsUpdateDto, urlImage);
                try
                {
                    await _unitOfWork.NewsRepository.Update(news);
                    await _unitOfWork.SaveChangesAsync();
                    return news;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }
        #endregion
    }
}

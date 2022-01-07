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
using System.Collections.Generic;
using OngProject.Core.DTOs.CommentsDTOs;

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

        public async Task<ResultValue<PaginationDTO<CommentResponseDTO>>> GetAllCommentsUsingPaging(int id, int page)
        {
            var exists = _unitOfWork.NewsRepository.EntityExists(id);
            if(!exists)
            {
                return new ResultValue<PaginationDTO<CommentResponseDTO>>(){
                    Messages = new List<string> () {$"La entidad con Id {id} no existe"},
                    HasErrors = true,
                    StatusCode = 400
                };
            }

            var newsCount = (await _unitOfWork.CommentsRepository.GetAll())
                .Where(n => n.NewId == id).Count();

            int totalPages = (newsCount/10) + 1;
            if(newsCount == 0 || totalPages < page || page < 1)
            {
                return new ResultValue<PaginationDTO<CommentResponseDTO>>(){
                    Messages = new List<string> () {$"La página número {page} no existe"},
                    HasErrors = true,
                    StatusCode = 400
                };
            }
            
            IEnumerable<Comments> pageContent = await _unitOfWork
                .CommentsRepository.GetPageAsync(c => c.Id, 10, page);
            List<CommentResponseDTO> dtosList = pageContent
                .Where(c => c.NewId == id)
                .Select(c => _entityMapper.FromCommentsToCommentResponseDTO(c))
                .ToList();

            string nextPage = "", prevPage= "";
            
            if(page < totalPages)
               nextPage = _uriService.GetPage("/news", page+1);
            if(page > 1)
               prevPage = _uriService.GetPage("/news", page-1);
            
            var result = new PaginationDTO<CommentResponseDTO>(){
                CurrentPage = page,
                TotalItems = newsCount,
                TotalPages = totalPages,
                PrevPage = prevPage,
                NextPage = nextPage,
                Items = dtosList
            };

            return new ResultValue<PaginationDTO<CommentResponseDTO>>(){
                Value =  result,
                HasErrors = false,
                StatusCode = 200
            };
        }

        #endregion
    }
}

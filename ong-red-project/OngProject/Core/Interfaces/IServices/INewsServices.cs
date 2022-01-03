using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.NewsDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
using System;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface INewsServices
    {
        Task<bool> NewsExistsById(int id);
        Task<Comments[]> GetAllCommentsByNews(int id);
        Task<News> GetAllDataByIdAsync(int id);
        Task<News> CreateAsync(NewsCreateDTO newsDto);
        Task<News> UpdatePutAsync(NewsUpdateDTO newsUpdateDto);
    }
}

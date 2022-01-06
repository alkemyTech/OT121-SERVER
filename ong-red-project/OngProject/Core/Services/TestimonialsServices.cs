using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.TestimonialsDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Helper.S3;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Mapper;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class TestimonialsServices : ITestimonialsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUriService _uriService;
        private readonly IImageService _imageServices;
        private readonly EntityMapper _mapper;

        public TestimonialsServices(IUnitOfWork unitOfWork, IImageService imageServices, IUriService uriService)
        {
            _uriService = uriService;
            _unitOfWork = unitOfWork;
            _imageServices = imageServices;
            _mapper = new EntityMapper();
        }

        public async Task<Result> CreateAsync(TestimonialsCreateDTO testimonialsCreate)
        {
            var newRecord = new Testimonials
            {
                Name = testimonialsCreate.Name,
                Content = testimonialsCreate.Content
            };

            if (testimonialsCreate.Image != null)
            {
                try
                {
                    var nameImage = Guid.NewGuid();
                    await _imageServices.Save(nameImage.ToString(), testimonialsCreate.Image);
                    newRecord.Image = nameImage.ToString();
                }
                catch (Exception e)
                {
                    return new Result().Fail(e.Message);
                }
            }

            await _unitOfWork.TestimonialsRepository.Insert(newRecord);

            await _unitOfWork.SaveChangesAsync();

            return new Result().Success("Datos guardados satisfactoriamente.");
        }

        public async Task<Result> UpdateAsync(TestimonialsUpdateDTO testimonialsUpdate)
        {
            var testimonial = await _unitOfWork.TestimonialsRepository.GetById(testimonialsUpdate.Id);

            if (testimonial == null)
                return new Result().Fail($"El registro {testimonialsUpdate.Id} no fue encontrado.");

            if (testimonialsUpdate.Image != null)
            {
                try
                {
                    await _imageServices.Save(testimonial.Image, testimonialsUpdate.Image);
                }
                catch (Exception e)
                {
                    return new Result().Fail(e.Message);
                }
            }

            testimonial.Name = testimonialsUpdate.Name;
            testimonial.Content = testimonialsUpdate.Content;

            var result = await _unitOfWork.TestimonialsRepository.Update(testimonial);

            await _unitOfWork.SaveChangesAsync();

            if (result == null)
            {
                return new Result().Fail("Ocurrio un problema al intentar actualizar el testimonio.");
            }

            return new Result().Success("Testimonio actualizado con éxito.");
        }

        #region Testimonials Get by paging
        public async Task<PaginationDTO<TestimonialsDTO>> GetByPagingAsync(int page, int quantity)
        {
            var prevPage = string.Empty;
            var nextPage = string.Empty;
            var count = await _unitOfWork.TestimonialsRepository.CountAsync();
            var totalPages = (int)Math.Floor((decimal)count / quantity);
            if ((count % quantity) > 0)
                totalPages++;

            if (page > totalPages || page == 1)
                page = 1;
            else
                prevPage = _uriService.GetPage("/Testimonials", page - 1);

            if (page < totalPages)
                nextPage = _uriService.GetPage("/Testimonials", page + 1);

            var testimonialsList = await _unitOfWork.TestimonialsRepository.GetPageAsync(x => x.Id > 0, quantity, page);
            var testimonialDto = testimonialsList.Select(x => _mapper.FromTestimonialsToTestimonialsDto(x)).ToList();
            PaginationDTO<TestimonialsDTO> pagingResponse = new()
            {
                CurrentPage = page,
                TotalItems = count,
                TotalPages = totalPages,
                PrevPage = prevPage,
                NextPage = nextPage,
                Items = testimonialDto.ToList()
            };
            return pagingResponse;
        }
        #endregion
    }
}
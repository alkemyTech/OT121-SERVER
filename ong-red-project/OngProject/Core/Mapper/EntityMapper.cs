using System;
using System.Collections;
using System.Collections.Generic;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.CategoriesDTOs;
using OngProject.Core.DTOs.CommentsDTOs;
using OngProject.Core.DTOs.NewsDTOs;
using OngProject.Core.DTOs.SlidesDTOs;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Entities;
using OngProject.Infrastructure.Repositories;

namespace OngProject.Core.Mapper
{
    public class EntityMapper
    {
        #region News Mappers

        public NewsDTO FromNewsToNewsDTO(News news)
        {
            var newsDTO = new NewsDTO()
            {
                Name = news.Name,
                Content = news.Content,
                Image = news.Image,
                CategoryId = news.CategoryId
            };
            return newsDTO;
        }

        public News FromNewsDTOtoNews(NewsDTO newsDTO)
        {
            var news = new News()
            {
                Name = newsDTO.Name,
                Content = newsDTO.Content,
                Image = newsDTO.Image,
                CategoryId = newsDTO.CategoryId
            };
            return news;
        }

        public News FromNewsUpdateDTOtoNews(NewsUpdateDTO newsDTO)
        {
            var news = new News()
            {
                Id = newsDTO.Id,
                Name = newsDTO.Name,
                Content = newsDTO.Content,
                Image = newsDTO.Image,
                CategoryId = newsDTO.CategoryId
            };
            return news;
        }

        #endregion News Mappers

        #region Member Mapper

        public MembersDTO FromMembersToMembersDto(Member member)
        {
            var membersDTO = new MembersDTO()
            {
                Name = member.Name,
                FacebookUrl = member.FacebookUrl,
                InstagramUrl = member.InstagramUrl,
                LinkedinUrl = member.LinkedinUrl,
                Image = member.Image,
                Description = member.Description
            };
            return membersDTO;
        }

        public Member FromMembersDTOtoMember(MemberInsertDTO membersDTO)
        {
            return new Member()
            {
                Name = membersDTO.Name,
                FacebookUrl = membersDTO.FacebookUrl,
                InstagramUrl = membersDTO.InstagramUrl,
                LinkedinUrl = membersDTO.LinkedinUrl,
                Image = membersDTO.Image.ToString(),
                Description = membersDTO.Description
            };
        }

        #endregion Member Mapper

        #region Contact Mappers

        public ContactDTO FromContactsToContactsDto(Contacts contact)
        {
            var contactDTO = new ContactDTO()
            {
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Message = contact.Message
            };
            return contactDTO;
        }

        #endregion Contact Mappers

        #region Organization Mappers

        public OrganizationsDTO FromOrganizationToOrganizationDto(Organizations organization)
        {
            return new OrganizationsDTO
            {
                Name = organization.Name,
                Image = organization.Image,
                Phone = organization.Phone,
                Address = organization.Address
            };
        }

        public OrganizationsGetDTO FromOrganizationToOrganizationGetDto(Organizations organization)
        {
            return new OrganizationsGetDTO
            {
                Name = organization.Name,
                Image = organization.Image,
                Phone = organization.Phone,
                Address = organization.Address,
                FacebookUrl = organization.FacebookUrl,
                InstagramUrl = organization.InstagramUrl,
                LinkedinUrl = organization.LinkedinUrl
            };
        }

        #endregion Organization Mappers

        #region User Mappers

        public UserRegistrationDTO FromUserToUserRegistrationDto(User user)
        {
            return new UserRegistrationDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password
            };
        }

        public UserLoginResponseDTO FromUserToUserLoginResponseDto(User user)
        {
            return new UserLoginResponseDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Photo = user.Photo,
                Role = user.Role.Description,
            };
        }

        #endregion User Mappers

        #region Activities Mappers

        public ActivitiesDTO FromActivitiesToActivitiesDTO(Activities activities)
        {
            var activitiesDTO = new ActivitiesDTO()
            {
                Name = activities.Name,
                Content = activities.Content,
                Image = activities.Image
            };
            return activitiesDTO;
        }

        public Activities FromActivitiesDTOToActivities(ActivitiesDTO activitiesDTO)
        {
            var activities = new Activities()
            {
                Name = activitiesDTO.Name,
                Content = activitiesDTO.Content,
                Image = activitiesDTO.Image
            };
            return activities;
        }

        #endregion Activities Mappers

        #region Comments Mappers

        public CommentCreateRequestDTO FromCommentsToCommentsDTO(Comments comments)
        {
            return new CommentCreateRequestDTO()
            {
                User_id = comments.UserId,
                Body = comments.Body,
                News_id = comments.NewId
            };
        }

        #endregion Comments Mappers

        #region Categories Mappers

        public CategoryGetDTO FromCategoriesToCategoriesDTO(Category category)
        {
            return new CategoryGetDTO()
            {
                Name = category.Name,
                Description = category.Description,
                Image = category.Image
            };
        }

        public CategoryGetDTO FromCategoryInsertDTOToCategoryGetDTO(CategoryInsertDTO category, string imageUrl)
        {
            return new CategoryGetDTO()
            {
                Name = category.Name,
                Description = category.Description,
                Image = imageUrl
            };
        }

        public Category FromCategoryUpdateDTOToCategory(CategoryUpdateDTO newCategory, Category oldCategory, string imageUrl)
        {
            oldCategory.Name = newCategory.Name != null && newCategory.Name != String.Empty ? newCategory.Name : oldCategory.Name;
            oldCategory.Description = newCategory.Description != String.Empty && newCategory.Description != null ? newCategory.Description : oldCategory.Description;
            oldCategory.Image = imageUrl != String.Empty && imageUrl != null ? imageUrl : oldCategory.Image;
            return oldCategory;
        }

        public Category FromCategoryGetDTOToCategory(CategoryGetDTO category)
        {
            return new Category()
            {
                Name = category.Name,
                Description = category.Description,
                Image = category.Image
            };
        }

        #endregion Categories Mappers

        #region Slides Mappers

        public SlideDataShortResponse FromSlidesToSlidesShortResponseDTO(Slides slide)
        {
            var result = new SlideDataShortResponse()
            {
                Id = slide.Id,
                ImageUrl = slide.ImageUrl,
                Text = slide.Text,
                Order = slide.Order
            };
            return result;
        }

        public SlideDataFullResponse FromSlideToSlidesFullResponseDTO(Slides slide)
        {
            var result = new SlideDataFullResponse()
            {
                Id = slide.Id,
                ImageUrl = slide.ImageUrl,
                Text = slide.Text,
                Order = slide.Order,
                CreatedAt = slide.CreatedAt,
                OrganizationId = slide.OrganizationId
            };
            return result;
        }

        public Slides FromEntryDTOtoSlide(SlideDTO model)
        {
            return new Slides(){
                Text = model.Text,
                Order = (int) model.Order,
                OrganizationId = (int) model.OrganizationId,               
                CreatedAt = model.CreatedAt,
                IsDeleted = false
            };
        }

        #endregion Slides Mappers
    }
}


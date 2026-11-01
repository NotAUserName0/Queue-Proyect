using Application.Models.Dto;
using Domain.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Queries.User
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ResponseModel<List<UserDto>>>
    {
        private readonly IUserRepository _userRepository;
        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ResponseModel<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var (users, totalRecords) = await _userRepository.GetAllAsync(request.page, request.pageSize, cancellationToken);
            var userDtos = users.Select(u => new UserDto
            (
                id: u.Id,
                name: u.Name,
                email: u.Email
            )).ToList();
         
            return new ResponseModel<List<UserDto>>
            {
                Data = userDtos,
                TotalRecords = totalRecords,
                Success = true
            };
        }
    }
}

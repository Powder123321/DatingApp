﻿using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;


namespace API.Data
{
    public class UserRepository : IuserRepository
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper){
_mapper = mapper;
_context = context;

        }

         public  async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users.Where(x=>x.UserName == username)
           .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
           return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
           return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUserAsync()
        {
           return await _context.Users.Include(x => x.Photos)
           .ToListAsync();
        }
         public async Task<bool> SaveAllAsync()
        {
            return await  _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
           return await _context.Users
           .ProjectTo<MemberDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }

  
}

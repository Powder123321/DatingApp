﻿using System.Security.Claims;
using API.Dto;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[Authorize]

        public class UsersController : BaseApiController{



private readonly IuserRepository _userRepository;
private readonly IMapper _mapper;

private readonly IPhotoService _photoService;
public  UsersController(IuserRepository userRepository, IMapper mapper, IPhotoService photoService)
{

   _mapper = mapper;
   _userRepository = userRepository;
   _photoService = photoService;
     }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){
            
            var users = await _userRepository.GetMembersAsync();
          
            return Ok(users);
        }
           
            [HttpGet("{username}")]

            public async Task<ActionResult<MemberDto>> GetUser(string username){
                  return   await _userRepository.GetMemberAsync(username);



         

             
           }
              [HttpPut]
              public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
             
                var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

                if(user == null) return NotFound();


_mapper.Map(memberUpdateDto, user);

if(await _userRepository.SaveAllAsync()) return NoContent();

return BadRequest("Failed to update user");


              }

  [HttpPost("add-photo")]
  public async Task<ActionResult<PhotoDto>>AddPhoto(IFormFile file){
    var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

if(user == null) return NotFound();

var result = await _photoService.AddPhotoAsync(file);
if(result.Error != null) return BadRequest(result.Error.Message);
var photo = new Photo{
  Url = result.SecureUrl.AbsoluteUri,
  PublicId = result.PublicId
};

  if(user.Photos.Count == 0) photo.IsMain = true;
  user.Photos.Add(photo);

  if(await _userRepository.SaveAllAsync()) {
    return CreatedAtAction(nameof(GetUser), new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
  }
  return BadRequest("Problem adding photo");

  }



}

